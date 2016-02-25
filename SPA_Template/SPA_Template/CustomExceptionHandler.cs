using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace SPA_Template
{
    //http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
    public class CustomExceptionHandler : IExceptionHandler
    {
        public virtual Task HandleAsync(ExceptionHandlerContext context,
                                    CancellationToken cancellationToken)
        {
            //Should handle
            if(!context.ExceptionContext.CatchBlock.IsTopLevel)
                return Task.FromResult(0);

            //handle CustomValidationException
            var customValidationExc = context.Exception as CustomValidationException;
            if(customValidationExc != null)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                result.Content = new StringContent(customValidationExc.Message);

                //todo: differenza fra new ResponseMessageResult(result) ed implementare IHttpActionResult?
                context.Result = new ResponseMessageResult(result);
            }

            var entityValidationExc = context.Exception as System.Data.Entity.Validation.DbEntityValidationException;
            if(entityValidationExc != null)
            {
                //todo: aggiungere errori al modelstate (possibilmente assegnandoli anche alle key giuste)
                var errors = entityValidationExc.EntityValidationErrors.SelectMany(p => p.ValidationErrors).Select(p => p.ErrorMessage);
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                result.Content = new StringContent(string.Join("; ", errors));
                context.Result = new ResponseMessageResult(result);
            }

            return Task.FromResult(0);
        }
    }
}
