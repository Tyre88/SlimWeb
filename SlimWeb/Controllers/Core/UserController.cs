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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers
{
    public class UserController : ApiController
    {
        [AuthorizeFilter(AccessType = AccessType.Account, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetAllUsers()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            List<Account> accounts = AccountBLL.GetAllUsers(loggedInUser.AccountSession.ClubId);
            List<UserModel> users = UserModel.MapUserModels(accounts, false, true);

            response.Content = new ObjectContent<List<UserModel>>(users, new JsonMediaTypeFormatter());

            return response;
        }

        [AuthorizeFilter(AccessType = AccessType.Account, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetUser(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ObjectContent<UserModel>(UserModel.MapUserModel(AccountBLL.GetUser(id), true), new JsonMediaTypeFormatter());
            return response;
        }

        [AuthorizeFilter]
        public HttpResponseMessage GetMe()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<UserModel>(UserModel.MapUserModel(AccountBLL.GetUser(loggedInUser.AccountSession.AccountId), true), new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost]
        [HttpOptions]
        [AuthorizeFilter]
        public HttpResponseMessage SaveMe(UserModel user)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            if (AccountBLL.UserNameExists(user.Username, user.Id))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new ObjectContent<string>
                    ("Användarnamnet finns redan, vänligen välj ett annat.", new JsonMediaTypeFormatter());
                return response;
            }
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            user.Id = loggedInUser.AccountSession.AccountId;
            user.ClubId = loggedInUser.AccountSession.ClubId;
            Account acc = UserModel.ConvertToAccount(user);
            Account account = AccountBLL.SaveAccount(acc);
            //response.Content = new ObjectContent<Account>(account, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost]
        [HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Account, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage SaveUser(UserModel user)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            if(AccountBLL.UserNameExists(user.Username, user.Id))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new ObjectContent<string>
                    ("Användarnamnet finns redan, vänligen välj ett annat.", new JsonMediaTypeFormatter());
                return response;
            }
            Account acc = UserModel.ConvertToAccount(user);
            Account account = AccountBLL.SaveAccount(acc);
            List<Account_Information_Generic_Value> genericValues = 
                AccountInformationGeneric.MapGenericValues(user.GenericValues, account.ID);
            AccountBLL.SaveGenericValues(genericValues, account.ClubId);
            //response.Content = new ObjectContent<Account>(account, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Account, AccessTypeRight = AccessTypeRight.Admin)]
        public HttpResponseMessage DeleteUser(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            AccountBLL.DeleteUser(id, loggedInUser.AccountSession.ClubId);
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Account, AccessTypeRight = AccessTypeRight.Admin)]
        public async Task<HttpResponseMessage> ImportUsersFromSportadmin()
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
                bool sendWelcomeMail = bool.Parse(((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Form["sendWelcomeMail"]);
                bool tryToMatchGroupName = bool.Parse(((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Form["tryToMatchGroupName"]);
                List<int> accessrightIds = new List<int>();
                try
                {
                    List<string> ids = ((System.Web.HttpContextWrapper)Request
                        .Properties["MS_HttpContext"]).Request.Form["accessrightIds"]
                        .ToString().Split(',').ToList();

                    ids.ForEach(i => accessrightIds.Add(int.Parse(i.Trim())));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(string.Format("Error importing users from sportadmin, cound't read accessrightIds."), 
                        ex, loggedInUser.AccountSession.ClubId);
                }

                AccountBLL.ImportUsersFromExcel(fileData.Result, loggedInUser.AccountSession.ClubId, sendWelcomeMail, tryToMatchGroupName, accessrightIds);
            }

            return response;
        }
    }
}
