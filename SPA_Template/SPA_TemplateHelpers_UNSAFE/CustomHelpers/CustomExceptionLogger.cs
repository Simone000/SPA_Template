using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;

namespace SPA_TemplateHelpers
{
    //http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
    class CustomExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null && context.Request != null)
            {
                string excToString = context.Exception.ToString();

                var entityValidationExc = context.Exception as System.Data.Entity.Validation.DbEntityValidationException;
                if (entityValidationExc != null)
                    excToString += Environment.NewLine
                                + string.Join(Environment.NewLine, entityValidationExc.EntityValidationErrors.SelectMany(p => p.ValidationErrors.Select(q => q.PropertyName + ": " + q.ErrorMessage)));

                var usedController = ((System.Web.Http.ApiController)context.ExceptionContext.ControllerContext.Controller);

                //similar to Global.asax/CustomLogRequest
                //todo: missing user and IP, catch Exceptions and null reference
                Trace.TraceError("CustomExceptionLogger"
                                 + Environment.NewLine
                                 + "HTTP {0} {1}"
                                 + Environment.NewLine
                                 + "Form Keys: {2}"
                                 + Environment.NewLine
                                 + "Exception:"
                                 + Environment.NewLine
                                 + excToString
                                 ,
                                 context.Request.Method.Method, context.Request.RequestUri,
                                 string.Join(Environment.NewLine,
                                            usedController.ActionContext.ActionArguments.Select(p => p.Key + ": " + JsonConvert.SerializeObject(p.Value))));
            }
        }
    }
}
