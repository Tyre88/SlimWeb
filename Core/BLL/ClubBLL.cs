using SW.Core.DAL;
using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.BLL
{
    public class ClubBLL
    {
        public static Club GetClub(int clubId)
        {
            return ClubDAL.GetClub(clubId);
        }

        public static void SaveClub(Club club)
        {
            ClubDAL.SaveClub(club);
        }

        public static Club GetClubByShortName(string shortName)
        {
            return ClubDAL.GetClubByShortName(shortName);
        }

        public static List<ModuleLink> GetModuleLinks(int clubId)
        {
            List<ModuleLink> links = new List<ModuleLink>();
            try
            {
                links = ClubDAL.GetModuleLinks(clubId);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error in: GetModuleLinks"), ex, clubId);
            }
            return links;
        }
    }
}
