using SW.Forms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Forms
{
    public class SubmitFormFieldModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int FormId { get; set; }
        public int FormFieldId { get; set; }

        public static FormSubmitValues MapModelToFormSubmitValue(SubmitFormFieldModel model, int userId, int clubId)
        {
            return new FormSubmitValues()
            {
                ClubId = clubId,
                UserId = userId,
                Value = model.Value,
                Id = model.Id,
                FormId = model.FormId,
                FormFieldId = model.FormFieldId
            };
        }

        public static FormExternalSubmitValues MapModelToExternalFormSubmitValue(SubmitFormFieldModel model)
        {
            return new FormExternalSubmitValues()
            {
                Date = DateTime.Now,
                FormFieldId = model.FormFieldId,
                FormId = model.FormId,
                Value = model.Value
            };
        }

        public static List<FormSubmitValues> MapModelToFormSubmitValues(List<SubmitFormFieldModel> models, int userId, int clubId)
        {
            List<FormSubmitValues> formSubmitValues = new List<FormSubmitValues>();
            models.ForEach(m => formSubmitValues.Add(MapModelToFormSubmitValue(m, userId, clubId)));
            return formSubmitValues;
        }

        internal static List<FormExternalSubmitValues> MapModelToExternalFormSubmitValues(List<SubmitFormFieldModel> models)
        {
            List<FormExternalSubmitValues> formSubmitValues = new List<FormExternalSubmitValues>();
            models.ForEach(m => formSubmitValues.Add(MapModelToExternalFormSubmitValue(m)));
            return formSubmitValues;
        }
    }
}