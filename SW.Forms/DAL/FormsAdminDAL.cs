using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Forms.DAL
{
    internal class FormsAdminDAL
    {
        internal static List<Form> GetAllForms(int clubId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                return formDAL.Form.Where(f => !f.IsDeleted && f.ClubId == clubId).ToList();
            }
        }

        internal static void DeleteForm(int formId, int clubId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                Form form = formDAL.Form.FirstOrDefault(f => f.Id == formId && f.ClubId == clubId);
                if(form != null)
                {
                    form.IsDeleted = true;
                    formDAL.Entry(form).State = System.Data.Entity.EntityState.Modified;
                    formDAL.SaveChanges();
                }
            }
        }

        internal static List<Form> GetUserSubmits(int formId, int clubId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                int temp = 0;
                List<Form> forms = new List<Form>();
                List<Account> accounts = formDAL.Account.Include("FormSubmitValues").Where(a => a.FormSubmitValues.Any(f => f.FormId == formId)).ToList();
                Form form = formDAL.Form.Include("FormSubmitValues").Include("Form_Emails").FirstOrDefault(f => f.Id == formId && f.ClubId == clubId);
                foreach (var account in accounts)
                {
                    account.FormSubmitValues.ToList().ForEach(f => temp = f.FormFields.Id);

                    Form newForm = new Form()
                    {
                        Account = account,
                        ClubId = form.ClubId,
                        CreatedByUserId = form.CreatedByUserId,
                        CreatedDate = form.CreatedDate,
                        EndDate = form.EndDate,
                        Id = form.Id,
                        IsDeleted = form.IsDeleted,
                        IsExternal = form.IsExternal,
                        Name = form.Name,
                        StartDate = form.StartDate,
                        FormSubmitValues = account.FormSubmitValues.Where(f => f.FormId == formId).ToList()
                    };

                    forms.Add(newForm);
                }

                return forms;
            }
        }

        internal static int GetExternalFormAnswerCount(int formId, int clubId)
        {
            int count = 0;

            try
            {
                using (FormsEntities formDAL = new FormsEntities())
                {
                    count = formDAL.FormExternalSubmitValues.Where(f => f.FormId == formId).Select(f => f.Batch).Distinct().Count();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get external form answer count."), ex, clubId);
            }

            return count;
        }

        internal static int GetInternalFormAnswerCount(int formId, int clubId)
        {
            int count = 0;
            try
            {
                using (FormsEntities formDAL = new FormsEntities())
                {
                    count = formDAL.FormSubmitValues.Where(f => f.Id == formId && f.ClubId == clubId).Select(f => f.UserId).Distinct().Count();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't get internal form answer count."), ex, clubId);
            }

            return count;
        }

        internal static void SaveFormFieldItem(FormFields formField)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                if (formField.Id <= 0)
                    formDAL.Entry(formField).State = System.Data.Entity.EntityState.Added;
                else
                {
                    foreach (var option in formField.FormFieldsOptions)
                    {
                        if (option.Id <= 0)
                            formDAL.Entry(option).State = System.Data.Entity.EntityState.Added;
                        else
                            formDAL.Entry(option).State = System.Data.Entity.EntityState.Modified;
                    }

                    formDAL.Entry(formField).State = System.Data.Entity.EntityState.Modified;
                }

                formDAL.SaveChanges();
            }
        }

        internal static int GetExternalSubmitFormFieldId(int id, string emailFieldName)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                return formDAL.FormExternalSubmitValues.Include("FormFields").Where(f => f.FormId == id && f.FormFields.Label == emailFieldName)
                    .Select(f => f.FormFieldId).FirstOrDefault();
            }
        }

        internal static List<string> GetExternalSubmitValues(int formFieldId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                return formDAL.FormExternalSubmitValues.Where(f => f.FormFieldId == formFieldId).Select(f => f.Value).ToList();
            }
        }

        internal static List<Form> GetExternalSubmits(int formId, int clubId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                List<Form> forms = new List<Form>();
                Form form = formDAL.Form.Include("FormSubmitValues").Include("Form_Emails").FirstOrDefault(f => f.Id == formId && f.ClubId == clubId);
                var submits = formDAL.FormExternalSubmitValues.Where(f => f.FormId == formId).GroupBy(f => f.Batch).ToList();

                foreach (var submit in submits)
                {
                    Form newForm = new Form()
                    {
                        ClubId = form.ClubId,
                        CreatedByUserId = form.CreatedByUserId,
                        CreatedDate = form.CreatedDate,
                        EndDate = form.EndDate,
                        Id = form.Id,
                        IsDeleted = form.IsDeleted,
                        IsExternal = form.IsExternal,
                        Name = form.Name,
                        StartDate = form.StartDate,
                        FormFields = form.FormFields,
                        FormExternalSubmitValues = submit.ToList()
                    };

                    forms.Add(newForm);
                }

                forms.Reverse();
                return forms;
            }
        }

        internal static void DeleteFormFieldItem(int formFieldId, int clubId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                FormFields field = formDAL.FormFields.Include("Form").FirstOrDefault(f => f.Id == formFieldId && f.Form.ClubId == clubId);
                if(field != null)
                {
                    formDAL.Entry(field).State = System.Data.Entity.EntityState.Deleted;
                    formDAL.SaveChanges();
                }
            }
        }

        internal static void DeleteExternalFormAnswer(string batch, int clubId)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                var submitValues = formDAL.FormExternalSubmitValues.Include("Form").Where(f => f.Batch == batch && f.Form.ClubId == clubId);

                foreach (var item in submitValues)
                {
                    formDAL.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                formDAL.SaveChanges();
            }
        }

        internal static void DeleteFormFieldOption(int optionId, int clubId)
        {
            //TODO: Make clubId check....
            using (FormsEntities formDAL = new FormsEntities())
            {
                FormFieldsOptions option = formDAL.FormFieldsOptions.FirstOrDefault(f => f.Id == optionId);
                if (option != null)
                {
                    formDAL.Entry(option).State = System.Data.Entity.EntityState.Deleted;
                    formDAL.SaveChanges();
                }
            }
        }

        internal static void SaveForm(Form form)
        {
            using (FormsEntities formDAL = new FormsEntities())
            {
                if (form.Id > 0)
                {
                    foreach (var field in form.FormFields)
                    {
                        if (field.Id <= 0)
                            formDAL.Entry(field).State = System.Data.Entity.EntityState.Added;
                        else
                        {
                            foreach (var option in field.FormFieldsOptions)
                            {
                                if (option.Id <= 0)
                                    formDAL.Entry(option).State = System.Data.Entity.EntityState.Added;
                            }
                        }
                    }

                    formDAL.SaveChanges();
                    formDAL.Entry(form).State = System.Data.Entity.EntityState.Modified;
                }
                else
                    formDAL.Form.Add(form);

                foreach (var email in form.Form_Emails)
                {
                    if (email.Id > 0)
                        formDAL.Entry(email).State = System.Data.Entity.EntityState.Modified;
                    else
                        formDAL.Entry(email).State = System.Data.Entity.EntityState.Added;
                }

                formDAL.SaveChanges();
            }
        }

        internal static Form GetForm(int formId, int clubId)
        {
            Form form = null;
            try
            {
                using (FormsEntities formDAL = new FormsEntities())
                {
                    form = formDAL.Form.Include("FormFields").Include("Form_Emails").FirstOrDefault(f => f.Id == formId && f.ClubId == clubId);
                    form.FormFields.ToList().ForEach(f => f.FormFieldsOptions.ToList());
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Error getting form: {1}", formId), ex, clubId);
            }

            return form;
        }
    }
}
