using SPA_TemplateHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using Microsoft.AspNet.Identity.Owin;

namespace SPA_TemplateHelpers.Controllers
{
    //todo: replace with the working version
    public class ServicesContext : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //dispose
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public class BaseApiController : ApiController
    {
        protected readonly ServicesContext sv = new ServicesContext();

        #region UserManager e SignInManager
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                //requires Microsoft.Owin.Host.SystemWeb
                return _signInManager ?? HttpContext.Current.Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                //requires Microsoft.Owin.Host.SystemWeb
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        protected string BaseUrl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SpaSettings.ForceBaseUrl))
                {
                    return SpaSettings.ForceBaseUrl;
                }

                //Home Page
                return HttpContext.Current.Request.Url.Scheme
                       + @"://"
                       + HttpContext.Current.Request.Url.Authority;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sv.Dispose();

                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
