using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SPA_TemplateHelpers_UNSAFE.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            /*

            //abilito log globale errori
            config.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

            //handler eccezioni
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            //handler risposte
            config.MessageHandlers.Add(new CustomResponseHandler());


            //custom datetime formatting
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateTimeConverter());*/

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}