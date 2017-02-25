using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.Entities;
using SW.Core.Helpers;
using SW.Forms.BLL;

namespace SW.Newsletter.DAL
{
    internal class NewsletterAdminDAL
    {
        internal static List<Newsletters> GetNewsletters(int clubId)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                return db.Newsletters.Include("Account").Where(n => n.ClubId == clubId && !n.IsDeleted).ToList();
            }
        }

        internal static Newsletters GetNewsletter(int newsletterId, int clubId)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                return db.Newsletters.Include("Account").FirstOrDefault(n => n.Id == newsletterId && n.ClubId == clubId && !n.IsDeleted);
            }
        }

        internal static void SaveNewsletter(Newsletters newsletter)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                if (newsletter.Id > 0)
                    db.Entry(newsletter).State = System.Data.Entity.EntityState.Modified;
                else
                {
                    newsletter.CreatedDate = DateTime.Now;
                    db.Newsletters.Add(newsletter);
                }

                db.SaveChanges();
            }
        }

        internal static void CreateNewsletterSendItems(int newsletterSendId, List<int> userIds, List<int> contactIds = null, int formFieldId = -1)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                foreach (int userId in userIds)
                {
                    Newsletter_Send_Item item = new Newsletter_Send_Item()
                    {
                        IsRead = false,
                        IsSent = false,
                        Email = SW.Core.BLL.AccountBLL.GetUserEmail(userId),
                        NewsletterSendId = newsletterSendId,
                        NewsletterSendItemGUID = Guid.NewGuid().ToString(),
                        SendDate = DateTime.Now,
                        SendToUserId = userId,
                        ReadDate = DateTime.Now.AddYears(-10),
                        HasUnsubscribed = false,
                        SendType = "USER"
                    };

                    db.Newsletter_Send_Item.Add(item);
                }

                if(contactIds != null && contactIds.Count > 0)
                {
                    foreach (int contactId in contactIds)
                    {
                        Newsletter_Send_Item item = new Newsletter_Send_Item()
                        {
                            IsRead = false,
                            IsSent = false,
                            Email = SW.Core.BLL.ContactBLL.GetContact(contactId).Email,
                            NewsletterSendId = newsletterSendId,
                            NewsletterSendItemGUID = Guid.NewGuid().ToString(),
                            SendDate = DateTime.Now,
                            SendToContactId = contactId,
                            ReadDate = DateTime.Now.AddYears(-10),
                            HasUnsubscribed = false,
                            SendType = "CONTACT"
                        };

                        db.Newsletter_Send_Item.Add(item);
                    }
                }

                if(formFieldId > 0)
                {
                    List<string> emails = FormsAdminBLL.GetExternalSubmitValues(formFieldId);

                    foreach (var email in emails)
                    {
                        Newsletter_Send_Item item = new Newsletter_Send_Item()
                        {
                            IsRead = false,
                            IsSent = false,
                            Email = email,
                            NewsletterSendId = newsletterSendId,
                            NewsletterSendItemGUID = Guid.NewGuid().ToString(),
                            SendDate = DateTime.Now,
                            ReadDate = DateTime.Now.AddYears(-10),
                            HasUnsubscribed = false,
                            SendType = "FORM"
                        };

                        db.Newsletter_Send_Item.Add(item);
                    }
                }
                
                db.SaveChanges();
            }
        }

        internal static void DeleteNewsletter(Newsletters newsletter)
        {
            try
            {
                using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
                {
                    newsletter.IsDeleted = true;
                    db.Entry(newsletter).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error delete newsletter: {0}", newsletter.Id), ex, 0);
            }
        }

        internal static List<GetNewsletterStatsByNewsletterId_Result> GetNewsletterStatsByNewsletterId(int newsletterId, int clubId)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                return db.GetNewsletterStatsByNewsletterId(newsletterId, clubId).ToList();
            }
        }

        internal static int CreateNewsletterSend(Newsletters newsletter, AccountSessionEntity accountSession)
        {
            using (Vicdude_NewsletterEntities db = new Vicdude_NewsletterEntities())
            {
                Newsletter_Send newsletterSend = new Newsletter_Send()
                {
                    ClubId = accountSession.ClubId,
                    UserSendId = accountSession.AccountId,
                    NewsletterContent = newsletter.Content,
                    NewsletterId = newsletter.Id,
                    NewsletterName = newsletter.Name,
                    NewsletterSendGUID = Guid.NewGuid().ToString(),
                    SendDate = DateTime.Now
                };

                db.Newsletter_Send.Add(newsletterSend);
                db.SaveChanges();
                return newsletterSend.Id;
            }
        }
    }
}
