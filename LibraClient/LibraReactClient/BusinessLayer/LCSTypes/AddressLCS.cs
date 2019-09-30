
using LibraReactClient.BusinessLayer.Common;

namespace LibraReactClient.BusinessLayer.LCSTypes
{
    public struct AddressLCS
    {
        public byte[] ValueByte { get; set; }
        public string Value { get; set; }
        public uint Length { get; set; }

        public AddressLCS(string address)
        {
            this.Value = address;
            this.ValueByte = address.HexStringToByteArray();
            this.Length = (uint)this.ValueByte.Length;
        }


        public override string ToString()
        {
            return Value;
        }
    }
}
