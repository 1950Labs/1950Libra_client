using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class SubmitTransactionIn
    {
        public int SourceAccountId { get; set; }
        public string Recipient { get; set; }
        public double Amount { get; set; }
    }
}
