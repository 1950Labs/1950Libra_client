using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Types;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class SubmitTransactionOut
    {
        public RawTransaction Transaction { get; set; }

        public string ACStatus { get; set; }

        public string SourceHex { get; set; }

        public string RecipientHex { get; set; }

        public double Amount { get; set; }

    }
}
