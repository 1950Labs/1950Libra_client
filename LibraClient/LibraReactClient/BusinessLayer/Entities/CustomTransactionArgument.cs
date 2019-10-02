using LibraReactClient.BusinessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class CustomTransactionArgument
    {
        public TransactionArgumentLCSEnum ArgTypeEnum
        {
            get
            {
                return (TransactionArgumentLCSEnum)ArgType;
            }
        }
        public uint ArgType { get; internal set; }
        public ulong U64 { get; internal set; }
        public string Address { get; internal set; }
        public byte[] ByteArray { get; internal set; }
        public string String { get; internal set; }
    }
}
