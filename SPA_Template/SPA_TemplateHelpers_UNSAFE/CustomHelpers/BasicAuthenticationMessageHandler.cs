using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SPA_TemplateHelpers
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        #region Settings
        private bool WsRequireAuth
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["WsRequireAuth"]);
            }
        }
        private string WsUsername
        {
            get { return ConfigurationManager.AppSettings["WsUsername"]; }
        }
        private string WsPassword
        {
            get { return ConfigurationManager.AppSettings["WsPassword"]; }
        }
        private string WsEmail
        {
            get { return ConfigurationManager.AppSettings["WsEmail"]; }
        }
        private string WsFirstname
        {
            get { return ConfigurationManager.AppSettings["WsFirstname"]; }
        }
        private string WsLastname
        {
            get { return ConfigurationManager.AppSettings["WsLastname"]; }
        } 
        #endregion

        private class User
        {
            public virtual Guid UserId { get; set; }
            public virtual string Firstname { get; set; }
            public virtual string Lastname { get; set; }
            public virtual string Username { get; set; }
            public virtual string Email { get; set; }
        }

        private const string BasicScheme = "Basic";
        private const string ChallengeAuthenticationHeaderName = "WWW-Authenticate";
        private const char AuthorizationHeaderSeparator = ':';

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (WsRequireAuth)
            {
                var authHeader = request.Headers.Authorization;
                if (authHeader == null)
                {
                    return CreateUnauthorizedResponse();
                }

                if (authHeader.Scheme != BasicScheme)
                {
                    return CreateUnauthorizedResponse();
                }

                var encodedCredentials = authHeader.Parameter;
                var credentialBytes = Convert.FromBase64String(encodedCredentials);
                var credentials = Encoding.ASCII.GetString(credentialBytes);
                var credentialParts = credentials.Split(AuthorizationHeaderSeparator);

                if (credentialParts.Length != 2)
                {
                    return CreateUnauthorizedResponse();
                }

                var username = credentialParts[0].Trim();
                var password = credentialParts[1].Trim();
                if (username != WsUsername
                    || password != WsPassword)
                {
                    return CreateUnauthorizedResponse();
                }

                SetPrincipal(username);
            }
            else
            {
                SetPrincipal(WsUsername);
            }

            return base.SendAsync(request, cancellationToken);
        }

        private void SetPrincipal(string username)
        {
            User modelUser = new User()
            {
                Username = username,
                Email = WsEmail,
                Firstname = WsFirstname,
                Lastname = WsLastname,
                UserId = Guid.NewGuid()
            };

            var identity = CreateIdentity(username, modelUser);

            string[] roles = new string[] { };
            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private GenericIdentity CreateIdentity(string username, User modelUser)
        {
            var identity = new GenericIdentity(username, BasicScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, modelUser.UserId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, modelUser.Firstname));
            identity.AddClaim(new Claim(ClaimTypes.Surname, modelUser.Lastname));
            identity.AddClaim(new Claim(ClaimTypes.Email, modelUser.Email));
            return identity;
        }

        private Task<HttpResponseMessage> CreateUnauthorizedResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.Add(ChallengeAuthenticationHeaderName, BasicScheme);

            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return taskCompletionSource.Task;
        }
    }
}