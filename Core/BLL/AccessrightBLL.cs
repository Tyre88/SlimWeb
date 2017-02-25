using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.DAL;
using SW.Core.Enums;
using SW.Core.Helpers;

namespace SW.Core.BLL
{
    public class AccessrightBLL
    {
        public static bool CheckAccessType(AccessType accessType, int userId)
        {
            return AccessrightDAL.CheckAccessType(accessType, userId);
        }

        public static bool CheckAccessTypeRight(AccessType accessType, AccessTypeRight accessTypeRight, int userId)
        {
            return AccessrightDAL.CheckAccessTypeRight(accessType, accessTypeRight, userId);
        }

        public static List<Accessright> GetAccessrights(int clubId)
        {
            return AccessrightDAL.GetAccessrights(clubId);
        }

        public static Accessright GetAccessright(int id, int clubId)
        {
            return AccessrightDAL.GetAccessright(id, clubId);
        }

        public static void SaveAccessright(Accessright accessright)
        {
            AccessrightDAL.SaveAccessright(accessright);
        }

        public static List<Module> GetModules(int clubId)
        {
            List<Module> modules = new List<Module>();
            try
            {
                modules = AccessrightDAL.GetModules(clubId);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error GetModules"), ex, clubId);
            }
            return modules;
        }
    }
}
