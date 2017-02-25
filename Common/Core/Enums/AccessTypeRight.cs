using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Enums
{
    public enum AccessTypeRight
    {
        UnAvailable = 0,
        Read = 10,
        Write = 20,
        Publish = 30,
        Admin = 40
    }
}
