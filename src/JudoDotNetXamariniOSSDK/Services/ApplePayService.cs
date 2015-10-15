using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;
using PassKit;
using UIKit;
using System.Net.Cache;
using JudoPayDotNet;
using System.Runtime.Remoting.Channels;

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

				// identify the currency you wish to process the transaction in. This currency must be supported by your judo account.
				request.CurrencyCode = payment.CurrencyCode;

				// This identifies the country where the transaction will be processed, for judo this will be "GB"
				request.CountryCode = payment.CountryCode;

				// Identify the type of Apple Pay transaction you want to process, this will always be PKMerchantCapability3DS
				request.MerchantCapabilities = payment.MerchantCapabilities;

				// add the card networks you can accept, this will usually be either @[PKPaymentNetworkVisa, PKPaymentNetworkMasterCard] or @[PKPaymentNetworkVisa, PKPaymentNetworkMasterCard, PKPaymentNetworkAmex]
				request.SupportedNetworks = payment.SupportedNetworks;

				// Next you need to include a summary of the user's basket, including total amount payable as the last line.

				//PKPaymentSummaryItem *lineItem1 = [self paymentSummaryItemWithLabel:@"iPhone charger" amount:14.00f];
				//PKPaymentSummaryItem *vatLine = [self paymentSummaryItemWithLabel:@"VAT" amount:2.80f];

				// The final line in the basket must always be the total, here rather than identifying the line we identify the recipient of the payment. This will appear prefixed with the
				// word "PAY" when presented in the Apple Pay sleeve. For example here the final line of the basket will be "PAY AWESOME CO £16.80"
				//PKPaymentSummaryItem *total = [self paymentSummaryItemWithLabel:@"Awesome Co" amount:16.80f];

				//NSArray *basket = @[lineItem1, vatLine, total];
				request.PaymentSummaryItems =payment.Basket;


				// set the merchant ID you wish to use (useful when you have multiple merchant IDs in a single app)
				request.MerchantIdentifier =payment.MerchantIdentifier;// @"merchant.com.judo.Xamarin"; // do it with configuration/overattion


				// Finally create the Apple Pay view controller show it to the user
				//self.apController = [[PKPaymentAuthorizationViewController alloc]initWithPaymentRequest: request];
				var pkController = new PKPaymentAuthorizationViewController(request);

				// Set a delegate to handle the processing once the user has approved payment in the Apple Pay sleeve.
				pkController.DidAuthorizePayment  +=  delegate(object sender, PKPaymentAuthorizationEventArgs args) {

					ExitDelegate(args.Payment,appleCallback);

				};

				controller.PresentViewController(pkController,true,null);



			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
			}
		}			
	

		 void  ExitDelegate (PKPayment payment, ApplePayCallBack applePayCallback)
		{
			applePayCallback (payment);
//			Task<IResult<ITransactionResult>> task =  _judoAPI. Payments.Create(payment);
//			return await task;
		}

		public void ApplePreAuthoriseCard (ApplePayViewModel payment, ApplePayCallBack appleCallback, UINavigationController controller)
		{
			throw new NotImplementedException ();
		}
	}
}

