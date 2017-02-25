//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SW.Forms.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Form
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Form()
        {
            this.Form_Emails = new HashSet<Form_Emails>();
            this.Form_Emails1 = new HashSet<Form_Emails>();
            this.FormExternalSubmitValues = new HashSet<FormExternalSubmitValues>();
            this.FormExternalSubmitValues1 = new HashSet<FormExternalSubmitValues>();
            this.FormFields = new HashSet<FormFields>();
            this.FormSubmitValues = new HashSet<FormSubmitValues>();
            this.FormSubmitValues1 = new HashSet<FormSubmitValues>();
        }
    
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }
        public int ClubId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsExternal { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool MultipleSubmits { get; set; }
        public bool SendThanksMail { get; set; }
        public string EmailFieldName { get; set; }
        public string EmailHtml { get; set; }
        public bool EnableExcelImport { get; set; }
        public string ExampleExcelPath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Form_Emails> Form_Emails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Form_Emails> Form_Emails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormExternalSubmitValues> FormExternalSubmitValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormExternalSubmitValues> FormExternalSubmitValues1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormFields> FormFields { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormSubmitValues> FormSubmitValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormSubmitValues> FormSubmitValues1 { get; set; }
        public virtual Account Account { get; set; }
        public virtual Club Club { get; set; }
    }
}
