using LibraClient.Enums;
using LibraClient.LCSTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraClient
{
    public class LibraCanonicalDeserialization
    {
        public byte[] U64ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] AddressToByte(AddressLCS source)
        {
            var len = U32ToByte((uint)source.Length);
            var data = source.Value.HexStringToByteArray();
            return len.Concat(data).ToArray();
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
            //var len = U32ToByte((uint)source);

            return retArr.ToArray();
        }

        public byte[] ProgramToByte(ProgramLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ScriptToByte(ScriptLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ModuleToByte(ModuleLCS source)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}