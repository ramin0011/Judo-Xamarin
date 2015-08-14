using System;

namespace JudoDotNetXamariniOSSDK
{
	public class PaymentReceiptViewModel
	{
		public string ReceiptId { get; set; }
		public DateTime CreatedAt { get; set; }
		public Decimal OriginalAmount { get; set; }
		public string Currency { get; set; }
		public string Message { get; set;}
	}
}

