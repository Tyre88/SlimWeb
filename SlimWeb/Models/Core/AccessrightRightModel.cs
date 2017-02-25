using SW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models
{
    public class AccessrightRightModel
    {
        public int Id { get; set; }
        public AccessType AccessType { get; set; }
        public AccessTypeRight AccessTypeRight { get; set; }
    }
}