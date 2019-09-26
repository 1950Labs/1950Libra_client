using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraClient.LCSTypes
{
    public class AccountResourceLCS
    {
        public AddressLCS AuthenticationKey { get; internal set; }
        public ulong Balance { get; internal set; }
        public bool DelegatedWithdrawalCapability { get; internal set; }
        public byte[] ReceivedEvents { get; internal set; }
        public byte[] SentEvents { get; internal set; }
        public ulong SequenceNumber { get; internal set; }

        public override string ToString()
        {
            string retVal = "{";
            retVal += "AuthenticationKey = " + AuthenticationKey +
                Environment.NewLine;
            retVal += "Balance = " + Balance + Environment.NewLine;
            retVal += "DelegatedWithdrawalCapability = " + DelegatedWithdrawalCapability +
           Environment.NewLine;
            //retVal += "ReceivedEvents = " + ReceivedEvents.ByteArryToString() +
            //Environment.NewLine;
            //retVal += "SentEvents = " + SentEvents.ByteArryToString() +
         // Environment.NewLine;
            retVal += "SequenceNumber = " + SequenceNumber + Environment.NewLine;
            retVal += "}";
            return retVal;
        }
    }
}
