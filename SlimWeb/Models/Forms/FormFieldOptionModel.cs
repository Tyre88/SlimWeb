using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SW.Forms.DAL;

namespace SlimWeb.Models.Forms
{
    public class FormFieldOptionModel
    {
        public int Id { get; set; }
        public int FormFieldId { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }

        internal static FormFieldOptionModel MapFormFieldOptionModel(FormFieldsOptions formFieldOption)
        {
            FormFieldOptionModel model = new FormFieldOptionModel()
            {
                FormFieldId = formFieldOption.FormFieldId,
                GroupName = formFieldOption.GroupName,
                Id = formFieldOption.Id,
                Name = formFieldOption.Name
            };

            return model;
        }
    }
}