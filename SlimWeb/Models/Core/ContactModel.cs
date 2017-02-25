using SW.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Core
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUnsubscribed { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
        public int ClubId { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public ContactModel() { }
        public ContactModel(Contact contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Email = contact.Email;
            Phone = contact.Phone;
            IsDeleted = contact.IsDeleted;
            IsUnsubscribed = contact.IsUnsubscribed;
            UnsubscribeDate = contact.UnsubscribeDate;
            ClubId = contact.ClubId;
        }

        public static List<ContactModel> MapModels(List<Contact> contacts)
        {
            List<ContactModel> models = new List<ContactModel>();
            foreach (var item in contacts)
            {
                models.Add(new ContactModel(item));
            }
            return models;
        }

        internal static Contact GetDalModel(ContactModel contact)
        {
            return new Contact()
            {
                Id = contact.Id,
                ClubId = contact.ClubId,
                Email = contact.Email,
                FirstName = contact.FirstName,
                IsDeleted = contact.IsDeleted,
                IsUnsubscribed = contact.IsUnsubscribed,
                LastName = contact.LastName,
                Phone = contact.Phone,
                UnsubscribeDate = contact.UnsubscribeDate
            };
        }
    }
}