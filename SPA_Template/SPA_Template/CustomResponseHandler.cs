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
    public class CustomResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);


            //todo: in caso di files cosa fa??
            if (response != null && response.Content != null)
            {
                var responseToLog = await response.Content.ReadAsStringAsync();
                Trace.TraceInformation("Response: {0} {1}", response.StatusCode.ToString(), responseToLog);
            }

            return response;
        }
    }
}
