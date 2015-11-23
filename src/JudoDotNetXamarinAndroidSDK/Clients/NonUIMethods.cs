using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;
using System;

namespace JudoDotNetXamarinAndroidSDK
{
    internal class NonUIMethods : JudoAndroidSDKAPI //INonUIMethods
    {
        #region IJudoSDKApi implementation

        #region JudoAndroidSDKAPI implementation

        IPaymentService _paymentService;
        ServiceFactory factory;

        public NonUIMethods ()
        {
            factory = new ServiceFactory ();
            _paymentService = factory.GetPaymentService (); 
        }

        public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context)
        {
            try {
                _paymentService.MakePayment (payment, new ClientService ()).ContinueWith (reponse => HandleResponse (success, failure, reponse));
            } catch (Exception ex) {
                // Failure
                HandleFailure (failure, ex);
            }
        }

        #endregion

        public void Payment (JudoDotNetXamarin.PaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }

        public void PreAuth (JudoDotNetXamarin.PaymentViewModel preAuthorisation, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }

        public void TokenPayment (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }

        public void TokenPreAuth (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }

        public void RegisterCard (JudoDotNetXamarin.PaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
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

        private static void HandleResponse (JudoSuccessCallback success, JudoFailureCallback failure, Task<IResult<ITransactionResult>> reponse)
        {
            var result = reponse.Result;
            if (result != null && !result.HasError && result.Response.Result != "Declined") {
                var secureReceipt = result.Response as PaymentRequiresThreeDSecureModel;
                if (secureReceipt != null) {
                    var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                    failure (new JudoError { ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel {
                            ErrorMessage = "Account requires 3D Secure but non UI Mode does not support this",
                            ErrorType = JudoApiError.General_Error,
                            ModelErrors = null
                        }
                    });
                }

                var paymentReceipt = result.Response as PaymentReceiptModel;

                if (success != null) {
                    success (paymentReceipt);
                } else {
                    throw new Exception ("SuccessCallback is not set.");
                }
            } else {
                // Failure
                if (failure != null) {
                    var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                    var paymentreceipt = result != null ? result.Response as PaymentReceiptModel : null;

                    if (paymentreceipt != null) {
                        // send receipt even we got card declined
                        failure (judoError, paymentreceipt);
                    } else {
                        failure (judoError);
                    }
                } else {
                    throw new Exception ("FailureCallback is not set.");
                }
            }
        }

        #endregion

        //        public Task<IResult<ITransactionResult>> Payment( string judoId, string currency, decimal amount, string paymentReference,
        //            string consumerReference, IDictionary<string, string> metaData,
        //			string cardNumber,string postCode, string startDate, string expiryDate, string cv2)
        //        {
        //            var cardPayment = new CardPaymentModel()
        //            {
        //                JudoId = judoId,
        //                Currency = currency,
        //                Amount = amount,
        //                YourPaymentReference = paymentReference,
        //                YourConsumerReference = consumerReference,
        //                YourPaymentMetaData = metaData,
        //                CardNumber = cardNumber,
        //                CardAddress = new CardAddressModel { PostCode = postCode },
        //                StartDate = startDate,
        //                ExpiryDate = expiryDate,
        //                CV2 = cv2,
        //                ClientDetails = JudoSDKManager.GetClientDetails(Application.Context),
        //				UserAgent = JudoSDKManager.GetSDKVersion()
        //            };
        //
        //			return null; //JudoSDKManager.JudoClient.Payments.Create(cardPayment);
        //        }
        //
        //        public Task<IResult<ITransactionResult>> TokenPayment(Context context, string judoId, string currency, decimal amount, string paymentReference,
        //            string consumerToken, string consumerReference, IDictionary<string, string> metaData,
        //            string cardToken, string cv2)
        //        {
        //            TokenPaymentModel payment = new TokenPaymentModel()
        //            {
        //                JudoId = judoId,
        //                Currency = currency,
        //                Amount = amount,
        //                YourPaymentReference = paymentReference,
        //                ConsumerToken = consumerToken,
        //                YourConsumerReference = consumerReference,
        //                YourPaymentMetaData = metaData,
        //                CardToken = cardToken,
        //                CV2 = cv2,
        //                ClientDetails = JudoSDKManager.GetClientDetails(context),
        //				UserAgent = JudoSDKManager.GetSDKVersion()
        //            };
        //
        //			return null;// JudoSDKManager.JudoClient.Payments.Create(payment);
        //        }
        //
        //        public Task<IResult<ITransactionResult>> PreAuth(Context context, string judoId, string currency, decimal amount, string paymentReference,
        //            string consumerReference, IDictionary<string, string> metaData, string cardNumber,
        //            string postCode, string startDate, string expiryDate, string cv2)
        //        {
        //            var cardPayment = new CardPaymentModel()
        //            {
        //                JudoId = judoId,
        //                Currency = currency,
        //                Amount = amount,
        //                YourPaymentReference = paymentReference,
        //                YourConsumerReference = consumerReference,
        //                YourPaymentMetaData = metaData,
        //                CardNumber = cardNumber,
        //                CardAddress = new CardAddressModel { PostCode = postCode },
        //                StartDate = startDate,
        //                ExpiryDate = expiryDate,
        //                CV2 = cv2,
        //                ClientDetails = JudoSDKManager.GetClientDetails(context),
        //				UserAgent = JudoSDKManager.GetSDKVersion()
        //            };
        //
        //			return null;// JudoSDKManager.JudoClient.PreAuths.Create(cardPayment);
        //        }
        //
        //        public Task<IResult<ITransactionResult>> TokenPreAuth(Context context, string judoId, string currency, decimal amount, string paymentReference,
        //            string consumerToken, string consumerReference, IDictionary<string, string> metaData,
        //            string cardToken, string cv2)
        //        {
        //            TokenPaymentModel payment = new TokenPaymentModel()
        //            {
        //                JudoId = judoId,
        //                Currency = currency,
        //                Amount = amount,
        //                YourPaymentReference = paymentReference,
        //                ConsumerToken = consumerToken,
        //                YourConsumerReference = consumerReference,
        //                YourPaymentMetaData = metaData,
        //                CardToken = cardToken,
        //                CV2 = cv2,
        //                ClientDetails = JudoSDKManager.GetClientDetails(context),
        //				UserAgent = JudoSDKManager.GetSDKVersion()
        //            };
        //
        //			return null;//  JudoSDKManager.JudoClient.PreAuths.Create(payment);
        //        }
        //
        //        public Task<IResult<ITransactionResult>> RegisterCard(string cardNumber, string cv2, string expiryDate, string consumerReference, string postCode)
        //        {
        //            var registerCard = new RegisterCardModel()
        //            {
        //                CardAddress = new CardAddressModel()
        //                {
        //                    PostCode = postCode
        //                },
        //                CardNumber = cardNumber,
        //                CV2 = cv2,
        //                ExpiryDate = expiryDate,
        //                YourConsumerReference = consumerReference
        //            };
        //
        //			return null;//  JudoSDKManager.JudoClient.RegisterCards.Create(registerCard);
        //        }
    }
}