using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraClient.LCSTypes
{
    public class WriteSetLCS
    {
        public Dictionary<AccessPathLCS, WriteOpLCS> WriteSet { get; set; }
        public uint Length { get; internal set; }

        public override string ToString()
        {
            string retVal = "[" + Environment.NewLine;
            foreach (var item in WriteSet)
            {
                retVal += "(" + item.Key + "," + Environment.NewLine + item.Value + ")"
                    + Environment.NewLine;
            }
            retVal += "]";

            return retVal;
        }
    }
}
