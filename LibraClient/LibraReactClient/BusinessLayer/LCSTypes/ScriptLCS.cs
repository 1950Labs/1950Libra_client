﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraReactClient.BusinessLayer.LCSTypes
{ 
    public class ScriptLCS
    {
        public byte[] Code { get; internal set; }
        public IEnumerable<TransactionArgumentLCS> TransactionArguments { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("CodeStringLength = {0},{1}", Code.Length,
                Environment.NewLine);
            retStr += string.Format("CodeString = {0},{1}", Code,
                Environment.NewLine);
            retStr += "Arguments = [";
            foreach (var item in TransactionArguments)
            {
                retStr += item;
                if (item != TransactionArguments.Last())
                    retStr += string.Format(",{0}", Environment.NewLine);
            }
            retStr += string.Format("],{0}", Environment.NewLine);
            retStr += "]";
            return retStr;
        }
    }
}
