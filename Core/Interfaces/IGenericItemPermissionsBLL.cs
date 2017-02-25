using SW.Core.DAL;
using SW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.Interfaces
{
    public interface IGenericItemPermissionsBLL
    {
        bool HasAccessToItem(GenericItemPermissionObjectTypes type, int objectId, Account user);
        IList<int> GetObjectIdsOfType(GenericItemPermissionObjectTypes type, Account user);
    }
}
