using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Newsletter
{
    public class SendNewsletterModel
    {
        public int NewsletterId { get; set; }
        public List<int> AccessrightIds { get; set; }
        public List<int> ContactIds { get; set; }
        public int FormFieldId { get; set; }
    }
}