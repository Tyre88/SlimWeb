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
    
    public partial class Search
    {
        public int id { get; set; }
        public string query { get; set; }
        public Nullable<int> hitsTec { get; set; }
        public Nullable<int> hitsWord { get; set; }
        public string ip { get; set; }
        public string date { get; set; }
    }
}