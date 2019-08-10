using Google.Protobuf;
using Grpc.Core;
using Multiformats.Hash;
using Multiformats.Hash.Algorithms;
using System;
using System.Linq;
using System.Text;
using Types;
using NBitcoin.DataEncoders;
using NSec.Cryptography;
using System.Security.Cryptography;
using System.IO;
using SHA3.Net;
using System.Collections.Generic;

namespace LibraClient
{
    class Program
    {

        const string LIBRA_HASH_SUFFIX = "@@$$LIBRA$$@@";
        const string RAWTX_HASH_SALT = "RawTransaction";
        static void Main(string[] args)
        {
            
            Channel channel = new Channel("ac.testnet.libra.org:8000", ChannelCredentials.Insecure);
            var client = new AdmissionControl.AdmissionControl.AdmissionControlClient(channel);

            HexEncoder hex = new HexEncoder();

            SharedSecret sharedSecret = SharedSecret.Import(Encoding.UTF8.GetBytes("dummy"));
            HkdfSha512 kdf = new HkdfSha512();
            var key = kdf.DeriveKey(sharedSecret, null, null, Ed25519.Ed25519);
            var sender = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            UInt64 seqNum = 4;
            string senderHex = hex.EncodeData(Sha3.Sha3256().ComputeHash(sender));

            var rawTx = CreateRawTx(senderHex, seqNum, "da031ffc817fd7694095a6891eaf0f0af1c8ef2907f94fba2f2338d241162649", 0xffffUL, 10000UL, 0, 0UL);

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


            var reply = client.SubmitTransaction(submitTxReq);
            Console.WriteLine($"Reply AcStatus {reply.AcStatus.Code}.");

            try
            {
               GetTransaction(client, senderHex, seqNum);
               //GetTransaction(client, "0197fa564143feee71f1308da2721e307fc2e0c8d8a0313e4bb67acc9269eba6", 4);
            }
            catch (Exception excp)
            {
                Console.WriteLine($"Exception Main. {excp.Message}");
            }
        }

        private static Types.RawTransaction CreateRawTx(string senderHex, UInt64 seqNum, string receipientHex, UInt64 recipientAmount, UInt64 maxGasAmount, UInt64 maxGasUnitPrice, UInt64 expirationTime)
        {
            HexEncoder hex = new HexEncoder();

            Types.RawTransaction rawTx = new Types.RawTransaction();
            rawTx.SenderAccount = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(senderHex));
            rawTx.SequenceNumber = seqNum;
            rawTx.Program = new Types.Program();
            rawTx.Program.Code = Google.Protobuf.ByteString.CopyFrom(Convert.FromBase64String("TElCUkFWTQoBAAcBSgAAAAQAAAADTgAAAAYAAAAMVAAAAAYAAAANWgAAAAYAAAAFYAAAACkAAAAEiQAAACAAAAAHqQAAAA4AAAAAAAABAAIAAQMAAgACBAIAAwADAgQCBjxTRUxGPgxMaWJyYUFjY291bnQEbWFpbg9wYXlfZnJvbV9zZW5kZXIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAgEEAAwADAERAQAC"));

            var recipientArg = new Types.TransactionArgument { Type = Types.TransactionArgument.Types.ArgType.Address };
            recipientArg.Data = Google.Protobuf.ByteString.CopyFrom(hex.DecodeData(receipientHex));
            rawTx.Program.Arguments.Add(recipientArg);

            var amountArg = new Types.TransactionArgument { Type = Types.TransactionArgument.Types.ArgType.U64 };
            amountArg.Data = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes(recipientAmount));
            rawTx.Program.Arguments.Add(amountArg);

            rawTx.MaxGasAmount = maxGasAmount;
            rawTx.GasUnitPrice = maxGasUnitPrice;
            rawTx.ExpirationTime = expirationTime;

            return rawTx;
        }


        private static void GetTransaction(AdmissionControl.AdmissionControl.AdmissionControlClient client, string accountHex, UInt64 seqNum)
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

                    Types.RawTransaction rawTx = Types.RawTransaction.Parser.ParseFrom(signedTx.SignedTransaction.RawTxnBytes);

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
        }
    }
}


