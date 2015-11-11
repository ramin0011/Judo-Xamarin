using System;
using JudoDotNetXamarin;

namespace JudoDotNetXamariniOSSDK
{
	public interface IApplePayMethods
	{
		void ApplePayment (ApplePayViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure,ApplePaymentType type);
	}
}

