using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SPA_Template.Controllers
{
    public class ExceptionsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult TestExc1()
        {
            throw new CustomValidationException("CustomExc Message");
        }
    }
}
