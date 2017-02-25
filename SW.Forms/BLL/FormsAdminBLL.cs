using SW.Core.Helpers;
using SW.Forms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Forms.BLL
{
    public class FormsAdminBLL
    {
        public static List<Form> GetAllForms(int clubId)
        {
            return FormsAdminDAL.GetAllForms(clubId);
        }

        public static void DeleteForm(int formId, int clubId)
        {
            FormsAdminDAL.DeleteForm(formId, clubId);
        }

        public static Form GetForm(int formId, int clubId)
        {
            return FormsAdminDAL.GetForm(formId, clubId);
        }

        public static void SaveForm(Form form)
        {
            if (form.EnableExcelImport)
            {
                DataTable dt = CreateDataTableFromFormFields(form.FormFields);
                form.ExampleExcelPath = ExcelHelper.CreateExcelDocument(dt, form.Name, true, "Forms");
            }

            FormsAdminDAL.SaveForm(form);
        }

        private static DataTable CreateDataTableFromFormFields(ICollection<FormFields> formFields)
        {
            DataTable dt = new DataTable();
            foreach (var item in formFields)
            {
                if (!dt.Columns.Contains(item.Label))
                {
                    dt.Columns.Add(new DataColumn(item.Label, typeof(string)));
                }
            }
            return dt;
        }

        public static int GetFormAnswerCount(int formId, bool isExternal, int clubId)
        {
            int count = 0;

            if(!isExternal)
                count = FormsAdminDAL.GetInternalFormAnswerCount(formId, clubId);
            else
                count = FormsAdminDAL.GetExternalFormAnswerCount(formId, clubId);

            return count;
        }

        public static List<Form> GetUserSubmits(int formId, int clubId)
        {
            return FormsAdminDAL.GetUserSubmits(formId, clubId);
        }

        public static void DeleteFormFieldItem(int formFieldId, int clubId)
        {
            FormsAdminDAL.DeleteFormFieldItem(formFieldId, clubId);
        }

        public static List<Form> GetExternalSubmits(int formId, int clubId)
        {
            return FormsAdminDAL.GetExternalSubmits(formId, clubId);
        }

        public static List<string> GetExternalSubmitValues(int formFieldId)
        {
            return FormsAdminDAL.GetExternalSubmitValues(formFieldId);
        }

        internal static int GetExternalSubmitFormFieldId(int id, string emailFieldName)
        {
            return FormsAdminDAL.GetExternalSubmitFormFieldId(id, emailFieldName);
        }

        public static void SaveFormFieldItem(FormFields formField)
        {
            FormsAdminDAL.SaveFormFieldItem(formField);
        }

        public static void DeleteFormFieldOption(int optionId, int clubId)
        {
            FormsAdminDAL.DeleteFormFieldOption(optionId, clubId);
        }

        public static void DeleteExternalFormAnswer(string batch, int clubId)
        {
            FormsAdminDAL.DeleteExternalFormAnswer(batch, clubId);
        }
    }
}
