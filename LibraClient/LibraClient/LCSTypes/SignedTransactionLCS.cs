﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraClient.LCSTypes
{
    public class SignedTransactionLCS
    {
        public byte[] Signature { get; internal set; }
        public byte[] Key { get; internal set; }
        public RawTransactionLCS RawTransaction { get; internal set; }
    }
}
