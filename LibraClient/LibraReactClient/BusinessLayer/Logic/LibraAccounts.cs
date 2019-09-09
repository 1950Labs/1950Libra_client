using AdmissionControl;
using Google.Protobuf;
using Grpc.Core;
using LibraReactClient.BusinessLayer.Entities;
using LibraReactClient.DataAccess;
using NBitcoin.DataEncoders;
using NSec.Cryptography;
using SHA3.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Logic
{

    public class LibraAccounts
    {
        public readonly string AssetType = "217da6c6b3e19f1825cfb2676daecce3bf3de03cf26647c78df00b371b25cc97";
        const string LIBRA_HASH_SUFFIX = "@@$$LIBRA$$@@";
        const string RAWTX_HASH_SALT = "RawTransaction";

        public async Task<AddAccountOut> AddAccountAsync(AddAccountIn input)
        {
            AddAccountOut output = new AddAccountOut { OperationSuccess = false };

            HexEncoder hex = new HexEncoder();

            SharedSecret sharedSecret = SharedSecret.Import(Encoding.UTF8.GetBytes("O11950Labs"));
            HkdfSha512 kdf = new HkdfSha512();
            var key = kdf.DeriveKey(sharedSecret, null, null, Ed25519.Ed25519);
            var address = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            string senderHex = hex.EncodeData(Sha3.Sha3256().ComputeHash(address));

            SharedSecret sharedSecret2 = SharedSecret.Import(Encoding.UTF8.GetBytes(input.Owner + "1950Labs"));
            HkdfSha512 kdf2 = new HkdfSha512();
            var key2 = kdf2.DeriveKey(sharedSecret2, null, null, Ed25519.Ed25519);
            var address2 = key2.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            string addressHex = hex.EncodeData(Sha3.Sha3256().ComputeHash(address2));

            var existsAccount = GetAccountInfoFromBackend(new GetAccountInfoIn { Address = addressHex }).Account != null;

            if (!existsAccount)
            {
                var client = new AdmissionControlEntity().Client;

                var senderAccount = GetAccountInfoFromBackend(new GetAccountInfoIn { Address = senderHex }).Account;

                var rawTx = CreateRawAc(senderHex, addressHex, senderAccount.SequenceNumber, 100000, 0);

                Console.WriteLine($"RawTx: {Convert.ToBase64String(rawTx.ToByteArray())}");

                Types.SignedTransaction signedTx = new Types.SignedTransaction();
                signedTx.SenderPublicKey = Google.Protobuf.ByteString.CopyFrom(address);
                signedTx.RawTxnBytes = rawTx.ToByteString();

                var seed = Encoding.ASCII.GetBytes(RAWTX_HASH_SALT + LIBRA_HASH_SUFFIX);
                var seedHash = Sha3.Sha3256().ComputeHash(seed);
                List<byte> hashInput = new List<byte>();
                hashInput.AddRange(seedHash);
                hashInput.AddRange(signedTx.RawTxnBytes.ToArray());
                var hash = Sha3.Sha3256().ComputeHash(hashInput.ToArray());

                Console.WriteLine($"Raw tx hash {hex.EncodeData(hash)}.");

                var sig = NSec.Cryptography.Ed25519.Ed25519.Sign(key, hash);
                signedTx.SenderSignature = Google.Protobuf.ByteString.CopyFrom(sig);

                Console.WriteLine($"Signature {hex.EncodeData(sig)}.");

                AdmissionControl.SubmitTransactionRequest submitTxReq = new AdmissionControl.SubmitTransactionRequest();
                submitTxReq.SignedTxn = signedTx;

                Console.WriteLine($"Submitting signed tx for {addressHex}.");

                SubmitTransactionResponse reply = client.SubmitTransaction(submitTxReq, new Metadata());
                Console.WriteLine($"Reply AcStatus {reply.AcStatus.Code}.");
                Types.RawTransaction resultTx = null;
                try
                {
                    Task.Delay(2000).Wait();
                    //resultTx = GetTransaction(client, addressHex, seqNum);
                }
                catch (Exception excp)
                {
                    Console.WriteLine($"Exception Main. {excp.Message}");
                }
                                
                Account account = new Account
                {
                    Address = address,
                    AddressHashed = addressHex,
                    Owner = input.Owner,
                    UserUID = input.UserUID
                };

                using (var db = new LibraContext())
                {
                    db.Accounts.Add(account);
                    var count = db.SaveChanges();
                    if (count > 0)
                    {
                        output.Account = account;
                        output.OperationSuccess = true;
                    }
                }
            }

            return output;
        }

        public IEnumerable<Account> GetAccounts(GetAccountsIn input)
        {
            List<Account> accounts = new List<Account>();
            using (var db = new LibraContext())
            {
                var accounts_db = db.Accounts.Where(a => a.UserUID.Equals(input.UserUID));
                if (accounts_db != null) {
                    foreach (var account in accounts_db)
                    {
                        accounts.Add(new Account
                        {
                            AccountId = account.AccountId,
                            AddressHashed = account.AddressHashed,
                            Owner = account.Owner
                        });
                    }
                }
            }

            return accounts;
        }


        public GetAccountInfoOut GetAccountInfoFromBackend(GetAccountInfoIn input)
        {
            HexEncoder hex = new HexEncoder();
            GetAccountInfoOut output = new GetAccountInfoOut();
            var client = new AdmissionControlEntity().Client;

            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var asr = new Types.GetAccountStateRequest();
            asr.Address = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(input.Address));
            requestItem.GetAccountStateRequest = asr;
            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);

            var result = client.UpdateToLatestLedger(
                updateToLatestLedgerRequest, new Metadata());

            Types.AccountStateBlob blob = result.ResponseItems?.FirstOrDefault().GetAccountStateResponse?.AccountStateWithProof?.Blob;
            if(blob != null)
            {
                output.Account = DeserializeBlob(blob.Blob.ToByteArray(), input.Address);
            }
           
            return output;
        }

        public GetAccountInfoOut GetAccountInfo(GetAccountInfoIn input)
        {
            AccountExtendedData account;
            using (var db = new LibraContext())
            {
                account = new AccountExtendedData(db.Accounts.Where(a => a.AccountId == input.AccountId).SingleOrDefault());
                if(account != null)
                {
                    AccountExtendedData backendInformation = GetAccountInfoFromBackend(new GetAccountInfoIn
                    {
                        Address = account.AddressHashed
                    }).Account;

                    if (backendInformation != null)
                    {
                        account.SequenceNumber = backendInformation.SequenceNumber;
                        account.SentEventsCount = backendInformation.SentEventsCount;
                        account.ReceivedEventsCount = backendInformation.ReceivedEventsCount;
                        account.Balance = backendInformation.Balance;
                    }
                }
            }

            return new GetAccountInfoOut { Account = account };  
        }


        /// <summary>
        /// Blob to AccountResource
        /// </summary>
        /// <param name="bytes">UTF8</param>
        public AccountExtendedData DeserializeBlob(byte[] bytes, string address)
        {
            AccountExtendedData account = new AccountExtendedData();

            int startIndex = GetAssetTypeStartIndex(bytes, address);

            startIndex += 9;
            account.AuthenticationKey = BitConverter.ToString(SubArray(bytes, startIndex, 32)).Replace("-", "").ToLower();
            startIndex += 32;
            account.Balance = (decimal)BitConverter.ToUInt64(SubArray(bytes, startIndex, 8)) / 1000000;
            startIndex += 9;// 8+1 self.delegated_withdrawal_capability
            account.ReceivedEventsCount = BitConverter.ToUInt64(SubArray(bytes, startIndex, 8));

            startIndex += 44;
            account.SentEventsCount = BitConverter.ToUInt64(SubArray(bytes, startIndex, 8));

            startIndex += 44;
            account.SequenceNumber = BitConverter.ToUInt64(SubArray(bytes, startIndex, 8));

            return account;
        }

        private int GetAssetTypeStartIndex(byte[] bytes, string address)
        {
            List<byte> assetTypeBytes = new List<byte>();
            int startIndex = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                var item = bytes[i];

                if (assetTypeBytes.Count == 32)
                    assetTypeBytes.Remove(assetTypeBytes.FirstOrDefault());
                assetTypeBytes.Add(item);

                if (assetTypeBytes.SequenceEqual(HexStringToByteArray(AssetType)))
                {
                    startIndex = i;
                    continue;
                }
            }

            return startIndex;
        }

        public byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private Types.RawTransaction CreateRawAc(string senderHex, string addressHex, UInt64 seqNum, UInt64 maxGasAmount, UInt64 maxGasUnitPrice)
        {
            HexEncoder hex = new HexEncoder();

            byte[] CreateAccountByteCode = new byte[] { 76, 73, 66, 82, 65, 86, 77, 10, 1, 0, 7, 1, 74, 0, 0, 0, 4, 0, 0, 0, 3, 78, 0, 0, 0, 6, 0, 0, 0, 12, 84, 0, 0, 0, 6, 0, 0, 0, 13, 90, 0, 0, 0, 6, 0, 0, 0, 5, 96, 0, 0, 0, 44, 0, 0, 0, 4, 140, 0, 0, 0, 32, 0, 0, 0, 7, 172, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 1, 3, 0, 2, 0, 2, 4, 2, 0, 3, 0, 3, 2, 4, 2, 6, 60, 83, 69, 76, 70, 62, 12, 76, 105, 98, 114, 97, 65, 99, 99, 111, 117, 110, 116, 4, 109, 97, 105, 110, 18, 99, 114, 101, 97, 116, 101, 95, 110, 101, 119, 95, 97, 99, 99, 111, 117, 110, 116, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 1, 4, 0, 12, 0, 12, 1, 19, 1, 0, 2 };
            
            Types.RawTransaction rawTx = new Types.RawTransaction();
            rawTx.SenderAccount = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(senderHex));
            rawTx.SequenceNumber = seqNum;
            rawTx.Program = new Types.Program();
            //rawTx.Program.Code = Google.Protobuf.ByteString.CopyFrom(Convert.FromBase64String("TElCUkFWTQoBAAcBSgAAAAQAAAADTgAAAAYAAAAMVAAAAAYAAAANWgAAAAYAAAAFYAAAACkAAAAEiQAAACAAAAAHqQAAAA4AAAAAAAABAAIAAQMAAgACBAIAAwADAgQCBjxTRUxGPgxMaWJyYUFjY291bnQEbWFpbg9wYXlfZnJvbV9zZW5kZXIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAgEEAAwADAERAQAC"));
            rawTx.Program.Code = Google.Protobuf.ByteString.CopyFrom(CreateAccountByteCode);

            var addressArg = new Types.TransactionArgument { Type = Types.TransactionArgument.Types.ArgType.Address };
            addressArg.Data = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(addressHex));
            rawTx.Program.Arguments.Add(addressArg);

            var amountArg = new Types.TransactionArgument { Type = Types.TransactionArgument.Types.ArgType.U64 };
            amountArg.Data = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes((ulong)0));
            rawTx.Program.Arguments.Add(amountArg);

            rawTx.MaxGasAmount = maxGasAmount;
            rawTx.GasUnitPrice = maxGasUnitPrice;
            rawTx.ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds();

            return rawTx;
        }
    }

}
