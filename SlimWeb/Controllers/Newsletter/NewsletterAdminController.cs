using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SW.Core.Helpers;
using SW.Newsletter.BLL;
using SlimWeb.Models.Newsletter;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers.Newsletter
{
    public class NewsletterAdminController : ApiController
    {
        [AuthorizeFilter(AccessType = AccessType.Core, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetNewsletters()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<List<NewsletterModel>>
                (NewsletterModel.MapNewsletterModels(NewsletterAdminBLL.GetNewsletters(loggedInUser.AccountSession.ClubId)), new JsonMediaTypeFormatter());

            return response;
        }

        [AuthorizeFilter(AccessType = AccessType.Core, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetNewsletter(int newsletterId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<NewsletterModel>
                (NewsletterModel.MapNewsletterModel(NewsletterAdminBLL.GetNewsletter(newsletterId, loggedInUser.AccountSession.ClubId)), 
                new JsonMediaTypeFormatter());

            return response;
        }

        [AuthorizeFilter(AccessType = AccessType.Core, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage SaveNewsletter(NewsletterModel newsletter)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            newsletter.CreatedBy = loggedInUser.AccountSession.AccountId;
            newsletter.ClubId = loggedInUser.AccountSession.ClubId;
            NewsletterAdminBLL.SaveNewsletter(NewsletterModel.MapDal(newsletter));
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Core, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage SendNewsletter(SendNewsletterModel sendNewsletterModel)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            NewsletterAdminBLL.SendNewsletter(sendNewsletterModel.NewsletterId, sendNewsletterModel.AccessrightIds, loggedInUser.AccountSession, 
                sendNewsletterModel.ContactIds, sendNewsletterModel.FormFieldId);
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Core, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetNewsletterStatsByNewsletterId(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<List<SW.Newsletter.DAL.GetNewsletterStatsByNewsletterId_Result>>
                (NewsletterAdminBLL.GetNewsletterStatsByNewsletterId(id, loggedInUser.AccountSession.ClubId),
                new JsonMediaTypeFormatter());
            return response;
        }

        [HttpDelete]
        [AuthorizeFilter(AccessType = AccessType.Core, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage DeleteNewsletter(int newsletterId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            var newsletter = NewsletterAdminBLL.GetNewsletter(newsletterId, loggedInUser.AccountSession.ClubId);

            if (newsletter != null && newsletter.ClubId == loggedInUser.AccountSession.ClubId)
                NewsletterAdminBLL.DeleteNewsletter(newsletter);
            else
            {
                LogHelper.LogWarn(string.Format("UserId: {0} trying to delete a newsletter outside of the club, newsletter: {1}",
                    loggedInUser.AccountSession.AccountId, newsletterId), null, loggedInUser.AccountSession.ClubId);
                response.StatusCode = HttpStatusCode.Forbidden;
            }

            return response;
        }
    }
}
