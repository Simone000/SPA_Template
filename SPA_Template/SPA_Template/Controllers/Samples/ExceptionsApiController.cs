using SharedUtilsNoReference.Exceptions;
using SPA_TemplateHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SPA_Template.Controllers
{
    [RoutePrefix("api/Samples/ExceptionsApi")]
    public class ExceptionsApiController : ApiController
    {
        [HttpGet]
        [Route("TestCustomValidationException")]
        public IHttpActionResult TestCustomValidationException()
        {
            throw new CustomValidationException("CustomExc Message");
        }

        [HttpGet]
        [Route("TestException")]
        public IHttpActionResult TestException()
        {
            throw new Exception("Exception Message");
        }
    }
}
