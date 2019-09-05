using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class GetAccountInfoIn
    {
        public int AccountId { get; set; }
        public string Address { get; set; }
    }
}
