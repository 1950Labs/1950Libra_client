using System;

namespace LibraReactClient.BusinessLayer.LCSTypes
{
    public class AccountEventLCS
    {
        public string Account { get; internal set; }
        public ulong Amount { get; internal set; }

        public override string ToString()
        {
            return "{ Account = " + Account + "," + Environment.NewLine +
                "Amount = " + Amount
                + "}";
        }
    }
}
