using System;

namespace JudoDotNetXamariniOSSDK
{
	public class PaymentViewModel
	{
		public CardViewModel Card { get; set; }
		public string Amount { get; set; }
		int JudoID	{ get; set;}
		string PaymentReference{ get; set; }
		string ConsumerReference{ get; set; }

	}


}

