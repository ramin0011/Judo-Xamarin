using System;
using JudoDotNetXamarin.Delegates;
using JudoDotNetXamarin.ViewModels;


namespace JudoDotNetXamarin
{
	public interface IJudoSDKManager
	{
		void Payment(PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void PreAuth (PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure);
		void TokenPayment (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void TokenPreAuth (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void RegisterCard (PaymentViewModel registerCard, JudoSuccessCallback success, JudoFailureCallback failure);

	}
}

