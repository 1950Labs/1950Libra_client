using LibraReactClient.BusinessLayer.Common;
using LibraReactClient.BusinessLayer.Enums;

namespace LibraReactClient.BusinessLayer.LCSTypes
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
