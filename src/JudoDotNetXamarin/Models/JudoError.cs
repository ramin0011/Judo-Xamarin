using System;
using JudoPayDotNet.Errors;

namespace JudoDotNetXamarin.Models
{
    public class JudoError
    {
        public Exception Exception { get; set; }
        public JudoApiErrorModel ApiError { get; set; } 
    }
}