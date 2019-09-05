using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class AccountExtendedData : Account
    {
        public decimal Balance { get; internal set; }
        public ulong ReceivedEventsCount { get; internal set; }
        public ulong SentEventsCount { get; internal set; }
        public ulong SequenceNumber { get; internal set; }
        public string AuthenticationKey { get; internal set; }


        public AccountExtendedData(Account account)
        {
            this.AccountId = account.AccountId;
            this.Address = account.Address;
            this.AddressHashed = account.AddressHashed;
            this.Owner = account.Owner;
        }

        public AccountExtendedData()
        {
        }
    }
}
