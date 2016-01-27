﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace SPA_Template
{
    class CustomExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null && context.Request != null)
            {
                string excToString = context.Exception.ToString();
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