using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPA_Template
{
    //todo: va in viewmodels o cmq sharedutils
    [Serializable]
    public class CustomValidationException : Exception
    {
        public CustomValidationException() { }
        public CustomValidationException(string message) : base(message) { }
        public CustomValidationException(string message, Exception inner) : base(message, inner) { }

        protected CustomValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}