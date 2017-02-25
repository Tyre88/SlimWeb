using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SW.Forms.DAL;

namespace SlimWeb.Models.Forms
{
    public class FormFieldModel
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string ClassName { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public bool IsRequired { get; set; }
        public bool CanMultiply { get; set; }
        public List<FormFieldOptionModel> Options { get; set; }

        public FormFieldModel()
        {
            Options = new List<FormFieldOptionModel>();
        }

        internal static FormFieldModel MapFormFieldModel(FormFields formField)
        {
            FormFieldModel model = new FormFieldModel()
            {
                ClassName = formField.ClassName,
                FormId = formField.FormId,
                Id = formField.Id,
                IsRequired = formField.IsRequired,
                Label = formField.Label,
                Type = formField.Type,
                CanMultiply = formField.CanMultiply
            };

            formField.FormFieldsOptions.ToList().ForEach(f => model.Options.Add(FormFieldOptionModel.MapFormFieldOptionModel(f)));

            return model;
        }

        internal static FormFields MapFormFieldDbModel(FormFieldModel model)
        {
            FormFields formField = new FormFields()
            {
                ClassName = model.ClassName,
                FormId = model.FormId,
                Id = model.Id,
                IsRequired = model.IsRequired,
                Label = model.Label,
                Type = model.Type,
                CanMultiply = model.CanMultiply
            };

            if(model.Options.Count > 0)
            {
                formField.FormFieldsOptions = new List<FormFieldsOptions>();

                foreach (var option in model.Options)
                {
                    formField.FormFieldsOptions.Add(MapFormFieldOption(option));
                }
            }

            return formField;
        }

        private static FormFieldsOptions MapFormFieldOption(FormFieldOptionModel option)
        {
            return new FormFieldsOptions()
            {
                FormFieldId = option.FormFieldId,
                GroupName = option.GroupName,
                Id = option.Id,
                Name = option.Name
            };
        }
    }
}