using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Core.Entities;
using SW.Core.Helpers;

namespace SW.Core.DAL
{
    internal class AccountDAL
    {
        internal static List<Account> GetAllAccounts()
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Account.ToList();
        }

        internal static Account GetUserAccount(int userId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Account.Include("Club").Include("Account_Information").FirstOrDefault(a => a.ID == userId);
        }

        internal static void AddToLoginLog(Account acc)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                LoginLog log = new LoginLog()
                {
                    ClubId = acc.ClubId,
                    UserId = acc.ID,
                    Date = DateTime.Now
                };

                coreDAL.LoginLog.Add(log);
                coreDAL.SaveChanges();
            }
        }

        internal static List<Account> GetAllUsers(int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Account.Include("Club").Where(a => a.ClubId == clubId).ToList();
        }

        internal static List<Account> GetAccountIdsToAccessrights(List<int> accessrightIds)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                return coreDAL.Account.Include("AccountAccess").Where(a => 
                a.AccountAccess.Any(r => accessrightIds.Contains(r.AccessID))).Distinct().ToList();
            }
        }

        internal static void DeleteAccountGenericValues(int userId)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                var items = coreDAL.Account_Information_Generic_Value.Where(a => a.AccountId == userId).ToList();
                foreach (var item in items)
                {
                    coreDAL.Account_Information_Generic_Value.Remove(item);
                }

                coreDAL.SaveChanges();
            }
        }

        internal static string GetUserEmail(int userId)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                return coreDAL.Account_Information.FirstOrDefault(a => a.AccountId == userId).Email;
            }
        }

        internal static Account GetUser(int id)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                Account account = new Account();
                try
                {
                    account = coreDAL.Account.Include("Club").Include("AccountAccess").Include("Account_Information").FirstOrDefault(a => a.ID == id);
                    account.AccountAccess.ToList().ForEach(a => a.Accessright.Accessright_Right.ToList());
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(string.Format("Couldn't get user: {0}", id), ex, 0);
                }
                return account;
            }
        }

        internal static void DeleteUser(int userId, int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                Account acc = coreDAL.Account.FirstOrDefault(a => a.ID == userId && a.ClubId == clubId);
                coreDAL.Entry(acc).State = System.Data.Entity.EntityState.Deleted;
                coreDAL.SaveChanges();
            }
        }

        internal static void AddSession(AccountSessionEntity session)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                Account_Session sess = new Account_Session()
                {
                    AccountId = session.AccountId,
                    Token = session.Token,
                    ExpirationDate = session.ExpirationDate,
                    LoginDate = DateTime.Now
                };

                coreDAL.Account_Session.Add(sess);
                coreDAL.SaveChanges();
            }
        }

        internal static void Logout(string token)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                Account_Session session = coreDAL.Account_Session.FirstOrDefault(a => a.Token == token);
                coreDAL.Entry(session).State = System.Data.Entity.EntityState.Deleted;
                coreDAL.SaveChanges();
            }
        }

        internal static Accessright GetAccessrightByName(string accessrightName, int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Accessright.FirstOrDefault(a => a.ClubId == clubId && a.Name == accessrightName);
        }

        internal static List<AccountAccess> GetAccountAccesses(int id)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.AccountAccess.Where(a => a.AccountID == id).ToList();
        }

        internal static Account_Information GetAccountSettings(int id)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Account_Information.FirstOrDefault(a => a.AccountId == id);
        }

        internal static void SaveGenericValues(List<Account_Information_Generic_Value> genericValues)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                foreach (var item in genericValues)
                {
                    if (item.Id > 0)
                        coreDAL.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    else
                        coreDAL.Account_Information_Generic_Value.Add(item);
                }
                coreDAL.SaveChanges();
            }
        }

        internal static string GetUserSessionByUserId(int userId)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                Account_Session session = coreDAL.Account_Session.FirstOrDefault(s => s.AccountId == userId);
                if (session != null)
                    return session.Token;

                return string.Empty;
            }
        }

        internal static bool UserNameExists(string username, int userId)
        {
            using (CoreModel coreDAL = new CoreModel())
                return coreDAL.Account.Any(a => a.UserName == username && a.ID != userId);
        }

        internal static AccountSessionEntity GetUserSession(string token)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                AccountSessionEntity account = null;
                Account_Session session = coreDAL.Account_Session.Include("Account").FirstOrDefault(s => s.Token == token);
                if (session != null)
                {
                    account = new AccountSessionEntity()
                    {
                        AccountId = session.AccountId,
                        Token = session.Token,
                        LoginDate = session.LoginDate,
                        ExpirationDate = session.ExpirationDate,
                        ClubId = session.Account.ClubId
                    };
                }
                return account;
            }
        }

        internal static List<Account_Information_Generic> GetGenericValues(int id, int clubId)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                var values = 
                    coreDAL.Account_Information_Generic.Join(
                        coreDAL.Account_Information_Generic_Value,
                        i => i.Id,
                        iv => iv.AccountInformationGenericId,
                        (i, iv) => new
                        {
                            Account_Information_Generic = i,
                            Account_Information_Generic_Value = i.Account_Information_Generic_Value.FirstOrDefault(g => g.AccountId == id)
                        })
                        .Select(i => new
                        {
                            Id = i.Account_Information_Generic.Id,
                            Name = i.Account_Information_Generic.Name,
                            Type = i.Account_Information_Generic.Type,
                            AccountInformationGenericId = i.Account_Information_Generic.Id,
                            Account_Information_Generic_Value = i.Account_Information_Generic_Value
                        }).DistinctBy(d => d.Id).ToList();

                List<Account_Information_Generic> valuesToReturn = new List<Account_Information_Generic>();

                foreach (var item in values)
                {
                    List<Account_Information_Generic_Value> gvalues = new List<Account_Information_Generic_Value>();
                    gvalues.Add(item.Account_Information_Generic_Value);
                    valuesToReturn.Add(new Account_Information_Generic()
                    {
                        Account_Information_Generic_Value = gvalues,
                        Id = item.Id,
                        Name = item.Name,
                        Type = item.Type
                    });
                }

                return valuesToReturn;
            }
        }

        internal static List<AccountSessionEntity> GetAllUserSessions()
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                List<AccountSessionEntity> sessions = new List<AccountSessionEntity>();

                coreDAL.Account_Session.Include("Account").Where(s => s.ExpirationDate >= DateTime.Now).ToList()
                    .ForEach(a => sessions.Add(new AccountSessionEntity()
                {
                    AccountId = a.AccountId,
                    Token = a.Token,
                    ExpirationDate = a.ExpirationDate,
                    LoginDate = a.LoginDate,
                    ClubId = a.Account.ClubId
                }));

                return sessions;
            }
        }

        internal static Account SaveAccount(Account acc)
        {
            using (CoreModel coreDAL = new CoreModel())
            {
                if (acc.ID > 0)
                {
                    Account account = coreDAL.Account.Include("Club").Include("AccountAccess").Include("Account_Information")
                        .Where(a => a.ID == acc.ID).FirstOrDefault();
                    account.AccountAccess.ToList().ForEach(a => a.Accessright.Accessright_Right.ToList());
                    if (account != null)
                    {
                        account.FirstName = acc.FirstName;
                        account.LastName = acc.LastName;
                        account.UserName = acc.UserName;
                        account.Image = acc.Image;
                        account.Gender = acc.Gender;

                        account.Account_Information.ToList().ForEach(a => coreDAL.Entry(a).State = System.Data.Entity.EntityState.Deleted);
                        acc.Account_Information.ToList().ForEach(a => account.Account_Information.Add(a));

                        account.AccountAccess.ToList().ForEach(a => coreDAL.Entry(a).State = System.Data.Entity.EntityState.Deleted);
                        acc.AccountAccess.ToList().ForEach(a => account.AccountAccess.Add(a));

                        if (!string.IsNullOrEmpty(acc.Password))
                        {
                            account.Password = Sha256Helper.GetHashSha256(acc.Password);
                        }
                        coreDAL.SaveChanges();

                        return account;
                    }
                    else
                        return null;
                }
                else
                {
                    acc.Password = Sha256Helper.GetHashSha256(acc.Password);
                    coreDAL.Account.Add(acc);
                    coreDAL.SaveChanges();
                    return acc;
                }
            }
        }
    }
}
