using System;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public abstract class ApplePayMethods
	{
		private readonly IApplePayService _paymentService;

		public ApplePayMethods(IApplePayService paymentService)
		{
			_paymentService = paymentService;
		}
		public void ApplePayment( ApplePayViewModel viewModel, SuccessCallback success, FailureCallback failure)
		{
			if (!JudoSDKManager.ApplePayAvailable) {
				failure (new JudoError {ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel{ErrorMessage ="Apple Pay is not enabled on device, application entitlement or setup by user.", ErrorType = JudoApiError.General_Error, ModelErrors = null }});
			}
			try
			{
				_paymentService.MakeApplePayment(viewModel).ContinueWith(reponse => HandResponse(success, failure, reponse));
			}
			catch (Exception ex)
			{
				// Failure
				HandleFailure (failure,ex);
			}
		}

		public void ApplePreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure)
		{


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

