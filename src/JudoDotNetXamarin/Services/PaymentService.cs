using System;
using System.Threading.Tasks;
using JudoDotNetXamarin;
using JudoPayDotNet;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo ("JudoDotNetXamariniOSSDK")]
[assembly: InternalsVisibleTo ("JudoDotNetXamarinAndroidSDK")]
namespace JudoDotNetXamarin
{
    internal class PaymentService : IPaymentService
    {
        private	JudoPayApi _judoAPI;

        public PaymentService (JudoPayApi judoAPI)
        {
            _judoAPI = judoAPI;
        }

        public async Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel paymentViewModel, IClientService clientService)
        {
            try {
                CardPaymentModel payment = new CardPaymentModel {
                    JudoId = JudoConfiguration.Instance.JudoId,
                    YourPaymentReference = paymentViewModel.PaymentReference,
                    YourConsumerReference = paymentViewModel.ConsumerReference,
                    Amount = paymentViewModel.Amount,
                    CardNumber = paymentViewModel.Card.CardNumber,
                    CV2 = paymentViewModel.Card.CV2,
                    ExpiryDate = paymentViewModel.Card.ExpireDate,
                    CardAddress = new CardAddressModel () {
                        PostCode = paymentViewModel.Card.PostCode,
                        CountryCode = (int)paymentViewModel.Card.CountryCode
                    },
                    StartDate = paymentViewModel.Card.StartDate,
                    IssueNumber = paymentViewModel.Card.IssueNumber,
                    YourPaymentMetaData = paymentViewModel.YourPaymentMetaData,
                    ClientDetails = clientService.GetClientDetails (),
                    Currency = paymentViewModel.Currency,
                    UserAgent = clientService.GetSDKVersion ()
                };

                Task<IResult<ITransactionResult>> task = _judoAPI.Payments.Create (payment);

                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel () {
                        ErrorMessage = e.InnerException.ToString ()
                    }
                };
                throw error;
                return null;
            }

        }

        public async Task<IResult<ITransactionResult>> PreAuthoriseCard (PaymentViewModel authorisation, IClientService clientService)
        {
            try {
                CardPaymentModel payment = new CardPaymentModel {
                    JudoId = JudoConfiguration.Instance.JudoId,
                    YourPaymentReference = authorisation.PaymentReference,
                    YourConsumerReference = authorisation.ConsumerReference,
                    Amount = authorisation.Amount,
                    CardNumber = authorisation.Card.CardNumber,
                    CV2 = authorisation.Card.CV2,
                    ExpiryDate = authorisation.Card.ExpireDate,
                    CardAddress = new CardAddressModel () {
                        PostCode = authorisation.Card.PostCode,
                        CountryCode = (int)authorisation.Card.CountryCode
                    },
                    StartDate = authorisation.Card.StartDate,
                    IssueNumber = authorisation.Card.IssueNumber,
                    YourPaymentMetaData = authorisation.YourPaymentMetaData,
                    ClientDetails = clientService.GetClientDetails (),
                    UserAgent = clientService.GetSDKVersion (),
                    Currency = authorisation.Currency
                };

                Task<IResult<ITransactionResult>> task = _judoAPI.PreAuths.Create (payment);
                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel () {
                        ErrorMessage = e.InnerException.ToString ()
                    }
                };
                throw error;		
                return null;
            }

        }

        public async Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {
            try {
                TokenPaymentModel payment = new TokenPaymentModel {
                    JudoId = JudoConfiguration.Instance.JudoId,
                    YourPaymentReference = tokenPayment.PaymentReference,
                    YourConsumerReference = tokenPayment.ConsumerReference,
                    Amount = tokenPayment.Amount,
                    CardToken = tokenPayment.Token,
                    CV2 = tokenPayment.CV2,
                    ConsumerToken = tokenPayment.ConsumerToken,
                    YourPaymentMetaData = tokenPayment.YourPaymentMetaData,
                    ClientDetails = clientService.GetClientDetails (),
                    UserAgent = clientService.GetSDKVersion ()
                };
                Task<IResult<ITransactionResult>> task = _judoAPI.Payments.Create (payment);
                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel () {
                        ErrorMessage = e.InnerException.ToString ()
                    }
                };
                throw error;	
                return null;
            }

        }

        public async Task<IResult<ITransactionResult>> MakeTokenPreAuthorisation (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {
            TokenPaymentModel payment = new TokenPaymentModel {
                JudoId = JudoConfiguration.Instance.JudoId,
                YourPaymentReference = tokenPayment.PaymentReference,
                YourConsumerReference = tokenPayment.ConsumerReference,
                Amount = tokenPayment.Amount,
                CardToken = tokenPayment.Token,
                CV2 = tokenPayment.CV2,
                ConsumerToken = tokenPayment.ConsumerToken,
                YourPaymentMetaData = tokenPayment.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion ()
            };
            try {
                Task<IResult<ITransactionResult>> task = _judoAPI.PreAuths.Create (payment);
                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel () {
                        ErrorMessage = e.InnerException.ToString ()
                    }
                };
                throw error;	
                return null;
            }
        }

        public async Task<IResult<ITransactionResult>> RegisterCard (PaymentViewModel payment, IClientService clientService)
        {


            var registerCard = new CardPaymentModel () {
                JudoId = JudoConfiguration.Instance.JudoId,
                YourPaymentReference = payment.PaymentReference,
                YourConsumerReference = payment.ConsumerReference,
                Amount = payment.Amount,
                CardNumber = payment.Card.CardNumber,
                CV2 = payment.Card.CV2,
                ExpiryDate = payment.Card.ExpireDate,
                CardAddress = new CardAddressModel () {
                    PostCode = payment.Card.PostCode,
                    CountryCode = (int)payment.Card.CountryCode
                },
                StartDate = payment.Card.StartDate,
                IssueNumber = payment.Card.IssueNumber,
                YourPaymentMetaData = payment.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion (),
                Currency = payment.Currency
            };
            try {
                Task<IResult<ITransactionResult>> task = _judoAPI.RegisterCards.Create (registerCard);
                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel () {
                        ErrorMessage = e.InnerException.ToString ()
                    }
                };
                throw error;
                return null;
            }
        }

        public async Task<IResult<ITransactionResult>> CompleteDSecure (string receiptID, string paRes, string md)
        {
            try {
                ThreeDResultModel model = new ThreeDResultModel ();
                model.PaRes = paRes;
                Task<IResult<PaymentReceiptModel>> task = _judoAPI.ThreeDs.Complete3DSecure (receiptID, model);
                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel () {
                        ErrorMessage = e.InnerException.ToString ()
                    }
                };
                throw error;
                return null;
            }
        }
    }
}
