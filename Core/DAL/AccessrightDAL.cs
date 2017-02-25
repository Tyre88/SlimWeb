using System;
using System.Collections.Generic;
using System.Linq;
using SW.Core.Enums;

namespace SW.Core.DAL
{
    internal class AccessrightDAL
    {
        internal static bool CheckAccessType(AccessType accessType, int userId)
        {
            using(CoreModel coreDAL = new CoreModel())
                return coreDAL.Account.FirstOrDefault(a => a.ID == userId)
                    .AccountAccess.Any(a => a.Accessright.Accessright_Right.Any(r => r.AccessType == (int)accessType));
        }

        internal static bool CheckAccessTypeRight(AccessType accessType, AccessTypeRight accessTypeRight, int userId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Account.FirstOrDefault(a => a.ID == userId)
                .AccountAccess.Any(a => a.Accessright.Accessright_Right.Any(r => r.AccessType == (int)accessType 
                && r.AccessTypeRight >= (int)accessTypeRight));
        }

        internal static Accessright GetAccessright(int id, int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Accessright.Include("Accessright_Right").FirstOrDefault(a => a.ClubId == clubId && a.ID == id);
        }

        internal static void SaveAccessright(Accessright accessright)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                if (accessright.ID > 0)
                {
                    var ar = coreDAL.Accessright.Include("Accessright_Right").FirstOrDefault(a => a.ID == accessright.ID);

                    ar.Description = accessright.Description;
                    ar.Name = accessright.Name;

                    ar.Accessright_Right.ToList().ForEach(a => coreDAL.Entry(a).State = System.Data.Entity.EntityState.Deleted);
                    accessright.Accessright_Right.ToList().ForEach(a => ar.Accessright_Right.Add(a));
                }
                else
                {
                    coreDAL.Accessright.Add(accessright);
                }

                coreDAL.SaveChanges();
            }
        }

        internal static List<Module> GetModules(int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Module.Where(m => m.ClubLinkModule.Any(c => c.ClubId == clubId)).ToList();
        }

        internal static List<Accessright> GetAccessrights(int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Accessright.Where(a => a.ClubId == clubId).ToList();
        }
    }
}
