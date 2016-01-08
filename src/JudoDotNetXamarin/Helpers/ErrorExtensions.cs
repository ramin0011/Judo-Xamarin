﻿using System;
using JudoPayDotNet.Errors;
using System.Collections.Generic;

namespace JudoDotNetXamarin
{
    public static class ErrorExtensions
    {
        public static void FlattenToJudoFailure (this ModelError error, JudoFailureCallback failureCallback)
        {
            if (failureCallback != null) {

////
////                List<FieldError> models = new List<FieldError> (); 
////                foreach (FieldError em in error.ModelErrors) {
////                    models.Add (new FieldError (){ Message = em.Message,FieldName = em.FieldName, });
////                }
//                var judoError = new JudoError () {
//                    Exception = error.Message,
//                    ApiError = error
//                };
//
//                failureCallback (judoError);

            }
        }

        public static JudoError FlattenToJudoError (this AggregateException errors)
        {
          
            List<FieldError> models = new List<FieldError> (); 
            foreach (Exception em in errors.InnerExceptions) {
                models.Add (new FieldError (){ Message = em.Message, });
            }
            var judoError = new JudoError () {
                Exception = errors.InnerException,
                ApiError = new ModelError (){ ModelErrors = models }
            };

            return judoError;

        }
    }
}
