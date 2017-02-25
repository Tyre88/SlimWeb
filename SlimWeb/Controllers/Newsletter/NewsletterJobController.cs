using SW.Newsletter.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace SlimWeb.Controllers.Newsletter
{
    public class NewsletterJobController : ApiController
    {
        [HttpGet, HttpPost, HttpOptions]
        public HttpResponseMessage StartSendingJob()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            NewsletterJobBLL.SendNewsletters();
            response.Content = new ObjectContent<string>
                ("Done",
                new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet, HttpPost, HttpOptions]
        public HttpResponseMessage ReadNewsletter(string newsletterSendItemGuid)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            NewsletterJobBLL.ReadNewsletter(newsletterSendItemGuid);
            return response;
        }
    }
}
