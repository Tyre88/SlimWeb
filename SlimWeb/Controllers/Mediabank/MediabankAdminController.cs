using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SW.Core.Interfaces;
using SW.Entities.Mediabank;
using SW.Mediabank.BLL;
using SW.Mediabank.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers.Mediabank
{
    public class MediabankAdminController : ApiController
    {
        private IMediabank _mediabank;
        private IGenericItemPermissionsBLL _genericItemPermissionBLL;

        public MediabankAdminController(IMediabank mediabank, IGenericItemPermissionsBLL genericItemPermissionBLL)
        {
            _mediabank = mediabank;
            _genericItemPermissionBLL = genericItemPermissionBLL;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Mediabank, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetAllFiles()
        {
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            IList<MediabankEntity> mediabankFiles = _mediabank.GetAllFiles(loggedInUser.AccountSession.ClubId);
            response.Content = new ObjectContent<IList<MediabankEntity>>(mediabankFiles, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Mediabank, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetAllFilesWithType(string fileType)
        {
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            IList<MediabankEntity> mediabankFiles = _mediabank.GetAllFilesWithType(loggedInUser.AccountSession.ClubId, fileType);
            response.Content = new ObjectContent<IList<MediabankEntity>>(mediabankFiles, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Mediabank, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetFile(int fileId)
        {
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MediabankEntity mediabankFile = _mediabank.GetFile(loggedInUser.AccountSession.ClubId, fileId);
            response.Content = new ObjectContent<MediabankEntity>(mediabankFile, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost]
        [AuthorizeFilter(AccessType = AccessType.Mediabank, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage UpdateMediabankFile(MediabankEntity file)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            bool success = _mediabank.UpdateMediabankFile(file);
            response.Content = new ObjectContent<bool>(success, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpDelete]
        [AuthorizeFilter(AccessType = AccessType.Mediabank, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage DeleteMediabankFile(int id)
        {
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            bool success = _mediabank.DeleteMediabankFile(id, loggedInUser.AccountSession.ClubId);
            response.Content = new ObjectContent<bool>(success, new JsonMediaTypeFormatter());
            return response;
        }

        [AuthorizeFilter(AccessType = AccessType.Mediabank, AccessTypeRight = AccessTypeRight.Write)]
        public async Task<HttpResponseMessage> UploadMediabankFile()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            if (!Request.Content.IsMimeMultipartContent())
            {
                response.StatusCode = HttpStatusCode.UnsupportedMediaType;
            }
            else
            {
                MediabankEntity mediabankFile = new MediabankEntity();

                UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
                mediabankFile.ClubId = loggedInUser.AccountSession.ClubId;
                mediabankFile.CreatedById = loggedInUser.AccountSession.AccountId;

                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                mediabankFile.Name = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Form["fileName"].ToString();
                mediabankFile.Description = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Form["fileDescription"].ToString();

                await Request.Content.ReadAsMultipartAsync(provider)
                    .ContinueWith(async o =>
                    {
                        var fileContent = provider.Contents.FirstOrDefault();
                        string name = string.Empty;

                        if (fileContent != null)
                        {
                            name = fileContent.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                        }

                        byte[] fileData = await provider.Contents.First().ReadAsByteArrayAsync();

                        response.Content = new ObjectContent<MediabankEntity>(_mediabank.SaveFile(fileData, name, mediabankFile), new JsonMediaTypeFormatter());
                    });
            }

            return response;
        }
    }
}
