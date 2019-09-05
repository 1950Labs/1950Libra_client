using AdmissionControl;
using Google.Protobuf;
using Grpc.Core;
using LibraReactClient.BusinessLayer.Entities;
using NBitcoin.DataEncoders;
using NSec.Cryptography;
using SHA3.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace LibraReactClient.BusinessLayer.Logic
{
    public class LibraTransactions
    {
        const string LIBRA_HASH_SUFFIX = "@@$$LIBRA$$@@";
        const string RAWTX_HASH_SALT = "RawTransaction";

        
        

        public RawTransaction SubmitTransaction(SubmitTransactionIn input)
        {
            var client = new AdmissionControlEntity().Client;

            LibraAccounts libraAccountsComponent = new LibraAccounts();

            var account = libraAccountsComponent.GetAccountInfo(new GetAccountInfoIn
            {
                AccountId = input.SourceAccountId
            }).Account;

            HexEncoder hex = new HexEncoder();

            SharedSecret sharedSecret = SharedSecret.Import(Encoding.UTF8.GetBytes(account.Owner + "1950Labs"));
            HkdfSha512 kdf = new HkdfSha512();
            var key = kdf.DeriveKey(sharedSecret, null, null, Ed25519.Ed25519);
            var sender = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            UInt64 seqNum = account.SequenceNumber;
            string senderHex = hex.EncodeData(Sha3.Sha3256().ComputeHash(sender));

            var rawTx = CreateRawTx(senderHex, seqNum, input.Recipient, Convert.ToUInt64(input.Amount * 1000000), 1000UL, 10000UL);

            Console.WriteLine($"RawTx: {Convert.ToBase64String(rawTx.ToByteArray())}");

            Types.SignedTransaction signedTx = new Types.SignedTransaction();
            signedTx.SenderPublicKey = Google.Protobuf.ByteString.CopyFrom(sender);
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

            Console.WriteLine($"Submitting signed tx for {senderHex} and seqnum {seqNum}.");


            SubmitTransactionResponse reply = client.SubmitTransaction(submitTxReq);
            Console.WriteLine($"Reply AcStatus {reply.AcStatus.Code}.");
            Types.RawTransaction resultTx = null;
            try
            {
                Task.Delay(2000).Wait();
                resultTx = GetTransaction(client, senderHex, seqNum);
            }
            catch (Exception excp)
            {
                Console.WriteLine($"Exception Main. {excp.Message}");
            }

            return resultTx;
        }

        private Types.RawTransaction CreateRawTx(string senderHex, UInt64 seqNum, string receipientHex, UInt64 recipientAmount, UInt64 maxGasAmount, UInt64 maxGasUnitPrice)
        {
            HexEncoder hex = new HexEncoder();

            byte[] PtPTrxBytecode = new byte[] { 76, 73, 66, 82, 65, 86, 77, 10, 1, 0, 7, 1, 74, 0, 0, 0, 4, 0, 0, 0, 3, 78, 0, 0, 0, 6, 0, 0, 0, 12, 84, 0, 0, 0, 6, 0, 0, 0, 13, 90, 0, 0, 0, 6, 0, 0, 0, 5, 96, 0, 0, 0, 41, 0, 0, 0, 4, 137, 0, 0, 0, 32, 0, 0, 0, 7, 169, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 1, 3, 0, 2, 0, 2, 4, 2, 0, 3, 0, 3, 2, 4, 2, 6, 60, 83, 69, 76, 70, 62, 12, 76, 105, 98, 114, 97, 65, 99, 99, 111, 117, 110, 116, 4, 109, 97, 105, 110, 15, 112, 97, 121, 95, 102, 114, 111, 109, 95, 115, 101, 110, 100, 101, 114, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 1, 4, 0, 12, 0, 12, 1, 19, 1, 0, 2 };

            Types.RawTransaction rawTx = new Types.RawTransaction();
            rawTx.SenderAccount = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(senderHex));
            rawTx.SequenceNumber = seqNum;
            rawTx.Program = new Types.Program();
            //rawTx.Program.Code = Google.Protobuf.ByteString.CopyFrom(Convert.FromBase64String("TElCUkFWTQoBAAcBSgAAAAQAAAADTgAAAAYAAAAMVAAAAAYAAAANWgAAAAYAAAAFYAAAACkAAAAEiQAAACAAAAAHqQAAAA4AAAAAAAABAAIAAQMAAgACBAIAAwADAgQCBjxTRUxGPgxMaWJyYUFjY291bnQEbWFpbg9wYXlfZnJvbV9zZW5kZXIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAgEEAAwADAERAQAC"));
            rawTx.Program.Code = Google.Protobuf.ByteString.CopyFrom(PtPTrxBytecode);
            var recipientArg = new Types.TransactionArgument { Type = Types.TransactionArgument.Types.ArgType.Address };
            recipientArg.Data = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(receipientHex));
            rawTx.Program.Arguments.Add(recipientArg);

            var amountArg = new Types.TransactionArgument { Type = Types.TransactionArgument.Types.ArgType.U64 };
            amountArg.Data = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes(recipientAmount));
            rawTx.Program.Arguments.Add(amountArg);

            rawTx.MaxGasAmount = maxGasAmount;
            rawTx.GasUnitPrice = maxGasUnitPrice;
            rawTx.ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds();
            return rawTx;
        }

        private RawTransaction GetTransaction(AdmissionControl.AdmissionControl.AdmissionControlClient client, string accountHex, UInt64 seqNum)
        {
            Console.WriteLine($"GetTransaction for {accountHex} and seqnum {seqNum}.");

            HexEncoder hex = new HexEncoder();

            Types.UpdateToLatestLedgerRequest updToLatestLedgerReq = new Types.UpdateToLatestLedgerRequest();
            var getTxReq = new Types.GetAccountTransactionBySequenceNumberRequest();
            getTxReq.SequenceNumber = seqNum;
            getTxReq.Account = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(accountHex));
            Types.RequestItem reqItem = new Types.RequestItem();
            reqItem.GetAccountTransactionBySequenceNumberRequest = getTxReq;
            updToLatestLedgerReq.RequestedItems.Add(reqItem);
            var reply = client.UpdateToLatestLedger(updToLatestLedgerReq);
            Types.RawTransaction rawTx = null;

            if (reply?.ResponseItems?.Count == 1)
            {
                var resp = reply.ResponseItems[0].GetAccountTransactionBySequenceNumberResponse;

                if (resp.SignedTransactionWithProof == null)
                {
                    Console.WriteLine("GetTransaction request did not return a signed transaction.");
                }
                else
                {
                    var signedTx = resp.SignedTransactionWithProof;

                    Console.WriteLine($"Sender {hex.EncodeData(signedTx.SignedTransaction.SenderPublicKey.ToByteArray())}.");
                    Console.WriteLine($"RawTxnBytes {hex.EncodeData(signedTx.SignedTransaction.RawTxnBytes.ToByteArray())}");

                    rawTx = Types.RawTransaction.Parser.ParseFrom(signedTx.SignedTransaction.RawTxnBytes);

                    Console.WriteLine($"SequenceNumber {rawTx.SequenceNumber}.");
                    Console.WriteLine($"MaxGasAmount {rawTx.MaxGasAmount}.");
                    Console.WriteLine($"GasUnitPrice {rawTx.GasUnitPrice}.");
                    Console.WriteLine($"ExpirationTime {rawTx.ExpirationTime}.");

                    var byteCode = rawTx.Program.Code.ToByteArray();

                    Console.WriteLine($"Program.Code base64 {Convert.ToBase64String(byteCode)}.");

                    SHA512 sha512 = SHA512.Create();
                    var byteCodeHash = hex.EncodeData(sha512.ComputeHash(byteCode));
                    Console.WriteLine($"Program.Code hash {byteCodeHash}.");
                }
            }
            else
            {
                Console.WriteLine("GetTransaction did not return a result.");
            }

            return rawTx;
        }

        public async Task<MintOut> MintAsync(MintIn input)
        {
            HttpClient client = new HttpClient();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>());

            var response = await client.PostAsync("http://faucet.testnet.libra.org/?amount=" + (input.Amount * 1000000) + "&address=" + input.Address, content);

            var responseString = response.Content.ReadAsStringAsync();

            MintOut output = new MintOut();

            if(response.StatusCode == System.Net.HttpStatusCode.OK && response.IsSuccessStatusCode)
            {
                output.Minted = true;
            }

            return output;
            
        }
    }
}
