using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.Enums;
using SW.Core.DAL;

namespace SW.Core.Interfaces
{
    internal interface IGenericItemPermissionDAL
    {
        bool HasAccessToItem(GenericItemPermissionObjectTypes type, int objectId, Account user);
    }
}
