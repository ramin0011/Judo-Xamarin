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
			var judoApi = JudoPaymentsFactory.Create (JudoPayDotNet.Enums.Environment.Sandbox, JudoConfiguration.Instance.ApiToken, JudoConfiguration.Instance.ApiSecret);
			return  new PaymentService(judoApi);
		}

		public ITokenService GetTokenService ()
		{
			var judoApi = JudoPaymentsFactory.Create (JudoPayDotNet.Enums.Environment.Sandbox, JudoConfiguration.Instance.ApiToken, JudoConfiguration.Instance.ApiSecret);
			return  new TokenService(judoApi);
		}
	}

}

