using System;
using JudoPayDotNet.Errors;

namespace JudoDotNetXamarin
{
    public class JudoError
    {
        public Exception Exception { get; set; }
        public JudoApiErrorModel ApiError { get; set; } 
    }
}