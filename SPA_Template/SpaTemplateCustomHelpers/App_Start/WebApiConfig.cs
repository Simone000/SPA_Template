using SPA_TemplateHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace SpaTemplateCustomHelpers
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Servizi e configurazione dell'API Web

            // Route dell'API Web
            config.MapHttpAttributeRoutes();

            //global exception logging
            config.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

            //handler exceptions
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            //Log request and response
            config.MessageHandlers.Add(new CustomRequestAndResponseHandler());

            //custom datetime formatting
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateTimeConverter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
