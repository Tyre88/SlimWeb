using SW.Core.DAL;
using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.BLL
{
    public static class ContactBLL
    {
        public static List<Contact> GetContacts(int clubId, bool unsubscribed = false)
        {
            return ContactDAL.GetContacts(clubId, unsubscribed);
        }

        public static Contact GetContact(int id)
        {
            return ContactDAL.GetContact(id);
        }

        public static Contact GetContactForEmail(string email, int clubId)
        {
            return ContactDAL.GetContactForEmail(email, clubId);
        }

        public static void Unsubscribe(int? contactId)
        {
            if (contactId != null && contactId > 0)
                ContactDAL.Unsubscribe(contactId);
        }

        public static bool IsUnsubscribed(int? contactId)
        {
            bool isUnsubscribed = false;

            if (contactId != null && contactId > 0)
                isUnsubscribed = ContactDAL.IsUnsubscribed((int)contactId);

            return isUnsubscribed;
        }

        public static void SaveContact(Contact contact, int clubId)
        {
            contact.ClubId = clubId;
            ContactDAL.SaveContact(contact, clubId);
        }

        public static void DeleteContact(int contactId)
        {
            DeleteContact(GetContact(contactId));
        }

        public static void DeleteContact(Contact contact)
        {
            ContactDAL.DeleteContact(contact);
        }

        public static int CsvImport(byte[] data, int clubId)
        {
            int importCount = 0;
            using (StreamReader stream = new StreamReader(new MemoryStream(data), Encoding.Default))
            {
                int counter = 0;
                while(stream.Peek() >= 0)
                {
                    string line = stream.ReadLine();

                    if (counter > 0)
                    {
                        List<string> lineData = line.Split(',').ToList();
                        if(lineData.Count > 58)
                        {
                            var contact = new Contact()
                            {
                                ClubId = clubId,
                                FirstName = lineData[1].Trim('"'),
                                LastName = lineData[3].Trim('"'),
                                Email = lineData[57].Trim('"')
                            };

                            //Check if email is valid, then save the contact... 
                            if (EmailHelper.IsValidEmail(contact.Email))
                            {
                                ContactDAL.SaveContact(contact, clubId);
                                importCount++;
                            }
                        }
                    }
                    counter++;
                }
            }

            return importCount;
        }
    }
}
