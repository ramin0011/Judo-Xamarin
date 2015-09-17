using System;
using System.Threading.Tasks;
using JudoPayDotNet.Models;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Clients
{
    internal class NonUIMethods : IJudoSDKApi
    {
        private readonly IPaymentService _paymentService;

        public NonUIMethods(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public void Payment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController = null)
        {
            try
            {
                _paymentService.MakePayment(payment).ContinueWith(reponse => HandResponse(true, success, failure, reponse));
            }
            catch (Exception ex)
            {
                // Failure
				HandleFailure (failure,ex);
            }
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

        public void PreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.PreAuthoriseCard(payment).ContinueWith(reponse => HandResponse(true, success, failure, reponse));
            }
            catch (Exception ex)
            {
                // Failure
				HandleFailure (failure,ex);
            }
        }

        public void TokenPayment(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.MakeTokenPayment(payment).ContinueWith(reponse => HandResponse(false, success, failure, reponse));
            }
            catch (Exception ex)
            {
                // Failure
				HandleFailure (failure,ex);
            }
        }

        public void TokenPreAuth(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.MakeTokenPreAuthorisation(payment).ContinueWith(reponse => HandResponse(false, success, failure, reponse));
            }
            catch (Exception ex)
            {
                // Failure
				HandleFailure (failure,ex);
            }
        }

        public void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.RegisterCard(payment).ContinueWith(reponse => HandResponse(true, success, failure, reponse));
            }
            catch (Exception ex)
            {
                // Failure
				HandleFailure (failure,ex);
            }
        }

        private static void HandResponse(bool savetoken, SuccessCallback success, FailureCallback failure, Task<IResult<ITransactionResult>> reponse)
        {
            var result = reponse.Result;
            if (result != null && !result.HasError && result.Response.Result != "Declined")
            {
                var paymentreceipt = result.Response as PaymentReceiptModel;

                if (savetoken && paymentreceipt != null && paymentreceipt.CardDetails != null)
                {
                    JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
                    JudoConfiguration.Instance.TokenCardType = paymentreceipt.CardDetails.CardType;
                    JudoConfiguration.Instance.ConsumerToken = paymentreceipt.Consumer.ConsumerToken;
                    JudoConfiguration.Instance.LastFour = paymentreceipt.CardDetails.CardLastfour;
                }

                if (success != null)
                {
                    success(paymentreceipt);
                }
                else
                {
                    throw new Exception("SuccessCallback is not set.");
                }
            }
            else
            {
                // Failure
                if (failure != null)
                {
                    var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                    failure(judoError);
                }
                else
                {
                    throw new Exception("FailureCallback is not set.");
                }
            }
        }
    }
}