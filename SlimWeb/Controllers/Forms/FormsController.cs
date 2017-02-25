using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SW.Forms.BLL;
using SlimWeb.Models;
using SlimWeb.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers.Forms
{
    public class FormsController : ApiController
    {
        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetForms()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;

            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetUnansweredForms(int count)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            List<FormModel> forms = FormModel.MapFormModels(FormsBLL.GetUnansweredForms(loggedInUser.AccountSession.ClubId, 
                loggedInUser.AccountSession.AccountId, count)); 
            response.Content = new ObjectContent<List<FormModel>>(forms, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage GetForm(int formId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            FormModel form = FormModel.MapFormModel(FormsBLL.GetForm(loggedInUser.AccountSession.ClubId, formId), true);
            response.Content = new ObjectContent<FormModel>(form, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Read)]
        public HttpResponseMessage SubmitForm(List<SubmitFormFieldModel> formFields)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            FormsBLL.SubmitForm(SubmitFormFieldModel.MapModelToFormSubmitValues(formFields, 
                loggedInUser.AccountSession.AccountId, loggedInUser.AccountSession.ClubId));
            return response;
        }
    }
}
