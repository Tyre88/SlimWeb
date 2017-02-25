using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.Entities
{
    public class UserPrincipal : GenericPrincipal
    {
        public AccountSessionEntity AccountSession { get; set; }

        public UserPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
            AccountSession = new AccountSessionEntity();
        }
    }
}
