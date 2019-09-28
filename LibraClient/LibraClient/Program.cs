using AdmissionControl;
using Google.Protobuf;
using Grpc.Core;
using LibraClient.Enums;
using LibraClient.LCSTypes;
using NBitcoin.DataEncoders;
using NSec.Cryptography;
using SHA3.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Types;

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

            SharedSecret sharedSecret = SharedSecret.Import(Encoding.UTF8.GetBytes("newdummy"));
            HkdfSha512 kdf = new HkdfSha512();
            var key = kdf.DeriveKey(sharedSecret, null, null, Ed25519.Ed25519);
            var sender = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            UInt64 seqNum = 9;
            string senderHex = hex.EncodeData(Sha3.Sha3256().ComputeHash(sender));

            uint amount = 10000000;
            string reciver = "4ba2555fd146e79e37fda7a2f30dc1b4f3d9228aa48b230dbab0a18d407f2f9b";

            RawTransactionLCS rawTr = new RawTransactionLCS()
            {
                ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60)
                .ToUnixTimeSeconds(),
                GasUnitPrice = 0,
                MaxGasAmount = 100000,
                SequenceNumber = seqNum
            };

            rawTr.TransactionPayload = new TransactionPayloadLCS();

            rawTr.TransactionPayload.PayloadType = (uint)TransactionPayloadLCSEnum.Script;
            rawTr.TransactionPayload.Script = new ScriptLCS()
            {
                Code = Utilities.PtPTrxBytecode,
                TransactionArguments = new List<TransactionArgumentLCS>() {
                     new TransactionArgumentLCS()
                     {
                         ArgType = (uint)TransactionArgumentLCSEnum.Address,
                         Address = new AddressLCS(reciver)
                     },
                     new TransactionArgumentLCS(){
                         ArgType = (uint)TransactionArgumentLCSEnum.U64,
                         U64 = amount
                     }
                }
            };

            rawTr.Sender = new AddressLCS(senderHex);

            var bytesTrx = LCSCore.LCSSerialization(rawTr);

            Types.SignedTransaction signedTx = new Types.SignedTransaction();
            var bytesTrxHash = Google.Protobuf.ByteString.CopyFrom(bytesTrx);

            var seed = Encoding.ASCII.GetBytes(RAWTX_HASH_SALT + LIBRA_HASH_SUFFIX);
            var seedHash = Sha3.Sha3256().ComputeHash(seed);
            List<byte> hashInput = new List<byte>();
            hashInput.AddRange(seedHash);
            hashInput.AddRange(bytesTrxHash);
            var hash = Sha3.Sha3256().ComputeHash(hashInput.ToArray());

            SubmitTransactionRequest req = new SubmitTransactionRequest();

            req.SignedTxn = new SignedTransaction();

            List<byte> retArr = new List<byte>();
            retArr = retArr.Concat(bytesTrx).ToList();

            retArr = retArr.Concat(
                LCSCore.LCSSerialization(key.Export(KeyBlobFormat.RawPublicKey))).ToList();
            var sig = SignatureAlgorithm.Ed25519.Sign(key, hash);
            retArr = retArr.Concat(LCSCore.LCSSerialization(sig)).ToList();
            req.SignedTxn.SignedTxn = ByteString.CopyFrom(retArr.ToArray());


            var result = client.SubmitTransaction(
                 req, new Metadata());

            Task.Delay(5000).Wait();
            GetTransaction(client, senderHex, seqNum);
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
                    byte[] result = signedTx.SignedTransaction.ToByteArray();
                    var deserializedResult = LCSCore.LCSDeserialization<SignedTransactionLCS>(result);
                }
            }
            else
            {
                Console.WriteLine("GetTransaction did not return a result.");
            }
        }
    }
}


