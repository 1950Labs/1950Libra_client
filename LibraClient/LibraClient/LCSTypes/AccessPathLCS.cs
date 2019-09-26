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

        //    AccessPath {
        //  address: c4c63f80c74b11263e421ebf8486a4e398d0dbc09fa7d4f62ccdb309f3aea81f,
        //  path: 01217da6c6b3e19f18
        //},

        public override string ToString()
        {
            string retVal = "AccessPath {" + Environment.NewLine;
            retVal += "address = " + Address + "," + Environment.NewLine;
            //retVal += "path = " + Path.ByteArryToString() + Environment.NewLine;
            retVal += "}";
            return retVal;
        }
    }
}
