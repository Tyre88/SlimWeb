using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Forms.DAL
{
    internal class FormsDAL
    {
        internal static List<Form> GetUnansweredForms(int clubId, int accountId, int count)
        {
            using (FormsEntities formsDAL = new FormsEntities())
            {
                return formsDAL.Form.Include("FormSubmitValues").Include("Form_Emails").Where(f => f.ClubId == clubId && f.StartDate < DateTime.Now && f.EndDate > DateTime.Now 
                    && !f.IsDeleted && !f.IsExternal && !f.FormSubmitValues.Any(s => s.UserId == accountId)).Take(count).ToList();
            }
        }

        internal static Form GetForm(int clubId, int formId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                Form form = formDAL.Form.Include("FormFields").Include("Form_Emails").FirstOrDefault(f => f.Id == formId && f.ClubId == clubId 
                    && f.StartDate < DateTime.Now && f.EndDate > DateTime.Now && !f.IsDeleted && !f.IsExternal);
                form.FormFields.ToList().ForEach(f => f.FormFieldsOptions.ToList());
                return form;
            }
        }

        internal static List<Form_Emails> GetFormEmails(int formId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                return formDAL.Form_Emails.Where(f => f.FormId == formId).ToList();
            }
        }

        internal static Form GetForm(string clubShortName, string formName)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                Form form = formDAL.Form.Include("FormFields").Include("Form_Emails").FirstOrDefault(f => f.Name == formName && f.Club.ShortName == clubShortName
                    && f.StartDate < DateTime.Now && f.EndDate > DateTime.Now && !f.IsDeleted && f.IsExternal);
                form.FormFields.ToList().ForEach(f => f.FormFieldsOptions.ToList());
                return form;
            }
        }

        internal static void SubmitForm(List<FormSubmitValues> submitValues)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                submitValues.ForEach(s => formDAL.FormSubmitValues.Add(s));
                formDAL.SaveChanges();
            }
        }

        internal static void SubmitExternalForm(List<FormExternalSubmitValues> submitValues)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                submitValues.ForEach(s => formDAL.FormExternalSubmitValues.Add(s));
                formDAL.SaveChanges();
            }
        }

        internal static Form GetExternalForm(int formId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                Form form = formDAL.Form.Include("FormFields").Include("Form_Emails").FirstOrDefault(f => f.Id == formId && f.IsExternal);
                form.FormFields.ToList().ForEach(f => f.FormFieldsOptions.ToList());
                return form;
            }
        }

        internal static Form GetForm(int formId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                Form form = formDAL.Form.Include("FormFields").Include("Form_Emails").FirstOrDefault(f => f.Id == formId);
                form.FormFields.ToList().ForEach(f => f.FormFieldsOptions.ToList());
                return form;
            }
        }
    }
}
