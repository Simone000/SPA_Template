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
        [Route("TestExc1")]
        public IHttpActionResult TestExc1()
        {
            throw new CustomValidationException("CustomExc Message");
        }
    }
}
