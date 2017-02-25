using SW.Core.BLL;
using SW.Newsletter.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Newsletter.BLL
{
    public static class NewsletterBLL
    {
        public static string Unsubscribe(string guid)
        {
            string email = string.Empty;
            Newsletter_Send_Item newsletterSendItem = NewsletterDAL.GetNewsletterSendItemByGuid(guid);
            if(newsletterSendItem != null && newsletterSendItem.SendToContactId != null && newsletterSendItem.SendToContactId > 0)
            {
                NewsletterDAL.UnsubscribeNewsletterItem(newsletterSendItem);
                ContactBLL.Unsubscribe(newsletterSendItem.SendToContactId);
                email = newsletterSendItem.Email;
            }
            return email;
        }
    }
}
