using System;
using System.Collections.Generic;
using System.Linq;
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
            
            //global exception logging
            config.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

            //handler exceptions
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            //handler answers
            config.MessageHandlers.Add(new CustomResponseHandler());

            //todo: is it necessary?
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //custom datetime formatting
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateTimeConverter());

            //only if directly serving objects from entity framework
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //Enable CORS for https (jQuery needs this)
            //requires package Microsoft.AspNet.WebApi.Cors
            //https://stackoverflow.com/questions/26649361/options-405-method-not-allowed-web-api-2
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}