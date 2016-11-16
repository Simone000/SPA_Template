using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPA_Template.Controllers.Samples
{
    public class ExceptionsController : Controller
    {
        public ActionResult Index()
        {
            throw new Exception("Test Exception MVC Message");
            return View();
        }
    }
}