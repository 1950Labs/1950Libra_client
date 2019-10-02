using AdmissionControl;
using Google.Protobuf;
using Grpc.Core;
using LibraReactClient.BusinessLayer.Common;
using LibraReactClient.BusinessLayer.Entities;
using LibraReactClient.BusinessLayer.Enums;
using LibraReactClient.BusinessLayer.LCSLogic;
using LibraReactClient.BusinessLayer.LCSTypes;
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
        

        public SubmitTransactionOut SubmitTransaction(SubmitTransactionIn input)
        {
            var client = new AdmissionControlEntity().Client;

            SubmitTransactionOut result = new SubmitTransactionOut();

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

            RawTransactionLCS rawTx = CreateRawTx(senderHex, seqNum, input.Recipient, Convert.ToUInt64(input.Amount * 1000000), 160000, 0);

            var bytesTrx = LCSCore.LCSSerialization(rawTx);

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

            SubmitTransactionResponse reply = client.SubmitTransaction(req, new Metadata());
            Console.WriteLine($"Reply AcStatus {reply.AcStatus.Code}.");
            result.Transaction = null;

            result.ACStatus = reply.AcStatus.Code.ToString();
            result.SourceHex = senderHex;
            result.RecipientHex = input.Recipient;
            result.Amount = input.Amount;

            if (reply.AcStatus.Code == AdmissionControlStatusCode.Accepted)
            {
                try
                {
                    Task.Delay(3000).Wait();
                    result.Transaction = GetTransaction(client, senderHex, seqNum);
                }
                catch (Exception excp)
                {
                    Console.WriteLine($"Exception Main. {excp.Message}");
                }
            }
            else
            {
                result.Transaction = new CustomRawTransaction();
            }

            return result;
        }

        private RawTransactionLCS CreateRawTx(string senderHex, UInt64 seqNum, string receipientHex, UInt64 recipientAmount, UInt64 maxGasAmount, UInt64 maxGasUnitPrice)
        {
            RawTransactionLCS rawTr = new RawTransactionLCS()
            {
                ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds(),
                GasUnitPrice = maxGasUnitPrice,
                MaxGasAmount = maxGasAmount,
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
                         Address = new AddressLCS(receipientHex)
                     },
                     new TransactionArgumentLCS(){
                         ArgType = (uint)TransactionArgumentLCSEnum.U64,
                         U64 = recipientAmount
                     }
                }
            };

            rawTr.Sender = new AddressLCS(senderHex);

            return rawTr;
        }

        private CustomRawTransaction GetTransaction(AdmissionControl.AdmissionControl.AdmissionControlClient client, string accountHex, UInt64 seqNum)
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
            CustomRawTransaction rawTx = null;

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
                    byte[] result = signedTx.SignedTransaction.SignedTxn.ToByteArray();
                    rawTx = new CustomRawTransaction(result);
                    rawTx.VersionId = resp.SignedTransactionWithProof.Version;

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
