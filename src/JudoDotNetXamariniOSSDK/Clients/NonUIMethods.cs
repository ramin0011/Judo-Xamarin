using System;
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
            _paymentService.MakePayment(payment).ContinueWith(reponse =>
            {
                var result = reponse.Result;
                if (result != null && !result.HasError && result.Response.Result != "Declined")
                {
                    try
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
                        PaymentReceiptViewModel receipt = new PaymentReceiptViewModel()
                        {
                            // need to fix it
                            CreatedAt = paymentreceipt.CreatedAt.DateTime,
                            Currency = paymentreceipt.Currency,
                            OriginalAmount = paymentreceipt.Amount,
                            ReceiptId = paymentreceipt.ReceiptId,
                            Message = "Payment Success"
                        };
                        JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
                        JudoConfiguration.Instance.TokenCardType = payment.Card.CardType;
                        JudoConfiguration.Instance.ConsumerToken = paymentreceipt.Consumer.ConsumerToken;
                        JudoConfiguration.Instance.LastFour = payment.Card.CardNumber.Substring(payment.Card.CardNumber.Length - Math.Min(4, payment.Card.CardNumber.Length));
                        success(receipt);
                    }
                    catch (Exception ex)
                    {
                        var judoError = new JudoError { Exception = ex };
                        failure(judoError);
                    }

                }
                else
                {
                    // Failure
                    var judoError = new JudoError { ApiError = result != null ? result.Error : null};
                    failure(judoError);
                }
            });
        }


        public void PreAuth(PreAuthorisationViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            _paymentService.PreAuthoriseCard(preAuthorisation).ContinueWith(reponse =>
            {
                var result = reponse.Result;
                if (result != null && !result.HasError && result.Response.Result != "Declined")
                {
                    try
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
                        PaymentReceiptViewModel receipt = new PaymentReceiptViewModel()
                        {
                            // need to fix it
                            CreatedAt = paymentreceipt.CreatedAt.DateTime,
                            Currency = paymentreceipt.Currency,
                            OriginalAmount = paymentreceipt.Amount,
                            ReceiptId = paymentreceipt.ReceiptId,
                            Message = "Payment Success"
                        };
                        JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
                        JudoConfiguration.Instance.TokenCardType = preAuthorisation.Card.CardType;
                        JudoConfiguration.Instance.ConsumerToken = paymentreceipt.Consumer.ConsumerToken;
                        JudoConfiguration.Instance.LastFour = preAuthorisation.Card.CardNumber.Substring(preAuthorisation.Card.CardNumber.Length - Math.Min(4, preAuthorisation.Card.CardNumber.Length));
                        success(receipt);
                    }
                    catch (Exception ex)
                    {
                        var judoError = new JudoError { Exception = ex };
                        failure(judoError);
                    }

                }
                else
                {
                    // Failure
                    var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                    failure(judoError);
                }
            });
        }

        public void TokenPayment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            throw new System.NotImplementedException();
        }

        public void TokenPreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            throw new System.NotImplementedException();
        }
    }
}