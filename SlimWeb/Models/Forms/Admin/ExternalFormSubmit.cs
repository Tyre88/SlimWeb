using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Forms.Admin
{
    public class ExternalFormSubmit
    {
        public string Batch { get; set; }
        public List<FormSubmitValuesModel> FormSubmits { get; set; }

        public ExternalFormSubmit()
        {
            FormSubmits = new List<FormSubmitValuesModel>();
        }

        public static ExternalFormSubmit MapExternalFormSubmit(SW.Forms.DAL.Form form)
        {
            ExternalFormSubmit model = new ExternalFormSubmit();

            try
            {
                model.Batch = form.FormExternalSubmitValues.First().Batch;
                foreach (var item in form.FormExternalSubmitValues.OrderBy(f => f.FormFields.Id))
                {
                    model.FormSubmits.Add(new FormSubmitValuesModel()
                    {
                        Id = item.Id,
                        Name = item.FormFields.Label,
                        Value = item.Value
                    });
                }
            }
            catch (Exception)
            {
            }

            return model;
        }

        public static List<ExternalFormSubmit> MapExternalFormSubmits(List<SW.Forms.DAL.Form> forms)
        {
            List<ExternalFormSubmit> models = new List<ExternalFormSubmit>();
            forms.ForEach(f => models.Add(MapExternalFormSubmit(f)));
            return models;
        }
    }
}