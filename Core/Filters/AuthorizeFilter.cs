using SW.Core.BLL;
using SW.Core.Entities;
using SW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SW.Core.Filters
{
    public class AuthorizeFilter : ActionFilterAttribute
    {
        public AccessType AccessType { get; set; }
        public AccessTypeRight AccessTypeRight { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method == HttpMethod.Options)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
                return;
            }

            string token = actionContext.Request.Headers.GetValues("AuthenticateToken").FirstOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            //Get UserId
            AccountSessionEntity session = AccountSessionCache.Instance.GetAccount(token);
            if (session == null)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            UserPrincipal principal = new UserPrincipal(new GenericIdentity(session.Token), new[] { string.Empty })
            {
                AccountSession = session
            };
            HttpContext.Current.User = principal;

            if (!CheckAccessRights(session.AccountId))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }

            base.OnActionExecuting(actionContext);
        }

        private bool CheckAccessRights(int userId)
        {
            if (AccessType > 0)
            {
                return CheckAccessType(userId);
            }
            else
                return true;
        }

        private bool CheckAccessType(int userId)
        {
            if (AccessTypeRight > 0)
            {
                return CheckAccessTypeRight(userId);
            }
            else
            {
                return AccessrightBLL.CheckAccessType(AccessType, userId);
            }
        }

        private bool CheckAccessTypeRight(int userId)
        {
            return AccessrightBLL.CheckAccessTypeRight(AccessType, AccessTypeRight, userId);
        }
    }
}
