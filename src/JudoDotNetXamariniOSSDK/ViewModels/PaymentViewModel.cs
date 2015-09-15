﻿using System;
using System.Collections.Generic;

namespace JudoDotNetXamariniOSSDK
{
	public class PaymentViewModel
	{
		public CardViewModel Card { get; set; }
		public string Amount { get; set; }
		int JudoId { get; set;}
		string PaymentReference{ get; set; }
		string ConsumerReference{ get; set; }
        public IDictionary<string, string> YourPaymentMetaData { get; set; }
	}
}

