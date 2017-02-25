using SW.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.Enums;
using SW.Core.DAL;

namespace SW.Core.BLL
{
    public class GenericItemPermissionsBLL : IGenericItemPermissionsBLL
    {
        IGenericItemPermissionDAL _dal;

        public GenericItemPermissionsBLL()
        {
            _dal = new GenericItemPermissionsDAL();
        }

        public IList<int> GetObjectIdsOfType(GenericItemPermissionObjectTypes type, Account user)
        {
            throw new NotImplementedException();
        }

        public bool HasAccessToItem(GenericItemPermissionObjectTypes type, int objectId, Account user)
        {
            return _dal.HasAccessToItem(type, objectId, user);
        }
    }
}
