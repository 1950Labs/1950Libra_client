using LibraReactClient.BusinessLayer.Common;
using LibraReactClient.BusinessLayer.Enums;
using LibraReactClient.BusinessLayer.LCSTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraReactClient.BusinessLayer.LCSLogic
{
    public class LibraCanonicalDeserialization
    {
        public byte[] U64ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] AddressToByte(AddressLCS source)
        {
            var data = source.Value.HexStringToByteArray();
            return data;
        }

        public byte[] U32ToByte(uint source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] StringToByte(string source)
        {
            var data = Encoding.UTF8.GetBytes(source);
            var len = U32ToByte((uint)data.Length);
            return len.Concat(data).ToArray();
        }

        public byte[] ByteArrToByte(byte[] source)
        {
            var len = U32ToByte((uint)source.Length);
            var data = source;
            return len.Concat(data).ToArray();
        }

        public byte[] ListByteArrToByte(List<byte[]> source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Count);
            retArr = retArr.Concat(len).ToList();
            foreach (var item in source)
            {
                var localLen = U32ToByte((uint)item.Length);
                retArr = retArr.Concat(localLen).ToList();
                retArr = retArr.Concat(item).ToList();
            }
            return retArr.ToArray();
        }

        public byte[] BoolToByte(bool source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] TransactionPayloadToByte(TransactionPayloadLCS source)
        {
            List<byte> retArr = new List<byte>();
            var payloadType = U32ToByte(source.PayloadType);
            retArr = retArr.Concat(payloadType).ToList();

            if (source.PayloadTypeEnum == TransactionPayloadLCSEnum.Program)
            {
                retArr = retArr.Concat(ProgramToByte(source.Program)).ToList();
            }
            else if (source.PayloadTypeEnum == TransactionPayloadLCSEnum.WriteSet)
            {
                throw new Exception("WriteSet Not Supported.");
            }
            else if (source.PayloadTypeEnum == TransactionPayloadLCSEnum.Script)
            {
                retArr = retArr.Concat(ScriptToByte(source.Script)).ToList();
            }
            else if (source.PayloadTypeEnum == TransactionPayloadLCSEnum.Module)
            {
                retArr = retArr.Concat(ModuleToByte(source.Module)).ToList();
            }

            return retArr.ToArray();
        }

        public byte[] ProgramToByte(ProgramLCS source)
        {
            List<byte> retArr = new List<byte>();
            var code = ByteArrToByte(source.Code);
            retArr = retArr.Concat(code).ToList();

            var transactionArg = ListTransactionArgumentToByte(source.TransactionArguments.ToList());
            retArr = retArr.Concat(transactionArg).ToList();

            var modules = ListByteArrToByte(source.Modules);
            retArr = retArr.Concat(modules).ToList();
            return retArr.ToArray();
        }

        public byte[] ScriptToByte(ScriptLCS source)
        {
            List<byte> retArr = new List<byte>();
            var code = LCSCore.LCSSerialization(source.Code);
            retArr = retArr.Concat(code).ToList();
            var transactionArg = LCSCore.LCSSerialization(source.TransactionArguments);
            retArr = retArr.Concat(transactionArg).ToList();
            return retArr.ToArray();
        }

        public byte[] ModuleToByte(ModuleLCS source)
        {
            List<byte> retArr = new List<byte>();
            var code = LCSCore.LCSSerialization(source.Code);
            retArr = retArr.Concat(code).ToList();
            return retArr.ToArray();
        }

        public byte[] TransactionArgumentToByte(TransactionArgumentLCS source)
        {
            var argType = U32ToByte(source.ArgType);

            byte[] data;
            switch (source.ArgTypeEnum)
            {
                case TransactionArgumentLCSEnum.Address:

                    data = AddressToByte(source.Address);
                    return argType.Concat(data).ToArray();

                case TransactionArgumentLCSEnum.ByteArray:

                    data = ByteArrToByte(source.ByteArray);
                    return argType.Concat(data).ToArray();

                case TransactionArgumentLCSEnum.String:

                    data = StringToByte(source.String);
                    return argType.Concat(data).ToArray();

                case TransactionArgumentLCSEnum.U64:

                    data = BitConverter.GetBytes(source.U64);
                    return argType.Concat(data).ToArray();
            }

            throw new InvalidOperationException();
        }

        public byte[] AccessPathToByte(AccessPathLCS source)
        {
            byte[] addressData = AddressToByte(source.Address);
            
            return addressData.Concat(ByteArrToByte(source.Path)).ToArray();
        }

        public byte[] ListTransactionArgumentToByte(List<TransactionArgumentLCS> source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Count);
            retArr = retArr.Concat(len).ToList();
            foreach (var item in source)
            {
                var arg = TransactionArgumentToByte(item);
                retArr = retArr.Concat(arg).ToList();
            }
            return retArr.ToArray();
        }

        public byte[] RawTransactionToByte(RawTransactionLCS source)
        {
            List<byte> retArr = new List<byte>();
            var sender = AddressToByte(source.Sender);
            retArr = retArr.Concat(sender).ToList();

            var sn = U64ToByte(source.SequenceNumber);
            retArr = retArr.Concat(sn).ToList();

            var payload = TransactionPayloadToByte(source.TransactionPayload);
            retArr = retArr.Concat(payload).ToList();

            var maxGasAmount = U64ToByte(source.MaxGasAmount);
            retArr = retArr.Concat(maxGasAmount).ToList();

            var gasUnitPrice = U64ToByte(source.GasUnitPrice);
            retArr = retArr.Concat(gasUnitPrice).ToList();

            var expirationTime = U64ToByte(source.ExpirationTime);
            retArr = retArr.Concat(expirationTime).ToList();

            return retArr.ToArray();
        }
    }
}