using SW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models
{
    public class UserInformationModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Occupation { get; set; }
        public Grade Grade { get; set; }
        public DateTime Birthday { get; set; }
        public string Weight { get; set; }
        public string Theme { get; set; }
    }
}