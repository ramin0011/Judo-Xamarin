using System;

namespace JudoDotNetXamariniOSSDK
{
	public class ServiceFactory
	{
		public	IPaymentService	GetPaymentService()
		{
			return  new PaymentService();
		}
	}

}

