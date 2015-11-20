using JudoDotNetXamarin;
using JudoDotNetXamariniOSSDK.Services;

namespace JudoDotNetXamariniOSSDK.Factories
{
	internal class ServiceFactory
	{

		public	IPaymentService	GetPaymentService()
		{
            var judoApi = JudoPaymentsFactory.Create(JudoConfiguration.Instance.Environment, JudoConfiguration.Instance.ApiToken, JudoConfiguration.Instance.ApiSecret);
			return  new PaymentService(judoApi);
		}

		public IApplePayService GetApplePaymentService ()
		{
			var judoApi = JudoPaymentsFactory.Create(JudoConfiguration.Instance.Environment, JudoConfiguration.Instance.ApiToken, JudoConfiguration.Instance.ApiSecret);
			return  new ApplePayService(judoApi);
		}
	}

}

