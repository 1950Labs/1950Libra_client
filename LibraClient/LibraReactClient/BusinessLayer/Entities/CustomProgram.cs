using LibraReactClient.BusinessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class CustomProgram
    {
        public TransactionPayloadEnum PayloadType { get; set; }
        public IEnumerable<byte[]> Modules { get; set; }
        public byte[] Code { get; set; }
        public IEnumerable<CustomTransactionArgument> Arguments { get; set; }
    }
}
