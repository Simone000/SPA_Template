using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPA_TemplateHelpers
{
    //config.MessageHandlers.Replace(new CustomResponseHandler());
    public class CustomResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //custom request handling for custom Header parameters
            /*if (request != null && request.RequestUri.AbsolutePath != @"/api/Account/Login")
            {
                //Se presente significa che arriva dall'app => voglio che venga reridetto alla pagina di login
                if (request.Headers.Where(p => p.Key == "x-force-auth").Any())
                {
                    if (!request.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                    {
                        HttpResponseMessage unauthResp = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                        unauthResp.RequestMessage = request;
                        return await Task.FromResult(unauthResp);
                    }
                }
            }*/


            var response = await base.SendAsync(request, cancellationToken);

            if (response == null || response.Content == null)
                return response;

            //In case of File I just log the disposition instead of the entire content
            if (response.Content.Headers.ContentDisposition != null)
            {
                Trace.TraceInformation("Response: {0}, ContentDisposition: {1}", response.StatusCode.ToString(), response.Content.Headers.ContentDisposition.ToString());
                return response;
            }


            var responseToLog = await response.Content.ReadAsStringAsync();
            Trace.TraceInformation("Response: {0} {1}", response.StatusCode.ToString(), responseToLog);
            return response;
        }
    }
}
