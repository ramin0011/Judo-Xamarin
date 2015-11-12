using System.Runtime.CompilerServices;
using JudoDotNetXamarin.Delegates;
using JudoDotNetXamarin.ViewModels;

[assembly: InternalsVisibleTo("JudoDotNetXamariniOSSDK")]
[assembly: InternalsVisibleTo("JudoDotNetXamarinAndroidSDK")]
namespace JudoDotNetXamarin.Clients
{
	
    internal interface IJudoSDKApi 
    {
		
		void Payment(PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void PreAuth(PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure);
		void TokenPayment(TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void TokenPreAuth(TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void RegisterCard(PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);


    }
}