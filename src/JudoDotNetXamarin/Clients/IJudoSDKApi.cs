using System.Collections.Generic;
using System.Threading.Tasks;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JudoDotNetXamariniOSSDK")]
[assembly: InternalsVisibleTo("JudoDotNetXamarinSDK")]
namespace JudoDotNetXamarin
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