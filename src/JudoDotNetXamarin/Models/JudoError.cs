using System;
using JudoPayDotNet.Errors;

namespace JudoDotNetXamarin
{
    public class JudoError : Exception
    {
        public Exception Exception { get; set; }

        public JudoApiErrorModel ApiError { get; set; }
    }
}