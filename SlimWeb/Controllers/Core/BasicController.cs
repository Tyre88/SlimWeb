using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Formatting;
using SlimWeb.Models;
using SW.Core.Filters;

namespace SlimWeb.Controllers
{
    [AuthorizeFilter]
    public class BasicController : ApiController
    {
    }
}
