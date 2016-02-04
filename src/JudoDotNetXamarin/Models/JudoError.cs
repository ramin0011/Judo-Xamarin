using System;
using JudoPayDotNet.Errors;
using System.Collections.Generic;

namespace JudoDotNetXamarin
{
    public class JudoError : Exception
    {
        public Exception Exception { get; set; }

        public ModelError ApiError { get; set; }

        public JudoError (Exception exception, ModelError apiError)
        {
            Exception = exception;
            ApiError = apiError;
        }

        public JudoError ()
        {
            ApiError = new ModelError () {
                ModelErrors = new List<FieldError> ()
            };
            Exception = new Exception ();
        }
    }
}