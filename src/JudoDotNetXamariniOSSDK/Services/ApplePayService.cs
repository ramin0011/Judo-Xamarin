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

		public  void MakeApplePayment (ApplePayViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController controller, ApplePaymentType type)
		{
			try
			{
				PKPaymentRequest request = new PKPaymentRequest();

				request.CurrencyCode = payment.CurrencyCode;

				request.CountryCode = payment.CountryCode;

				request.MerchantCapabilities = (PKMerchantCapability)payment.MerchantCapabilities;


				request.SupportedNetworks = payment.SupportedNetworks;


				request.PaymentSummaryItems =payment.Basket;

				request.MerchantIdentifier =payment.MerchantIdentifier;// @"merchant.com.judo.Xamarin"; // do it with configuration/overwrite

				var pkDelegate = new JudoPKPaymentAuthorizationViewControllerDelegate(this,request,type,success,failure);



				PKPaymentAuthorizationViewController pkController = new PKPaymentAuthorizationViewController(request){Delegate = pkDelegate };
				controller.PresentViewController(pkController,true,null);

			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
			}
		}		


		public void ApplePreAuthoriseCard (ApplePayViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController controller)
		{
			throw new NotImplementedException ();
		}
	

		public async Task<IResult<ITransactionResult>> HandlePKPayment (PKPayment payment,NSDecimalNumber amount, ApplePaymentType type)
		{
			try
			{
				CardPaymentModel paymentmodel = new CardPaymentModel
				{
					JudoId = JudoConfiguration.Instance.JudoId,
					ClientDetails = JudoSDKManager.GetClientDetails(),
				};
					
			
				var test = payment.Token.PaymentData.ToString(NSStringEncoding.UTF8);
				JObject jo=  JObject.Parse(test.ToString());
				PKPaymentModel pkModel = new PKPaymentModel()
				{
					JudoId = JudoConfiguration.Instance.JudoId,
					YourPaymentReference = "paymentRef12343",
					YourConsumerReference = "CUSTOMERREF1234",
					Amount = amount.ToDecimal(),
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
				Task<IResult<ITransactionResult>> task=null ;
				if(type == ApplePaymentType.Payment)
				{
					
				task =  _judoAPI.Payments.Create(pkModel);
				}
				else if(type == ApplePaymentType.PreAuth)
				{
				task =  _judoAPI.PreAuths.Create(pkModel);
				}
				if(task==null)
				{
					throw new Exception("MAKE A PROPER EXCEPTION");
				}
				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}
		}
	}
}

