using SW.Core.BLL;
using SW.Core.DAL;
using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SW.Core.Helpers;
using SlimWeb.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers.Core
{
    public class ContactController : ApiController
    {
        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Contact, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetAllContacts()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            List<Contact> contacts = ContactBLL.GetContacts(loggedInUser.AccountSession.ClubId);
            List<ContactModel> models = ContactModel.MapModels(contacts);
            response.Content = new ObjectContent<List<ContactModel>>(models, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Contact, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetContact(int contactId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            Contact contact = ContactBLL.GetContact(contactId);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            if (loggedInUser.AccountSession.ClubId == contact.ClubId)
                response.Content = new ObjectContent<ContactModel>(new ContactModel(contact), new JsonMediaTypeFormatter());
            else
            {
                LogHelper.LogWarn(string.Format("UserId: {0} trying to get a contact outside of the club, contact: {1}",
                    loggedInUser.AccountSession.AccountId, contactId), null, loggedInUser.AccountSession.ClubId);
                response.StatusCode = HttpStatusCode.Forbidden;
            }
                
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Contact, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetContactForEmail(string email, int clubId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            Contact contact = ContactBLL.GetContactForEmail(email, clubId);
            response.Content = new ObjectContent<ContactModel>(new ContactModel(contact), new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost]
        [AuthorizeFilter(AccessType = AccessType.Contact, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage SaveContact(Contact contact)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            ContactBLL.SaveContact(contact, loggedInUser.AccountSession.ClubId);
            return response;
        }

        [HttpDelete]
        [AuthorizeFilter(AccessType = AccessType.Contact, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage DeleteContact(int contactId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            Contact contact = ContactBLL.GetContact(contactId);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            if (loggedInUser.AccountSession.ClubId == contact.ClubId)
                ContactBLL.DeleteContact(contact);
            else
            {
                LogHelper.LogWarn(string.Format("UserId: {0} trying to delete a contact outside of the club, contact: {1}",
                    loggedInUser.AccountSession.AccountId, contactId), null, loggedInUser.AccountSession.ClubId);
                response.StatusCode = HttpStatusCode.Forbidden;
            }

            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Contact, AccessTypeRight = AccessTypeRight.Admin)]
        public async Task<HttpResponseMessage> CsvImport()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            if (!Request.Content.IsMimeMultipartContent())
            {
                response.StatusCode = HttpStatusCode.UnsupportedMediaType;
            }
            else
            {
                UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Task<byte[]> fileData = provider.Contents.First().ReadAsByteArrayAsync();
                int importCount = ContactBLL.CsvImport(fileData.Result, loggedInUser.AccountSession.ClubId);
                response.Content = new ObjectContent<int>(importCount, new JsonMediaTypeFormatter());
            }

            return response;
        }
    }
}
