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
            try
            {
                _paymentService.MakePayment(payment).ContinueWith(reponse =>
                {
                    var result = reponse.Result;
                    if (result != null && !result.HasError && result.Response.Result != "Declined")
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
                        
                        JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
                        JudoConfiguration.Instance.TokenCardType = payment.Card.CardType;
                        JudoConfiguration.Instance.ConsumerToken = paymentreceipt.Consumer.ConsumerToken;
                        JudoConfiguration.Instance.LastFour = payment.Card.CardNumber.Substring(payment.Card.CardNumber.Length - Math.Min(4, payment.Card.CardNumber.Length));

                        if (success != null) success(paymentreceipt);
                    }
                    else
                    {
                        // Failure
                        if (failure != null)
                        {
                            var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                            failure(judoError);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Failure
                if (failure != null)
                {
                    var judoError = new JudoError { Exception = ex };
                    failure(judoError);
                }
            }
        }


        public void PreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.PreAuthoriseCard(payment).ContinueWith(reponse =>
                {
                    var result = reponse.Result;
                    if (result != null && !result.HasError && result.Response.Result != "Declined")
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;

                        JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
                        JudoConfiguration.Instance.TokenCardType = payment.Card.CardType;
                        JudoConfiguration.Instance.ConsumerToken = paymentreceipt.Consumer.ConsumerToken;
                        JudoConfiguration.Instance.LastFour = payment.Card.CardNumber.Substring(payment.Card.CardNumber.Length - Math.Min(4, payment.Card.CardNumber.Length));

                        if (success != null) success(paymentreceipt);
                    }
                    else
                    {
                        // Failure
                        if (failure != null)
                        {
                            var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                            failure(judoError);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Failure
                if (failure != null)
                {
                    var judoError = new JudoError { Exception = ex};
                    failure(judoError);
                }
            }
        }

        public void TokenPayment(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.MakeTokenPayment(payment).ContinueWith(reponse =>
                {
                    var result = reponse.Result;
                    if (result != null && !result.HasError && result.Response.Result != "Declined")
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;

                        if (success != null) success(paymentreceipt);
                    }
                    else
                    {
                        // Failure
                        if (failure != null)
                        {
                            var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                            failure(judoError);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Failure
                if (failure != null)
                {
                    var judoError = new JudoError { Exception = ex};
                    failure(judoError);
                }
            }
        }

        public void TokenPreAuth(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.MakeTokenPreAuthorisation(payment).ContinueWith(reponse =>
                {
                    var result = reponse.Result;
                    if (result != null && !result.HasError && result.Response.Result != "Declined")
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;

                        if (success != null) success(paymentreceipt);
                    }
                    else
                    {
                        // Failure
                        if (failure != null)
                        {
                            var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                            failure(judoError);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Failure
                if (failure != null)
                {
                    var judoError = new JudoError { Exception = ex };
                    failure(judoError);
                }
            }
        }

        public void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            try
            {
                _paymentService.PreAuthoriseCard(payment).ContinueWith(reponse =>
                {
                    var result = reponse.Result;
                    if (result != null && !result.HasError && result.Response.Result != "Declined")
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;

                        JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
                        JudoConfiguration.Instance.TokenCardType = payment.Card.CardType;
                        JudoConfiguration.Instance.ConsumerToken = paymentreceipt.Consumer.ConsumerToken;
                        JudoConfiguration.Instance.LastFour = payment.Card.CardNumber.Substring(payment.Card.CardNumber.Length - Math.Min(4, payment.Card.CardNumber.Length));

                        if (success != null) success(paymentreceipt);
                    }
                    else
                    {
                        // Failure
                        if (failure != null)
                        {
                            var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                            failure(judoError);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Failure
                if (failure != null)
                {
                    var judoError = new JudoError { Exception = ex };
                    failure(judoError);
                }
            }
        }
    }
}