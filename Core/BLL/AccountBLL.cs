using SW.Core.DAL;
using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.BLL
{
    public class AccountBLL
    {
        public static Account GetUserAccount(int userId)
        {
            return AccountDAL.GetUserAccount(userId);
        }

        public static string GetUserSession(int userId)
        {
            return AccountDAL.GetUserSessionByUserId(userId);
        }

        public static List<Account> GetAllUsers(int clubId)
        {
            return AccountDAL.GetAllUsers(clubId);
        }

        public static Account GetUser(int id)
        {
            return AccountDAL.GetUser(id);
        }

        public static Account SaveAccount(Account acc)
        {
            return AccountDAL.SaveAccount(acc);
        }

        public static Account_Information GetAccountSettings(int id)
        {
            return AccountDAL.GetAccountSettings(id);
        }

        public static List<int> GetAccountIdsToAccessrights(List<int> accessrightIds)
        {
            List<Account> accounts = AccountDAL.GetAccountIdsToAccessrights(accessrightIds);
            List<int> userIds = new List<int>();
            accounts.ForEach(a => userIds.Add(a.ID));
            return userIds.Distinct().ToList();
        }

        public static List<AccountAccess> GetAccountAccesses(int id)
        {
            return AccountDAL.GetAccountAccesses(id);
        }

        public static void DeleteUser(int userId, int clubId)
        {
            try
            {
                Account user = GetUser(userId);
                LogHelper.LogInfo(string.Format("Delete user: Username: {0}, Name: {1} {2}", 
                    user.UserName, user.FirstName, user.LastName), clubId);
                AccountDAL.DeleteAccountGenericValues(userId);
                AccountDAL.DeleteUser(userId, clubId);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't delete user: {0}", userId), ex, clubId);
            }
        }

        public static string GetUserEmail(int userId)
        {
            return AccountDAL.GetUserEmail(userId);
        }

        public static void ImportUsersFromExcel(byte[] data, int clubId, bool sendWelcomeMail, 
            bool tryToMatchGroupName, List<int> accessrightIds)
        {
            MemoryStream ms = new MemoryStream(data);
            DataTable dt = ExcelHelper.GetDataTableFromXls(ms);

            foreach (DataRow row in dt.Rows)
            {
                Account acc = new Account();
                try
                {
                    string password = GenerateRandomPassword(5);
                    acc = new Account()
                    {
                        ClubId = clubId,
                        FirstName = row["Förnamn"].ToString(),
                        Gender = row["Kön"].ToString().ToLower() == "man" ? 0 : 1,
                        LastName = row["Efternamn"].ToString(),
                        UserName = row["E-post 1"].ToString(),
                        Password = password,
                        Image = string.Empty
                    };

                    Account_Information info = new Account_Information()
                    {
                        AccountId = acc.ID,
                        City = row["Ort"].ToString(),
                        Email = row["E-post 1"].ToString(),
                        Phone = row["Mobiltelefon"].ToString(),
                        Street = row["Adress"].ToString(),
                        Zip = row["Postnummer"].ToString(),
                        Grade = 16,
                        Birthday = DateTime.Now,
                        Occupation = "-",
                        Weight = "-",
                        Theme = "blue"
                    };

                    acc.Account_Information.Add(info);

                    if (tryToMatchGroupName)
                        TryToMatchGroupName(acc, row["Gruppkoppling"].ToString(), clubId);
                    else
                    {
                        foreach (int accessrightId in accessrightIds)
                        {
                            DAL.AccountAccess accAccess = new AccountAccess()
                            {
                                AccessID = accessrightId,
                                AccountID = acc.ID
                            };

                            acc.AccountAccess.Add(accAccess);
                        }
                    }

                    SaveAccount(acc);

                    if (sendWelcomeMail)
                        SendWelcomeMail(acc, password);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(string.Format("Error importing user: {0} {1} - username: {2}", 
                        acc.FirstName, acc.LastName, acc.UserName), ex, clubId);
                }
            }
        }

        public static void SaveGenericValues(List<Account_Information_Generic_Value> genericValues, int clubId)
        {
            try
            {
                AccountDAL.SaveGenericValues(genericValues);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format(""), ex, clubId);
            }
        }

        public static bool UserNameExists(string username, int userId)
        {
            return AccountDAL.UserNameExists(username, userId);
        }

        private static void SendWelcomeMail(Account acc, string password)
        {
            //TODO: Implement welcome mail... 
            string email = acc.Account_Information.FirstOrDefault()?.Email;
            if (!string.IsNullOrEmpty(email) && EmailHelper.IsValidEmail(email))
            {
                var club = ClubDAL.GetClub(acc.ClubId);
                string subject = string.Format("Välkommen till gradera system och {0}", club.Name);

                string emailBody = string.Format(@"<div style='text-align: center;'>
                    <h2>Välkommen {0}</h2>
                    <p>Ditt användarnamn är: {1}</p>
                    <p>Ditt lösenord är: {2}</p>
                    <p><a href='http://club.gradera.nu/'>Logga in här</a></p>
                    </div>", 
                    string.Format("{0} {1}", acc.FirstName, acc.LastName),
                    acc.UserName,
                    password);

                EmailHelper.SendEmail(email, emailBody, subject);
            }
        }

        private static void TryToMatchGroupName(Account acc, string accessrightName, int clubId)
        {
            Accessright accessright = AccountDAL.GetAccessrightByName(accessrightName, clubId);
            if(accessright != null)
            {
                DAL.AccountAccess accAccess = new AccountAccess()
                {
                    AccessID = accessright.ID,
                    AccountID = acc.ID
                };

                acc.AccountAccess.Add(accAccess);
            }
            else
            {
                LogHelper.LogInfo(string.Format("Couldn't match group name: {0}", accessrightName), clubId);
            }
        }

        public static List<Account_Information_Generic> GetGenericValues(int id, int clubId)
        {
            List<Account_Information_Generic> values = new List<Account_Information_Generic>();
            try
            {
                values = AccountDAL.GetGenericValues(id, clubId);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get generic values for user: {0}", id), ex, clubId);
            }

            return values;
        }

        public static void ImportUsersFromExcel(byte[] data, int clubId, bool sendWelcomeMail)
        {
            ImportUsersFromExcel(data, clubId, sendWelcomeMail, false, new List<int>());
        }

        private static string GenerateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
