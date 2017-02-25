using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.DAL
{
    internal class AuthenticateDAL
    {
        internal static Account Login(string userName, string hashedPassword)
        {
            try
            {
                LogHelper.LogInfo(string.Format("Trying to login. userName: {0}, hashedPassword: {1}", userName, hashedPassword), 0);
                using (CoreModel coreDAL = new CoreModel())
                {
                    Account account = coreDAL.Account.Include("Club").Include("AccountAccess")
                        .FirstOrDefault(a => a.UserName == userName && a.Password == hashedPassword);

                    account.AccountAccess.ToList().ForEach(a => a.Accessright.Accessright_Right.ToList());

                    return account;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Failed to login, userName: {0}, hashedPassword: {1}", userName, hashedPassword), ex, 0);
                return null;
            }
        }
    }
}
