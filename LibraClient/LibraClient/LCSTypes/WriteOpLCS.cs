using LibraClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraClient.LCSTypes
{
    public class WriteOpLCS
    {
        public uint WriteOpType { get; internal set; }
        public WriteOpLCSEnum WriteOpTypeEnum
        {
            get
            {
                return (WriteOpLCSEnum)WriteOpType;
            }
        }

        public byte[] Value { get; internal set; }
        public override string ToString()
        {
            if (WriteOpTypeEnum == WriteOpLCSEnum.Value)
            {
                return Value.ByteArryToString();
            }
            else
                return "Deletion";
        }
    }
}
