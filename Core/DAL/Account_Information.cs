//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SW.Core.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account_Information
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Occupation { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public int Grade { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Weight { get; set; }
        public string Theme { get; set; }
    
        public virtual Account Account { get; set; }
    }
}