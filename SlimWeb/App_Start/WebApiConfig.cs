using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SlimWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/Authenticate/Login/{userName}/{password}",
                defaults: new
                {
                    controller = "Authenticate",
                    action = "Login",
                    userName = RouteParameter.Optional,
                    password = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "CompetitionAddCategory",
                routeTemplate: "api/Competition/AddCategory/{competitionId}/{categoryName}",
                defaults: new
                {
                    controller = "Competition",
                    action = "AddCategory",
                    competitionId = RouteParameter.Optional,
                    categoryName = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "ReadNewsletter",
                routeTemplate: "api/NewsletterJob/ReadNewsletter/{newsletterSendItemGuid}",
                defaults: new { controller = "NewsletterJob", action = "ReadNewsletter", newsletterSendItemGuid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            //var cors = new EnableCorsAttribute("*", "*", "*", "*");
            //config.EnableCors();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
