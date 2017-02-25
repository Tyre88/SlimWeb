using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Newsletter.DAL
{
    internal class NewsletterJobDAL
    {
        internal static List<Newsletter_Send> GetNewslettersToSend()
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                return db.Newsletter_Send.Include("Newsletter_Send_Item").Where(n => n.Newsletter_Send_Item.Any(s => !s.IsSent)).ToList();
            }
        }

        internal static void UpdateNewsletterSendItemToSent(int id)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                var item = db.Newsletter_Send_Item.FirstOrDefault(n => n.Id == id);
                item.IsSent = true;
                item.SendDate = DateTime.Now;
                db.SaveChanges();
            }
        }

        internal static void ReadNewsletter(string newsletterSendItemGuid)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                var item = db.Newsletter_Send_Item.FirstOrDefault(n => n.NewsletterSendItemGUID == newsletterSendItemGuid);
                item.IsRead = true;
                item.ReadDate = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}
