using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Enums
{
    public enum TransactionPayloadEnum
    {
        Program = 0,
        WriteSet = 1,
        Script = 2,
        Module = 3
    }
}
