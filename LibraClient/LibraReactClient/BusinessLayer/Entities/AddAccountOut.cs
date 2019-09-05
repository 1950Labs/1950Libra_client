using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class AddAccountOut
    {
        public Account Account { get; set; }

        public bool OperationSuccess { get; set; }
    }
}
