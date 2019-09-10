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

            SharedSecret sharedSecret = SharedSecret.Import(Encoding.UTF8.GetBytes(input.Owner + "1950Labs"));
            HkdfSha512 kdf = new HkdfSha512();
            var key = kdf.DeriveKey(sharedSecret, null, null, Ed25519.Ed25519);
            var sender = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            string senderHex = hex.EncodeData(Sha3.Sha3256().ComputeHash(sender));

            var existsAccount = GetAccountInfoFromBackend(new GetAccountInfoIn { Address = senderHex }).Account != null;

            if (!existsAccount)
            {
                HttpClient client = new HttpClient();

                var content = new FormUrlEncodedContent(new Dictionary<string, string>());

                var response = await client.PostAsync("http://faucet.testnet.libra.org/?amount=100000&address=" + senderHex, content);

                var responseString = await response.Content.ReadAsStringAsync();

                Account account = new Account
                {
                    Address = sender,
                    AddressHashed = senderHex,
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
    }

}
