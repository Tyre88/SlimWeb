using SW.Core.Entities;
using SW.Core.Enums;
using SW.Core.Filters;
using SW.Core.Helpers;
using SW.Forms.BLL;
using SlimWeb.Models;
using SlimWeb.Models.Forms;
using SlimWeb.Models.Forms.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace SlimWeb.Controllers.Forms
{
    public class FormsAdminController : ApiController
    {
        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage GetAllForms()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            List<FormModel> forms = FormModel.MapFormModels(FormsAdminBLL.GetAllForms(loggedInUser.AccountSession.ClubId));
            forms.ForEach(f => f.AnswerCount = FormsAdminBLL.GetFormAnswerCount(f.Id, f.IsExternal, loggedInUser.AccountSession.ClubId));
            response.Content = new ObjectContent<List<FormModel>>(forms, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Admin)]
        public HttpResponseMessage DeleteForm(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            FormsAdminBLL.DeleteForm(id, loggedInUser.AccountSession.ClubId);
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        //[ObjectChangeFilter(IdentifierProperty = "Id", ChangeType = ChangeType.Get)]
        public HttpResponseMessage GetForm(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<FormModel>(FormModel.MapFormModel(FormsAdminBLL.GetForm(id, loggedInUser.AccountSession.ClubId), true), 
                new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        //[ObjectChangeFilter(IdentifierProperty = "Id", ChangeType = ChangeType.Save)]
        public HttpResponseMessage SaveForm(FormModel form)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            form.ClubId = loggedInUser.AccountSession.ClubId;
            if (form.Id <= 0)
            {
                form.CreatedByUserId = loggedInUser.AccountSession.AccountId;
                form.CreatedDate = DateTime.Now;
            }

            SW.Forms.DAL.Form f = FormModel.MapModelToForm(form);
            FormsAdminBLL.SaveForm(f);
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage GetUserSubmits(int formId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<List<UserFormSubmit>>(UserFormSubmit.MapUserFormSubmits(
                FormsAdminBLL.GetUserSubmits(formId, loggedInUser.AccountSession.ClubId), true),
                new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage ExportGetUserSubmitsToExcel(int formId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            List<UserFormSubmit> submits = UserFormSubmit.MapUserFormSubmits(
                FormsAdminBLL.GetUserSubmits(formId, loggedInUser.AccountSession.ClubId), true);

            DataTable dt = new DataTable();

            foreach (var item in submits)
            {
                string[] values = new string[item.FormSubmits.Count];
                int counter = 0;
                foreach (var submit in item.FormSubmits)
                {
                    if(!dt.Columns.Contains(submit.Name))
                    {
                        dt.Columns.Add(new DataColumn(submit.Name, typeof(string)));
                    }

                    values[counter] = submit.Value;
                    counter++;
                }

                dt.Rows.Add(values);
            }
            var form = FormsAdminBLL.GetForm(formId, loggedInUser.AccountSession.ClubId);
            response.Content = new ObjectContent<string>(ExcelHelper.CreateExcelDocument(dt, form.Name, true, "Forms"), new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage GetExternalAnswers(int formId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            response.Content = new ObjectContent<List<ExternalFormSubmit>>(ExternalFormSubmit.MapExternalFormSubmits(
                FormsAdminBLL.GetExternalSubmits(formId, loggedInUser.AccountSession.ClubId)),
                new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage ExportExternalAnswersToExcel(int formId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            List<ExternalFormSubmit> submits = ExternalFormSubmit.MapExternalFormSubmits(
                FormsAdminBLL.GetExternalSubmits(formId, loggedInUser.AccountSession.ClubId));

            DataTable dt = new DataTable();

            foreach (var item in submits)
            {
                string[] values = new string[item.FormSubmits.Count];
                int counter = 0;
                foreach (var submit in item.FormSubmits)
                {
                    if (!dt.Columns.Contains(submit.Name))
                    {
                        dt.Columns.Add(new DataColumn(submit.Name, typeof(string)));
                    }

                    values[counter] = submit.Value;
                    counter++;
                }

                dt.Rows.Add(values);
            }
            var form = FormsAdminBLL.GetForm(formId, loggedInUser.AccountSession.ClubId);
            response.Content = new ObjectContent<string>(ExcelHelper.CreateExcelDocument(dt, form.Name, true, "Forms"), new JsonMediaTypeFormatter());
            return response;
        }

        [HttpDelete, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage DeleteFormFieldItem(int formFieldId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            FormsAdminBLL.DeleteFormFieldItem(formFieldId, loggedInUser.AccountSession.ClubId);
            return response;
        }

        [HttpDelete, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage DeleteFormFieldOption(int optionId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            FormsAdminBLL.DeleteFormFieldOption(optionId, loggedInUser.AccountSession.ClubId);
            return response;
        }

        [HttpPost, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Write)]
        public HttpResponseMessage SaveFormFieldItem(FormFieldModel formField)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            FormsAdminBLL.SaveFormFieldItem(FormFieldModel.MapFormFieldDbModel(formField));
            return response;
        }

        [HttpDelete, HttpOptions]
        [AuthorizeFilter(AccessType = AccessType.Forms, AccessTypeRight = AccessTypeRight.Admin)]
        public HttpResponseMessage DeleteExternalFormAnswer(string batch)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            UserPrincipal loggedInUser = (UserPrincipal)HttpContext.Current.User;
            FormsAdminBLL.DeleteExternalFormAnswer(batch, loggedInUser.AccountSession.ClubId);
            return response;
        }
    }
}
