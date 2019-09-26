using LibraClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraClient.LCSTypes
{
    public class TransactionArgumentLCS
    {
        public uint ArgType { get; internal set; }
        public TransactionArgumentLCSEnum ArgTypeEnum
        {
            get
            {
                return (TransactionArgumentLCSEnum)ArgType;
            }
        }

        public ulong U64 { get; internal set; }
        public AddressLCS Address { get; internal set; }
        public byte[] ByteArray { get; internal set; }
        public string String { get; internal set; }

        public override string ToString()
        {
            if (ArgTypeEnum == TransactionArgumentLCSEnum.Address)
                return "{" + $"{ArgTypeEnum}: {Address}" + "}";
            else if (ArgTypeEnum == TransactionArgumentLCSEnum.ByteArray)
                return "{" + $"{ArgTypeEnum}: {ByteArray.ByteArryToString()}" + "}";
            else if (ArgTypeEnum == TransactionArgumentLCSEnum.String)
                return "{" + $"{ArgTypeEnum}: {String}" + "}";
            else if (ArgTypeEnum == TransactionArgumentLCSEnum.U64)
                return "{"+ $"{ArgTypeEnum}: {U64}" + "}";


            return "TransactionArgumentLCS is null";
        }
    }
}
