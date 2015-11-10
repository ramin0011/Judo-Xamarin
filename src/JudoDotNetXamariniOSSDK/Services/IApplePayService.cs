using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;
using UIKit;
using PassKit;
using Foundation;
using JudoDotNetXamarin;

namespace JudoDotNetXamariniOSSDK
{
	internal interface IApplePayService
	{
		void MakeApplePayment (ApplePayViewModel payment,JudoSuccessCallback success, JudoFailureCallback failure,UINavigationController controller, ApplePaymentType type);
		Task<IResult<ITransactionResult>> HandlePKPayment (PKPayment payment,string customerRef,NSDecimalNumber amount, ApplePaymentType type,JudoFailureCallback failure);
	}
}

