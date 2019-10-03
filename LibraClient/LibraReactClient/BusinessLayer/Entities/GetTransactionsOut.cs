using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class GetTransactionsOut
    {
        public List<CustomRawTransaction> Transactions { get; set; }
    }
}
