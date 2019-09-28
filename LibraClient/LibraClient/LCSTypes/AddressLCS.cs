using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraClient.LCSTypes
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
