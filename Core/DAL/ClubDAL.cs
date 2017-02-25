using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.DAL
{
    internal class ClubDAL
    {
        internal static Club GetClub(int clubId)
        {
            try
            {
                using (CoreModel coreDAL = new CoreModel())
                {
                    return coreDAL.Club.FirstOrDefault(c => c.Id == clubId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error getting club: {0}", clubId), ex, clubId);
                return null;
            }
        }

        internal static void SaveClub(Club club)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                coreDAL.Entry(club).State = System.Data.Entity.EntityState.Modified;
                coreDAL.SaveChanges();
            }
        }

        internal static Club GetClubByShortName(string shortName)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                return coreDAL.Club.FirstOrDefault(c => c.ShortName == shortName);
            }
        }

        internal static List<ModuleLink> GetModuleLinks(int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                return coreDAL.ModuleLink.Include("Module")
                    .Where(m => m.Module.ClubLinkModule.Any(c => c.ClubId == clubId) 
                        && (m.SpecificClubId == null || m.SpecificClubId == clubId)).ToList();
            }
        }
    }
}
