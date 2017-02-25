using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Forms.DAL;
using SW.Core.Helpers;
using System.IO;
using System.Data;
using SW.Forms.Entities;

namespace SW.Forms.BLL
{
    public class FormsBLL
    {
        public static List<Form> GetUnansweredForms(int clubId, int accountId, int count)
        {
            return FormsDAL.GetUnansweredForms(clubId, accountId, count);
        }

        public static Form GetForm(int clubId, int formId)
        {
            return FormsDAL.GetForm(clubId, formId);
        }

        public static void SubmitForm(List<FormSubmitValues> submitValues)
        {
            FormsDAL.SubmitForm(submitValues);
            List<Form_Emails> emails = GetFormEmails(submitValues[0].FormId);

            string emailHtml = GenerateEmail(submitValues);
            string emailSubject = GenerateEmailSubject(submitValues[0].FormId);

            foreach (var email in emails)
            {
                EmailHelper.SendEmail(email.Email, emailHtml, emailSubject);
            }
        }

        private static List<Form_Emails> GetFormEmails(int formId)
        {
            return FormsDAL.GetFormEmails(formId);
        }

        public static void SubmitExternalForm(List<FormExternalSubmitValues> submitValues)
        {
            string batch = Guid.NewGuid().ToString();
            submitValues.ForEach(s => s.Batch = batch);
            FormsDAL.SubmitExternalForm(submitValues);

            Form form = FormsDAL.GetExternalForm(submitValues[0].FormId);
            List<Form_Emails> emails = form.Form_Emails.ToList();

            string emailHtml = GenerateExternalEmail(submitValues);
            string emailSubject = GenerateEmailSubject(submitValues[0].FormId);

            try
            {
                foreach (var email in emails)
                {
                    EmailHelper.SendEmail(email.Email, emailHtml, emailSubject);
                }

                if (form.SendThanksMail)
                {
                    int formFieldId = FormsAdminBLL.GetExternalSubmitFormFieldId(form.Id, form.EmailFieldName);
                    string email = submitValues.FirstOrDefault(s => s.FormFieldId == formFieldId).Value;

                    HandleFormEmail(form, emailHtml);

                    if (!string.IsNullOrEmpty(email))
                        EmailHelper.SendEmail(email, form.EmailHtml, form.Name);
                    else
                        LogHelper.LogWarn(string.Format("Couldn't send Email for submission {0}, form: {1}", batch, form.Name), new Exception(), form.ClubId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error sending email for external form submit", ex, form.ClubId);
            }
        }

        public static FormExcelImportResult SubmitExternalFormFromExcel(byte[] result, int formId)
        {
            FormExcelImportResult importResult = new FormExcelImportResult();
            try
            {
                MemoryStream ms = new MemoryStream(result);
                DataTable dt = ExcelHelper.GetDataTableFromXls(ms);

                Form form = GetForm(formId);
                bool valid = true;
                //Validating
                foreach (var item in form.FormFields)
                {
                    if (!dt.Columns.Contains(item.Label))
                        valid = false;
                }

                if (!valid)
                {
                    LogHelper.LogInfo("Excel file not valid for submission.", -1);
                    importResult.Message = "Import filen är inte giltig, vänligen ladda hem mall filen och prova med den. ";
                    return importResult;
                }

                foreach (DataRow row in dt.Rows)
                {
                    List<FormExternalSubmitValues> submitValues = new List<FormExternalSubmitValues>();

                    foreach (var item in form.FormFields)
                    {
                        submitValues.Add(new FormExternalSubmitValues()
                        {
                            FormId = formId,
                            FormFieldId = item.Id,
                            Value = row[item.Label].ToString(),
                            Date = DateTime.Now
                        });
                    }

                    SubmitExternalForm(submitValues);
                    importResult.Count++;
                }

                importResult.Success = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error importing excel to form. FormId: " + formId, ex, -1);
                importResult.Message = "Ett oväntat fel uppstod, vänligen försök igen.";
            }

            return importResult;
        }

        private static void HandleFormEmail(Form form, string emailHtml)
        {
            form.EmailHtml = form.EmailHtml.Replace("#FORM#", emailHtml);
        }

        public static Form GetForm(string clubShortName, string formName)
        {
            return FormsDAL.GetForm(clubShortName, formName);
        }

        private static string GenerateEmail(List<FormSubmitValues> submitValues)
        {
            string html = string.Empty;
            FormSubmitValues firstValue = submitValues.First();
            Form form = GetForm(firstValue.ClubId, firstValue.FormId);

            foreach (var value in submitValues)
            {
                html = string.Format("{0} <p>{1}: {2}</p>",
                    html,
                    form.FormFields.FirstOrDefault(f => f.Id == value.FormFieldId).Label,
                    value.Value);
            }

            return html;
        }

        private static string GenerateExternalEmail(List<FormExternalSubmitValues> submitValues)
        {
            string html = string.Empty;
            FormExternalSubmitValues firstValue = submitValues.First();
            Form form = GetExternalForm(firstValue.FormId);

            foreach (var value in submitValues)
            {
                html = string.Format("{0} <p>{1}: {2}</p>",
                    html,
                    form.FormFields.FirstOrDefault(f => f.Id == value.FormFieldId).Label,
                    value.Value);
            }

            return html;
        }

        private static string GenerateEmailSubject(int formId)
        {
            string subject = string.Empty;
            Form form = GetForm(formId);
            subject = string.Format("Nytt formulär svar: {0}", form.Name);
            return subject;
        }

        private static Form GetForm(int formId)
        {
            return FormsDAL.GetForm(formId);
        }

        private static Form GetExternalForm(int formId)
        {
            return FormsDAL.GetExternalForm(formId);
        }
    }
}
