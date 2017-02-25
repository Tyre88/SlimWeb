using SW.Core.DAL;
using SW.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.BLL
{
    public sealed class AccountSessionCache
    {
        private static AccountSessionCache _instance;
        public List<AccountSessionEntity> Accounts { get; set; }

        public static AccountSessionCache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AccountSessionCache();

                return _instance;
            }
        }

        private AccountSessionCache()
        {
            Accounts = new List<AccountSessionEntity>();
        }

        public AccountSessionEntity GetAccount(string token)
        {
            if (Accounts == null)
                Accounts = new List<AccountSessionEntity>();

            AccountSessionEntity account = Accounts.FirstOrDefault(a => a.Token == token);

            if(account == null)
            {
                account = AccountDAL.GetUserSession(token);

                if(account != null)
                    Accounts.Add(account);
            }

            return account;
        }

        public void Add(AccountSessionEntity session)
        {
            AccountDAL.AddSession(session);
            Accounts.Add(session);
        }

        public void Logout(string token)
        {
            AccountSessionEntity session = GetAccount(token);
            AccountDAL.Logout(token);
            Instance.Accounts.Remove(session);
        }

        public void MapAllSessions()
        {
            Accounts.AddRange(DAL.AccountDAL.GetAllUserSessions());
        }
    }
}
