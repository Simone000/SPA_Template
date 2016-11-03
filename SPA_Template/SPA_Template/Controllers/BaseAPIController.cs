using SPA_TemplateHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SPA_Template.Controllers
{
    public class BaseAPIController : ApiController
    {
        protected readonly ApplicationDbContext db = new ApplicationDbContext();

        protected string BaseUrl
        {
            get
            {
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
