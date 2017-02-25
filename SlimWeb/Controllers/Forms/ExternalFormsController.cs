using SW.Forms.BLL;
using SW.Forms.Entities;
using SlimWeb.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace SlimWeb.Controllers.Forms
{
    public class ExternalFormsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetForm(string clubShortName, string formName)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            FormModel form = FormModel.MapFormModel(FormsBLL.GetForm(clubShortName, formName), true);
            response.Content = new ObjectContent<FormModel>(form, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, HttpOptions]
        public HttpResponseMessage SubmitForm(List<SubmitFormFieldModel> formFields)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            FormsBLL.SubmitExternalForm(SubmitFormFieldModel.MapModelToExternalFormSubmitValues(formFields));
            return response;
        }

        [HttpPost, HttpOptions]
        public async Task<HttpResponseMessage> ImportExcelFile()
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
                int formId = int.Parse(((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Form["formId"]);

                FormExcelImportResult result = FormsBLL.SubmitExternalFormFromExcel(fileData.Result, formId);

                response.Content = new ObjectContent<FormExcelImportResult>(result, new JsonMediaTypeFormatter());
            }

            return response;
        }
    }
}
