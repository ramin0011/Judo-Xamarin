using System;
using JudoPayDotNet;
using JudoDotNetXamarin;

namespace JudoDotNetXamariniOSSDK
{
	public class ServiceFactory
	{

		public	IPaymentService	GetPaymentService()
		{
            var judoApi = JudoPaymentsFactory.Create(JudoConfiguration.Instance.Environment, JudoConfiguration.Instance.ApiToken, JudoConfiguration.Instance.ApiSecret);
			return  new PaymentService(judoApi);
		}

	}

}

