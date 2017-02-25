using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SlimWeb.Models;
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

namespace SlimWeb.Controllers.Core
{
    public class FileController : ApiController
    {
        [AuthorizeFilter]
        public async Task<HttpResponseMessage> UploadFile()
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
                string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());
                string directory = string.Format(@"{0}Uploads\{1}", AppDomain.CurrentDomain.BaseDirectory, loggedInUser.AccountSession.ClubId);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (FileStream fs = new FileStream(string.Format(@"{0}\{1}", directory, fileName), FileMode.OpenOrCreate))
                {
                    await fs.WriteAsync(fileData.Result, 0, fileData.Result.Length);
                    fs.Close();
                }

                response.Content = new ObjectContent<string>(fileName, new JsonMediaTypeFormatter());
            }

            return response;
        }

        [AuthorizeFilter]
        public async Task<HttpResponseMessage> UploadGenericFile()
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
                string name = provider.Contents.First().Headers.ContentDisposition.FileName.Replace("/", string.Empty);
                string ext = name.Substring(name.LastIndexOf("."), name.Length);
                string fileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), ext);
                string directory = string.Format(@"{0}Uploads\{1}", AppDomain.CurrentDomain.BaseDirectory, loggedInUser.AccountSession.ClubId);

                int objectId = int.Parse(HttpContext.Current.Request.QueryString["objectId"].ToString());
                GenericFileType fileType = (GenericFileType)Enum.Parse(typeof(GenericFileType), HttpContext.Current.Request.QueryString["fileType"].ToString());
                GenericFileModuleType fileModuleType = (GenericFileModuleType)Enum.Parse(typeof(GenericFileModuleType), HttpContext.Current.Request.QueryString["fileModuleType"].ToString());

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (FileStream fs = new FileStream(string.Format(@"{0}\{1}", directory, fileName), FileMode.OpenOrCreate))
                {
                    await fs.WriteAsync(fileData.Result, 0, fileData.Result.Length);
                    fs.Close();
                }

                response.Content = new ObjectContent<string>(fileName, new JsonMediaTypeFormatter());
            }

            return response;
        }

        public async Task<HttpResponseMessage> UploadFilePublic()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            if (!Request.Content.IsMimeMultipartContent())
            {
                response.StatusCode = HttpStatusCode.UnsupportedMediaType;
            }
            else
            {
                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Task<byte[]> fileData = provider.Contents.First().ReadAsByteArrayAsync();
                string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());
                string directory = string.Format(@"{0}Uploads\Public", AppDomain.CurrentDomain.BaseDirectory);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (FileStream fs = new FileStream(string.Format(@"{0}\{1}", directory, fileName), FileMode.OpenOrCreate))
                {
                    await fs.WriteAsync(fileData.Result, 0, fileData.Result.Length);
                    fs.Close();
                }

                response.Content = new ObjectContent<string>(fileName, new JsonMediaTypeFormatter());
            }

            return response;
        }
    }
}
