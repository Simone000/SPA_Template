using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SPA_TemplateHelpers
{
    /// <summary>
    /// config.MessageHandlers.Add(new CustomRequestAndResponseHandler());
    /// in WebApiConfig.Register(HttpConfiguration config)
    /// </summary>
    public class CustomRequestAndResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
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

            //before base.SendAsync to read Content
            string requestBody = string.Empty;
            if (!request.Content.IsMimeMultipartContent()) //don't log files
                requestBody = await request.Content.ReadAsStringAsync();
            else
                requestBody = request.Content.Headers.ContentDisposition?.ToString();

            var response = await base.SendAsync(request, cancellationToken);

            //log request body (after base.SendAsync to get the info on identity)
            string requestToLog = GetLogRequest(request, requestBody);
            Trace.TraceInformation(requestToLog);

            if (response == null || response.Content == null)
                return response;

            //In case of File I just log the disposition instead of the entire content
            if (response.Content.Headers.ContentDisposition != null)
            {
                Trace.TraceInformation("Response: {0}, ContentDisposition: {1}",
                    response.StatusCode.ToString(),
                    response.Content.Headers.ContentDisposition.ToString());
                return response;
            }

            var responseToLog = await response.Content.ReadAsStringAsync();
            Trace.TraceInformation("Response: {0} {1}",
                response.StatusCode.ToString(), responseToLog);
            return response;
        }

        private string GetLogRequest(HttpRequestMessage Request, string Body)
        {
            string tipoRichiesta = Request.Method.Method;
            string url = Request.RequestUri.ToString();

            string parametri = Body;

            string username = "NotAuthenticated";
            var principal = Request.GetRequestContext().Principal;
            if (principal != null && principal.Identity.IsAuthenticated)
                username = principal.Identity.Name;

            string ip = GetClientIp(Request);

            string logRequest = string.Format("HTTP {0} {1}"
                + Environment.NewLine
                + "Form Keys: {2}"
                + Environment.NewLine
                + "User: {3}, IP: {4}",
                tipoRichiesta, url, parametri, username, ip);
            return logRequest;
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"])
                    .Request.UserHostAddress;
            }

            //using System.ServiceModel.Channels;
            /*if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request
                    .Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }*/

            return string.Empty;
        }
    }
}
