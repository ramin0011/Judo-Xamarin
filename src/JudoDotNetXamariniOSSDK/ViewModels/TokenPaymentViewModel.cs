﻿using System;

namespace JudoDotNetXamariniOSSDK
{
	public class TokenPaymentViewModel
	{
		public string Token { get; set; }
		public string CV2 { get; set; }
		public string ConsumerToken { get; set; }
		public string Amount { get; set; }
		int JudoID	{ get; set;}
		string PaymentReference{ get; set; }
		string ConsumerReference{ get; set; }
	}
}

