using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Newsletter.DAL;
using SW.Core.Entities;

namespace SW.Newsletter.BLL
{
    public class NewsletterAdminBLL
    {
        public static List<Newsletters> GetNewsletters(int clubId)
        {
            return NewsletterAdminDAL.GetNewsletters(clubId);
        }

        public static Newsletters GetNewsletter(int newsletterId, int clubId)
        {
            return NewsletterAdminDAL.GetNewsletter(newsletterId, clubId);
        }

        public static void SaveNewsletter(Newsletters newsletter)
        {
            NewsletterAdminDAL.SaveNewsletter(newsletter);
        }

        public static void SendNewsletter(int newsletterId, List<int> accessrightIds, AccountSessionEntity accountSession,
            List<int> contactIds = null, int formFieldId = -1)
        {
            List<int> userIds = Core.BLL.AccountBLL.GetAccountIdsToAccessrights(accessrightIds);
            Newsletters newsletter = NewsletterAdminDAL.GetNewsletter(newsletterId, accountSession.ClubId);
            int newsletterSendId = NewsletterAdminDAL.CreateNewsletterSend(newsletter, accountSession);
            NewsletterAdminDAL.CreateNewsletterSendItems(newsletterSendId, userIds, contactIds, formFieldId);
        }

        public static List<GetNewsletterStatsByNewsletterId_Result> GetNewsletterStatsByNewsletterId(int newsletterId, int clubId)
        {
            return NewsletterAdminDAL.GetNewsletterStatsByNewsletterId(newsletterId, clubId);
        }

        public static void DeleteNewsletter(int newsletterId, int clubId)
        {
            DeleteNewsletter(GetNewsletter(newsletterId, clubId));
        }

        public static void DeleteNewsletter(Newsletters newsletter)
        {
            NewsletterAdminDAL.DeleteNewsletter(newsletter);
        }
    }
}
