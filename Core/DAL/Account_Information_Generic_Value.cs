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
    
    public partial class Account_Information_Generic_Value
    {
        public int Id { get; set; }
        public int AccountInformationGenericId { get; set; }
        public int AccountId { get; set; }
        public string Value { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
        public virtual Account_Information_Generic Account_Information_Generic { get; set; }
        public virtual Account_Information_Generic Account_Information_Generic1 { get; set; }
    }
}
