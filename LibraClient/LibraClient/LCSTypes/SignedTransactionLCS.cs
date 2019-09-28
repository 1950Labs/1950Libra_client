using System;
using System.Collections.Generic;
using System.Text;

namespace LibraClient.LCSTypes
{
    public class SignedTransactionLCS
    {
        public byte[] Signature { get; internal set; }
    }
}
