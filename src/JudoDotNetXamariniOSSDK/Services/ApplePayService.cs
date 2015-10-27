using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;
using PassKit;
using UIKit;
using System.Net.Cache;
using JudoPayDotNet;
using System.Runtime.Remoting.Channels;
using Foundation;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JudoDotNetXamariniOSSDK
{
	public class ApplePayService :IApplePayService
	{
		private	JudoPayApi _judoAPI;

		public ApplePayService (JudoPayApi judoAPI)
		{
			_judoAPI = judoAPI;
		}

		public  void MakeApplePayment (ApplePayViewModel payment, ApplePayCallBack appleCallback, UINavigationController controller)
		{
			try
			{
				PKPaymentRequest request = new PKPaymentRequest();

				request.CurrencyCode = payment.CurrencyCode;

				request.CountryCode = payment.CountryCode;

				request.MerchantCapabilities = (PKMerchantCapability)payment.MerchantCapabilities;


				request.SupportedNetworks = payment.SupportedNetworks;


				request.PaymentSummaryItems =payment.Basket;

				request.MerchantIdentifier =payment.MerchantIdentifier;// @"merchant.com.judo.Xamarin"; // do it with configuration/overattion

				var pkDelegate = new JudoPKPaymentAuthorizationViewControllerDelegate(this,request);



				PKPaymentAuthorizationViewController pkController = new PKPaymentAuthorizationViewController(request){Delegate = pkDelegate };

//				pkController.DidAuthorizePayment  +=  delegate(object sender, PKPaymentAuthorizationEventArgs args) {
//
//					ExitDelegate(args.Payment,appleCallback);
//
//				};
//
//				pkController.PaymentAuthorizationViewControllerDidFinish +=  delegate(object sender, EventArgs e) {
//				//	ExitDelegate(args.Payment,appleCallback);
//
//
//					var test= "test";
//				};

				//controller.PresentViewController(pkController,true,null);



			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
			}
		}			
	

		 void  ExitDelegate (PKPayment payment, ApplePayCallBack applePayCallback)
		{
			applePayCallback (payment);

		}

		public void ApplePreAuthoriseCard (ApplePayViewModel payment, ApplePayCallBack appleCallback, UINavigationController controller)
		{
			throw new NotImplementedException ();
		}

		public async Task<IResult<ITransactionResult>> HandlePKPayment (PKPayment payment,NSDecimalNumber amount)
		{
			try
			{
				CardPaymentModel paymentmodel = new CardPaymentModel
				{
					JudoId = JudoConfiguration.Instance.JudoId,
//					YourPaymentReference = paymentViewModel.PaymentReference,
//					YourConsumerReference = paymentViewModel.ConsumerReference,
//					Amount = paymentViewModel.Amount,
//					CardNumber = paymentViewModel.Card.CardNumber,
					//CV2 = paymentViewModel.Card.CV2,
					//ExpiryDate = paymentViewModel.Card.ExpireDate,
					//CardAddress = new CardAddressModel() { PostCode = paymentViewModel.Card.PostCode, CountryCode = (int)paymentViewModel.Card.CountryCode },
					//StartDate = paymentViewModel.Card.StartDate,
					//IssueNumber = paymentViewModel.Card.IssueNumber,
					//YourPaymentMetaData = paymentViewModel.YourPaymentMetaData,
					ClientDetails = JudoSDKManager.GetClientDetails(),
					//Currency = paymentViewModel.Currency
				};
					
				//var test= new NSString(payment.Token.PaymentData,NSStringEncoding.UTF8).ToString();
				var test = payment.Token.PaymentData.ToString(NSStringEncoding.UTF8);
//				test = test.Replace("\"","");
				JObject jo=  JObject.Parse(test.ToString());
				PKPaymentModel pkModel = new PKPaymentModel()
				{
					JudoId = JudoConfiguration.Instance.JudoId,
					YourPaymentReference = "paymentRef12343",
					YourConsumerReference = "CUSTOMERREF1234",
					Amount = amount.ToDecimal(),
				
//					
					ClientDetails = JudoSDKManager.GetClientDetails(),
					PkPayment = new PKPaymentInnerModel()
					{
							// 					BillingAddress = new CardAddressModel()
							//					{
							//						PostCode =payment.BillingContact.PostalAddress.PostalCode,
							//						Line1 = payment.BillingContact.PostalAddress.Street,
							//						Town = payment.BillingContact.PostalAddress.City
							//					},
						Token = new PKPaymentTokenModel()
						{
							PaymentData = jo,
							PaymentInstrumentName = payment.Token.PaymentInstrumentName,
							PaymentNetwork = payment.Token.PaymentNetwork
						}
					}

				};

				Task<IResult<ITransactionResult>> task =  _judoAPI.Payments.Create(pkModel);

				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}
		}
	}
}

