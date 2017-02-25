using SW.Core.BLL;
using SW.Core.DAL;
using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SW.Core.Helpers;
using SlimWeb.Models;
using SlimWeb.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers
{
    public class ClubController : ApiController
    {
        [HttpGet]
        [AuthorizeFilter]
        public HttpResponseMessage GetClub()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;

                response.Content = new ObjectContent<ClubModel>(ClubModel.MapClub(ClubBLL.GetClub(loggedInUser.AccountSession.ClubId)),
                    new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error getting club..."), ex, 0);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetClubByShortName(string shortName)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new ObjectContent<ClubModel>(ClubModel.MapClub(ClubBLL.GetClubByShortName(shortName)),
                new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Club, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage SaveClub(ClubModel club)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            ClubBLL.SaveClub(ClubModel.MapClubModelToClub(club));
            return response;
        }

        [HttpGet]
        [AuthorizeFilter]
        public HttpResponseMessage GetModuleLinks()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<List<ModuleLinkModel>>(ModuleLinkModel.MapModuleLinks(
                ClubBLL.GetModuleLinks(loggedInUser.AccountSession.ClubId)),
                new JsonMediaTypeFormatter());
            return response;
        }
    }
}
