using System;
using JudoPayDotNet;
using JudoDotNetXamarin;

namespace JudoDotNetXamariniOSSDK
{
	public class ServiceFactory
	{

		public ServiceFactory()
		{

		}

		public	IPaymentService	GetPaymentService()
		{
			var judoApi = JudoPaymentsFactory.Create (JudoPayDotNet.Enums.Environment.Sandbox, AppConfig.ApiToken, AppConfig.ApiSecret);
			return  new PaymentService(judoApi);
		}
	}

}

