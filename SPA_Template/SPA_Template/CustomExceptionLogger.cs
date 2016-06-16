using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace SPA_Template
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

                Trace.TraceError("CustomExceptionLogger Log, Request: {0}, Eccezione: {1}", RequestToString(context.Request), excToString);
            }
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}
