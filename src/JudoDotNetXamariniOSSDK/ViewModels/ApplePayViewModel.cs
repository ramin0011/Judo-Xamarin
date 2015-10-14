using System;
using System.Dynamic;
using PassKit;
using System.Linq;

namespace JudoDotNetXamariniOSSDK
{
	public class ApplePayViewModel
	{
		public String CurrencyCode  { get; set;}

		public String CountryCode  { get; set;}

		public String MerchantCapabilities  { get {return"PKMerchantCapability3DS";}}

		//[PKPaymentNetworkVisa, PKPaymentNetworkMasterCard] or @[PKPaymentNetworkVisa, PKPaymentNetworkMasterCard, PKPaymentNetworkAmex]
	
		public String[] SupportedNetworks { get; set;}
		# Next you need to include a summary of the user's basket, including total amount payable as the last line.

		public PKPaymentSummaryItem[] SummaryItems {get;set;}

		/// <summary>
		/// Final item on sleeve totalling previous items. Made out to merchant
		/// </summary>
		/// <value>The total summary item.</value>
		public PKPaymentSummaryItem TotalSummaryItem { get; set;}

		public PKPaymentSummaryItem[] Basket {
			get{ 
				if (SummaryItems.Length == 0) {
					return new PKPaymentSummaryItem[0] ();
				} else {
					var _basket = SummaryItems.ToList ();
					if (TotalSummaryItem != null) {
						_basket.Add (TotalSummaryItem);
					}
					return _basket.ToArray ();
				}
				 
				}
		}

		public String MerchantIdentifier  { get; set;}

	}
}

