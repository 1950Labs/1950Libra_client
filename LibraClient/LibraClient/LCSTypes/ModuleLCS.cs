using System;
using System.Collections.Generic;
using System.Text;

namespace LibraClient.LCSTypes
{
    public class ModuleLCS
    {
        public byte[] Code { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("CodeStringLength = {0},{1}", Code.Length, Environment.NewLine);
            retStr += string.Format("CodeString = {0},{1}", Code, Environment.NewLine);
            retStr += "]";
            return retStr;
        }
    }
}
