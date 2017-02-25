using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.DAL
{
    internal static class ContactDAL
    {
        internal static List<Contact> GetContacts(int clubId, bool unsubscribed)
        {
            List<Contact> contacts = new List<Contact>();
            try
            {
                using (CoreModel db = new CoreModel())
                {
                    contacts = db.Contact.Where(c => c.ClubId == clubId && c.IsUnsubscribed == unsubscribed && !c.IsDeleted).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get contacts"), ex, clubId);
            }

            return contacts;
        }

        internal static void SaveContact(Contact contact, int clubId)
        {
            try
            {
                using (CoreModel db = new CoreModel())
                {
                    if (contact.Id <= 0)
                        db.Contact.Add(contact);
                    else
                        db.Entry(contact).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't save contact"), ex, clubId);
            }
        }

        internal static bool IsUnsubscribed(int contactId)
        {
            bool isUnsubscribed = false;
            try
            {
                using (CoreModel db = new CoreModel())
                    isUnsubscribed = db.Contact.FirstOrDefault(c => c.Id == contactId)?.IsUnsubscribed ?? false;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get contact, ContactId: {0}", contactId), ex, 0);
            }

            return isUnsubscribed;
        }

        internal static void Unsubscribe(int? contactId)
        {
            try
            {
                using (CoreModel db = new CoreModel())
                {
                    Contact contact = db.Contact.FirstOrDefault(c => c.Id == (int)contactId);

                    if(contact != null)
                    {
                        contact.IsUnsubscribed = true;
                        contact.UnsubscribeDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't unsubscribe contact, ContactId: {0}", contactId), ex, 0);
            }
        }

        internal static void DeleteContact(Contact contact)
        {
            try
            {
                using (CoreModel db = new CoreModel())
                {
                    contact.IsDeleted = true;
                    db.Entry(contact).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't delete contact"), ex, 0);
            }
        }

        internal static Contact GetContactForEmail(string email, int clubId)
        {
            Contact contact = null;

            try
            {
                using (CoreModel db = new CoreModel())
                    contact = db.Contact.FirstOrDefault(c => c.Email == email && c.ClubId == clubId && !c.IsDeleted);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get contact (GetContactForEmail)"), ex, clubId);
            }

            return contact;
        }

        internal static Contact GetContact(int id)
        {
            Contact contact = null;

            try
            {
                using (CoreModel db = new CoreModel())
                {
                    contact = db.Contact.Where(c => c.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get contact, id: {0}", id), ex, -1);
            }

            return contact;
        }
    }
}
