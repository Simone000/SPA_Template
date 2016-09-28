using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SPA_Template.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SPA_Template
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //public override void Init()
        //{
        //    this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
        //    base.Init();
        //}
        //
        ////Add Session to Web API
        //void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    if (HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/api/"))
        //    {
        //        System.Web.HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        //    }
        //}


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //Create basic roles
            using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();
            if(error != null)
            {
                Trace.TraceError("Application_Error, Eccezione: {0}", error.ToString());
                Server.ClearError();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
            {
                Trace.TraceWarning("Global Application_AuthenticateRequest, HttpContext.Current == null");
                return;
            }

            CustomLogRequest(HttpContext.Current.Request);
        }

        private void CustomLogRequest(HttpRequest Request)
        {
            string tipoRichiesta = Request.HttpMethod;
            string url = Request.RawUrl;

            string parametri = string.Join("; ", Request.Form.AllKeys.Select(p => p + ": " + Request.Form[p]));

            string username = "NotAuthenticated";
            if (User != null && User.Identity.IsAuthenticated)
                username = User.Identity.Name;
            string ip = GetUserIP(Request);

            Trace.TraceInformation("HTTP {0}, Url: {1}"
                                   + Environment.NewLine
                                   + "Form Keys: {2}"
                                   + Environment.NewLine
                                   + "User: {3}, IP: {4}",
                                   tipoRichiesta, url, parametri, username, ip);
        }

        private string GetUserIP(HttpRequest Request)
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
                return ipList.Split(',')[0];

            return Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
