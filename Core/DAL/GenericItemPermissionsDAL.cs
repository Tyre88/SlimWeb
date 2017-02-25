using SW.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.Enums;

namespace SW.Core.DAL
{
    internal class GenericItemPermissionsDAL : IGenericItemPermissionDAL
    {
        public bool HasAccessToItem(GenericItemPermissionObjectTypes type, int objectId, Account user)
        {
            using (CoreModel coreDAL = new CoreModel())
            {

                return coreDAL.GenericItemPermission.Any(p => (p.ObjectType == (int)type && p.ObjectId == objectId && p.ClubId == user.ClubId) && (p.AccountId == user.ID || user.AccountAccess.Any(a => a.AccessID == p.AccessrightId)));
            }
        }
    }
}
