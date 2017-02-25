using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Forms.Admin
{
    public class UserFormSubmit
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserImage { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public List<FormSubmitValuesModel> FormSubmits { get; set; }

        public UserFormSubmit()
        {
            FormSubmits = new List<FormSubmitValuesModel>();
        }

        public static UserFormSubmit MapUserFormSubmit(SW.Forms.DAL.Form form, bool deepLoad = false)
        {
            UserFormSubmit model = new UserFormSubmit()
            {
                FormId = form.Id,
                FormName = form.Name,
                FirstName = form.Account.FirstName,
                LastName = form.Account.LastName,
                UserImage = form.Account.Image
            };

            if(deepLoad)
            {
                foreach (var item in form.FormSubmitValues)
                {
                    model.FormSubmits.Add(new FormSubmitValuesModel()
                    {
                        Id = item.Id,
                        Name = item.FormFields.Label,
                        Value = item.Value
                    });
                }
            }

            return model;
        }

        public static List<UserFormSubmit> MapUserFormSubmits(List<SW.Forms.DAL.Form> forms, bool deepLoad = false)
        {
            List<UserFormSubmit> models = new List<UserFormSubmit>();
            forms.ForEach(f => models.Add(MapUserFormSubmit(f, deepLoad)));
            return models;
        }
    }
}