﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

//using System.Web.Security;
//using System.Web.SessionState;

namespace SPA_TemplateHelpers
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
            
            //Create basic roles
            using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();
            if (error != null)
            {
                string excToLog = error.GetBaseException().ToString();

                var httpExc = error as System.Web.HttpException;
                if (httpExc != null && httpExc.GetHttpCode() == 404)
                {
                    Trace.TraceWarning("Global/Application_Error, Page Not Found: {0}", Request.RawUrl);
                    return;
                }

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    Trace.TraceError("Global/Application_Error"
                                     + Environment.NewLine
                                     + GetLogRequest(HttpContext.Current.Request)
                                     + Environment.NewLine
                                     + "Exception:"
                                     + Environment.NewLine
                                     + excToLog);
                }
                else
                {
                    Trace.TraceError("Application_Error, Eccezione: {0}", excToLog);
                }

                //to be called only if the Exception is actually handled here (ignored)
                //if called it will also stop customError in the web.config
                //Server.ClearError();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
            {
                Trace.TraceWarning("Global Application_AuthenticateRequest, HttpContext.Current == null");
                return;
            }

            if (HttpContext.Current.Request.FilePath == "/Glimpse.axd")
                return;

            Trace.TraceInformation(GetLogRequest(HttpContext.Current.Request));
        }

        private string GetLogRequest(HttpRequest Request)
        {
            string tipoRichiesta = Request.HttpMethod;
            string url = Request.RawUrl;

            string parametri = string.Join("; ", Request.Form.AllKeys.Select(p => p + ": " + Request.Form[p]));

            string username = "NotAuthenticated";
            if (User != null && User.Identity.IsAuthenticated)
                username = User.Identity.Name;
            string ip = GetUserIP(Request);

            string logRequest = string.Format("HTTP {0} {1}"
                                              + Environment.NewLine
                                              + "Form Keys: {2}"
                                              + Environment.NewLine
                                              + "User: {3}, IP: {4}",
                                              tipoRichiesta, url, parametri, username, ip);
            return logRequest;
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