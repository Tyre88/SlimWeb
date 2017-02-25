using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Newsletter.DAL
{
    internal static class NewsletterDAL
    {
        internal static Newsletter_Send_Item GetNewsletterSendItemByGuid(string guid)
        {
            try
            {
                using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
                {
                    return db.Newsletter_Send_Item.FirstOrDefault(n => n.NewsletterSendItemGUID == guid);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error getting newsletter send item, GUID: {0}", guid), ex, 0);
                return null;
            }
        }

        internal static void UnsubscribeNewsletterItem(Newsletter_Send_Item newsletterSendItem)
        {
            try
            {
                using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
                {
                    var item = db.Newsletter_Send_Item.FirstOrDefault(n => n.NewsletterSendItemGUID == newsletterSendItem.NewsletterSendItemGUID);
                    if(item != null)
                    {
                        item.HasUnsubscribed = true;

                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error updating newsletter send item to unsubscribed..."), ex, 0);
            }
        }
    }
}
