using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public interface IApplePayService
	{
		void MakeApplePayment (ApplePayViewModel payment,ApplePayCallBack appleCallback,UINavigationController controller);
		void ApplePreAuthoriseCard (ApplePayViewModel payment,ApplePayCallBack appleCallback,UINavigationController controller);
	}
}

