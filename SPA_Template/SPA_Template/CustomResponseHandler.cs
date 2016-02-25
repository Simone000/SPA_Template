using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPA_Template
{
    //config.MessageHandlers.Replace(new CustomResponseHandler());
    public class CustomResponseHandler : DelegatingHandler
    {
        //todo: parametri da web config per log request e response

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request
            if(request != null)
            {
                //todo: copiare da global.asax

                //Trace.TraceInformation("HTTP {0}, Url: {1}, parametri: {2}, Utente: {3}, IP: {4}", tipoRichiesta, url, parametri, username, ip);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response != null && response.Content != null)
            {
                var responseToLog = await response.Content.ReadAsStringAsync();
                Trace.TraceInformation("Response: {0} {1}", response.StatusCode.ToString(), responseToLog);
            }

            return response;
        }
    }
}
