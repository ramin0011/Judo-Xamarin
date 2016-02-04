using System;
using JudoPayDotNet.Errors;
using System.Collections.Generic;

namespace JudoDotNetXamarin
{
    public static class ErrorExtensions
    {

        public static JudoError FlattenToJudoError (this AggregateException errors)
        {
          
            List<FieldError> models = new List<FieldError> (); 
            foreach (Exception em in errors.InnerExceptions) {
                models.Add (new FieldError (){ Message = em.Message, Detail = "", Code = 0, FieldName = "" });
            }
            var judoError = new JudoError () {
                Exception = errors.InnerException,
                ApiError = new ModelError () {
                    ModelErrors = models,
                    Message = errors.InnerException.Message,
                    Code = 0,
                    Category = ""
                },
                
            };

            return judoError;

        }
    }
}

