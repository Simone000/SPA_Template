using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace SPA_TemplateHelpers
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
            
            //abilito log globale errori
            config.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

            //handler eccezioni
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            //handler risposte
            config.MessageHandlers.Add(new CustomResponseHandler());

            //needed?
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //custom datetime formatting
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateTimeConverter());

            //only if directly returning data from entity framework
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}