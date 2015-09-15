using System;
using System.Collections.Generic;

namespace JudoDotNetXamariniOSSDK
{
	public class TokenPaymentViewModel
	{
		public string Token { get; set; }
		public string CV2 { get; set; }
		public string ConsumerToken { get; set; }
		public string Amount { get; set; }
		int JudoId	{ get; set;}
		string PaymentReference{ get; set; }
		string ConsumerReference{ get; set; }
        public IDictionary<string, string> YourPaymentMetaData { get; set; }
	}
}

