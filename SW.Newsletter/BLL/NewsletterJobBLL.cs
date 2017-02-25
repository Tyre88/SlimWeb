using SW.Core.BLL;
using SW.Core.Helpers;
using SW.Newsletter.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SW.Newsletter.BLL
{
    public class NewsletterJobBLL
    {
        public static void SendNewsletters()
        {
            List<Newsletter_Send> newslettersToSend = NewsletterJobDAL.GetNewslettersToSend();

            foreach (var newsletter in newslettersToSend)
            {
                foreach (var newsletterSendItem in newsletter.Newsletter_Send_Item)
                {
                    if(!newsletterSendItem.IsSent)
                    {
                        if (newsletterSendItem.SendToContactId != null && newsletterSendItem.SendToContactId > 0 
                            && ContactBLL.IsUnsubscribed(newsletterSendItem.SendToContactId))
                            continue;

                        string mailContent = string.Concat(newsletter.NewsletterContent, 
                            string.Format(@"<img style='height: 0px; width: 0px; display: none;' src='http://gradera-klubb.local/api/NewsletterJob/ReadNewsletter/{0}' />
                            <center><p><a href='{1}unsubscribe/{0}'>Avbryt prenumerationen på detta nyhetsbrev</a></p></center>", 
                            newsletterSendItem.NewsletterSendItemGUID,
                            AppSettingsHelper.GetAppSetting("ExternalPath") ?? "//club.gradera.nu/external.html#/"));
                        EmailHelper.SendEmail(newsletterSendItem.Email,
                            mailContent, 
                            newsletter.NewsletterName);
                        NewsletterJobDAL.UpdateNewsletterSendItemToSent(newsletterSendItem.Id);
                        Thread.Sleep(10000);
                    }
                }
            }
        }

        public static void ReadNewsletter(string id)
        {
            NewsletterJobDAL.ReadNewsletter(id);
        }
    }
}
