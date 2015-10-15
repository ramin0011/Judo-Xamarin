using System;
using JudoPayDotNet.Models;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public abstract class ApplePayMethods
	{
		private readonly IApplePayService _paymentService;

		public ApplePayMethods(IApplePayService paymentService)
		{
			_paymentService = paymentService;
		}
		public void ApplePayment( ApplePayViewModel viewModel, ApplePayCallBack appleCallback, FailureCallback failure,UINavigationController navigationController)
		{
			if (!JudoSDKManager.ApplePayAvailable) {
				failure (new JudoError {ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel{ErrorMessage ="Apple Pay is not enabled on device, application entitlement or setup by user.", ErrorType = JudoApiError.General_Error, ModelErrors = null }});
			}
			try
			{
				_paymentService.MakeApplePayment(viewModel,appleCallback,navigationController);
			}
			catch (Exception ex)
			{
				// Failure
				HandleFailure (failure,ex);
			}
		}

		public void ApplePreAuth(PaymentViewModel payment,  ApplePayCallBack appleCallback, FailureCallback failure)
		{


		}

		object HandResponse ( FailureCallback failure, System.Threading.Tasks.Task reponse)
		{
			throw new NotImplementedException ();
		}

		private void HandleFailure (FailureCallback failure,Exception ex)
		{
			if (failure != null) {
				var judoError = new JudoError {
					Exception = ex
				};
				failure (judoError);
			}
		}
	}
}

