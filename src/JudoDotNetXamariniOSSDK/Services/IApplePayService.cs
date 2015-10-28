using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;
using UIKit;
using PassKit;
using Foundation;

namespace JudoDotNetXamariniOSSDK
{
	public interface IApplePayService
	{
		void MakeApplePayment (ApplePayViewModel payment,ApplePayCallBack appleCallback,UINavigationController controller, ApplePaymentType type);
		void ApplePreAuthoriseCard (ApplePayViewModel payment,ApplePayCallBack appleCallback,UINavigationController controller);
		Task<IResult<ITransactionResult>> HandlePKPayment (PKPayment payment,NSDecimalNumber amount, ApplePaymentType type);
	}
}

