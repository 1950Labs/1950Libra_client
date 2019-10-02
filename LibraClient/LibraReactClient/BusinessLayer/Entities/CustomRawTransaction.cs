using LibraReactClient.BusinessLayer.Common;
using LibraReactClient.BusinessLayer.Enums;
using LibraReactClient.BusinessLayer.LCSLogic;
using LibraReactClient.BusinessLayer.LCSTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class CustomRawTransaction
    {
        public ulong ExpirationTimeUnix { get; set; }
        public DateTime ExpirationTime { get; set; }
        public ulong GasUnitPrice { get; set; }
        public ulong MaxGasAmount { get; set; }
        public TransactionPayloadLCSEnum PayloadCase { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public ulong Amount { get; set; }
        public ulong SequenceNumber { get; set; }
        public CustomProgram Program { get; set; }

        public ulong VersionId { get; set; }

        public byte[] RawTxnBytes { get { return _rawTxnBytes; } }
        byte[] _rawTxnBytes;
        public CustomRawTransaction()
        {
        }

        public CustomRawTransaction(byte[] rawTxnBytes)
        {
            DeserializeRawTransaction(rawTxnBytes);
        }

        private void DeserializeRawTransaction(byte[] rawTxnBytes)
        {
            _rawTxnBytes = rawTxnBytes;
            int cursor = 0;
            RawTransactionLCS rawTr = _rawTxnBytes.LCSDeserialization<RawTransactionLCS>(ref cursor);

            ExpirationTimeUnix = rawTr.ExpirationTime;
            ExpirationTime = rawTr.ExpirationTime.UnixTimeStampToDateTime();

            GasUnitPrice = rawTr.GasUnitPrice;
            MaxGasAmount = rawTr.MaxGasAmount;
            PayloadCase = rawTr.TransactionPayload.PayloadTypeEnum;

            Sender = rawTr.Sender.Value;

            var programCode = rawTr.TransactionPayload.PayloadTypeEnum;
            if (rawTr.TransactionPayload.PayloadTypeEnum == TransactionPayloadLCSEnum.Program)
            {
                var args = rawTr.TransactionPayload.Program.TransactionArguments.ToArray();
                if (args.Count() == 2 &&
                    args[0].ArgTypeEnum == TransactionArgumentLCSEnum.Address &&
                    args[1].ArgTypeEnum == TransactionArgumentLCSEnum.U64)
                {
                    Receiver = args[0].Address.Value;
                    Amount = args[1].U64;
                }
            }

            SequenceNumber = rawTr.SequenceNumber;

            if (rawTr.TransactionPayload.PayloadTypeEnum ==
               TransactionPayloadLCSEnum.Script &&
               Utilities.IsPtPOrMint(rawTr.TransactionPayload.Script.Code))
            {
                var args = rawTr.TransactionPayload.Script
                    .TransactionArguments.ToArray();
                if (args.Count() == 2 &&
                    args[0].ArgTypeEnum == TransactionArgumentLCSEnum.Address &&
                    args[1].ArgTypeEnum == TransactionArgumentLCSEnum.U64)
                {
                    Receiver = args[0].Address.Value;
                    Amount = args[1].U64;
                }
            }

            if (rawTr.TransactionPayload.PayloadTypeEnum ==
                TransactionPayloadLCSEnum.Program)
            {
                List<CustomTransactionArgument> arguments =
                    new List<CustomTransactionArgument>();
                foreach (var item in rawTr.TransactionPayload.Program
                    .TransactionArguments)
                {
                    GerArg(arguments, item);
                }

                List<byte[]> modules = new List<byte[]>();

                foreach (var item in rawTr.TransactionPayload.Program.Modules)
                    modules.Add(item);

                Program = new CustomProgram()
                {
                    PayloadType = (TransactionPayloadEnum)rawTr.TransactionPayload
                    .PayloadTypeEnum,
                    Arguments = arguments,
                    Code = rawTr.TransactionPayload.Program.Code,
                    Modules = modules
                };
            }
            else if (rawTr.TransactionPayload.PayloadTypeEnum ==
                TransactionPayloadLCSEnum.Script)
            {
                List<CustomTransactionArgument> arguments =
                    new List<CustomTransactionArgument>();
                foreach (var item in rawTr.TransactionPayload.Script
                    .TransactionArguments)
                {
                    GerArg(arguments, item);
                }

                Program = new CustomProgram()
                {
                    PayloadType = (TransactionPayloadEnum)rawTr.TransactionPayload
                    .PayloadTypeEnum,
                    Arguments = arguments,
                    Code = rawTr.TransactionPayload.Script.Code
                };
            }
            else if (rawTr.TransactionPayload.PayloadTypeEnum ==
                TransactionPayloadLCSEnum.Module)
            {
                List<byte[]> modules = new List<byte[]>();
                modules.Add(rawTr.TransactionPayload.Module.Code);

                Program = new CustomProgram()
                {
                    PayloadType = (TransactionPayloadEnum)rawTr.TransactionPayload
                    .PayloadTypeEnum,
                    Modules = modules
                };
            }
        }

        private static void GerArg(List<CustomTransactionArgument> arguments, TransactionArgumentLCS item)
        {
            var transactionArgument = new CustomTransactionArgument();

            transactionArgument.ArgType = (uint)item.ArgTypeEnum;

            transactionArgument.Address = item.Address.Value;
            transactionArgument.U64 = item.U64;
            transactionArgument.String = item.String;
            transactionArgument.ByteArray = item.ByteArray;

            arguments.Add(transactionArgument);
        }

        public override string ToString()
        {
            return "{\n   ExpirationTime : " + ExpirationTime + "\n" +
                    "   GasUnitPrice : " + GasUnitPrice + "\n" +
                    "   Sender : " + Sender + "\n" +
                    "   Receiver : " + Receiver + "\n" +
                    "   Amount : " + Amount + "\n" +
                    "   SequenceNumber : " + SequenceNumber + "\n}";
        }


    }
}