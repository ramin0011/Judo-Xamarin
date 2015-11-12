using System;
using JudoDotNetXamarin;
using JudoDotNetXamarin.Delegates;
using JudoDotNetXamarin.Enum;
using JudoDotNetXamarin.Models;
using JudoDotNetXamariniOSSDK.Services;
using JudoDotNetXamariniOSSDK.Utils;
using JudoDotNetXamariniOSSDK.ViewModels;
using JudoPayDotNet.Models;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Clients
{
	internal class ApplePayMethods : IApplePayMethods
	{
		private readonly IApplePayService _paymentService;

		public ApplePayMethods (IApplePayService paymentService)
		{
			_paymentService = paymentService;
		}

		public void ApplePayment (ApplePayViewModel viewModel, JudoSuccessCallback success, JudoFailureCallback failure, ApplePaymentType type)
		{
			if (!ClientDetailsProvider.ApplePayAvailable) {
				failure (new JudoError { ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel {
						ErrorMessage = "Apple Pay is not enabled on device, application entitlement or setup by user.",
						ErrorType = JudoApiError.General_Error,
						ModelErrors = null
					}
				});
			}
			try {
				var vc = GetCurrentViewController ();

				if (JudoSDKManager.UIMode && vc == null) {
					var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
					failure (error);
				} else {
					_paymentService.MakeApplePayment (viewModel, success, failure, vc as UINavigationController, type);
				}
			
			} catch (Exception ex) {
				HandleFailure (failure, ex);
			}
		}

		private void HandleFailure (JudoFailureCallback failure, Exception ex)
		{
			if (failure != null) {
				var judoError = new JudoError {
					Exception = ex
				};
				failure (judoError);
			}
		}


		UIViewController GetCurrentViewController ()
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			var vc = window.RootViewController;
			while (vc.PresentedViewController != null) {
				vc = vc.PresentedViewController;
			}
			return vc;
		}
	}
}

