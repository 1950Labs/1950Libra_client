using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraClient.LCSTypes
{
    public class AccessPathLCS
    {
        public AddressLCS Address { get; internal set; }
        public byte[] Path { get; internal set; }

        public override string ToString()
        {
            string retVal = "AccessPath {" + Environment.NewLine;
            retVal += "address: " + Address + "," + Environment.NewLine;
            retVal += "path: " + Path.ByteArryToString() + Environment.NewLine;
            retVal += "}";
            return retVal;
        }
    }
}
