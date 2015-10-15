using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public interface IApplePayService
	{
		Task<IResult<ITransactionResult>> MakeApplePayment (ApplePayViewModel payment,UINavigationController controller);
		Task<IResult<ITransactionResult>> ApplePreAuthoriseCard (ApplePayViewModel payment,UINavigationController controller);
	}
}

