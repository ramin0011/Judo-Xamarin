using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreFoundation;
using JudoPayDotNet.Models;
using CoreGraphics;
using System.IO;
using System.Text;

namespace JudoDotNetXamariniOSSDK
{
	[Register("SecureWebView")]
	public class SecureWebView :UIWebView
	{
		public SecureWebView(IntPtr p) : base(p)
		{
		}

		private IPaymentService _paymentService;
		public string ReceiptID;
		public SuccessCallback _successCallback { private get; set; }
		public FailureCallback _failureCallback { private get; set; }


		public void SetupWebView(IPaymentService paymentService,SuccessCallback successCallback,FailureCallback failureCallback)
		{
			_paymentService = paymentService;
			_successCallback = successCallback;
			_failureCallback = failureCallback;
		}

		public override void LoadRequest (NSUrlRequest r)
		{
			base.LoadRequest (r);
			
		if(r.Url.ToString().Equals("judo1234567890://threedsecurecallback") && ReceiptID !=null)
		{
			Dictionary<string,string> queryStringDictionary = new Dictionary<string,string>();

			var TrackTraceDataArray = r.Body.ToString().Split (new char[] { '&' });

			foreach (string keyValuePair in TrackTraceDataArray)
			{
				var pairComponents = keyValuePair.Split (new char[] { '=' });
				string key =pairComponents.First();
				string value =pairComponents.Last();
				queryStringDictionary.Add(key,value);
			}

			_paymentService.CompleteDSecure (ReceiptID,queryStringDictionary["PaRes"],queryStringDictionary["MD"]).ContinueWith (reponse => {
					var result = reponse.Result;
					if (result != null && !result.HasError && result.Response.Result != "Declined") {
						var paymentreceipt = result.Response as PaymentReceiptModel;

						if (paymentreceipt != null) {
							// call success callback
							if (_successCallback != null)

								_successCallback (paymentreceipt);
						} else {
							throw new Exception ("JudoXamarinSDK: unable to find the receipt in response.");
						}

					} else {
						// Failure callback
						if (_failureCallback != null) {
							var judoError = new JudoError { ApiError = result != null ? result.Error : null };
							var paymentreceipt = result != null ? result.Response as PaymentReceiptModel : null;

							if (paymentreceipt != null) {
								// send receipt even we got card declined

								_failureCallback (judoError, paymentreceipt);
							} else {

								_failureCallback (judoError);
							}
						}
					}
			});

		}
		}
//
//		public override void LoadRequest (NSUrlRequest request)
//		{
//			base.LoadRequest (request);
//
//			if(request.Url.ToString().Equals("judo1234567890://threedsecurecallback") && ReceiptID !=null)
//			{
//				Dictionary<string,string> queryStringDictionary = new Dictionary<string,string>();
//
//				var TrackTraceDataArray = request.Body.ToString().Split (new char[] { '&' });
//
//				foreach (string keyValuePair in TrackTraceDataArray)
//				{
//					var pairComponents = keyValuePair.Split (new char[] { '=' });
//					string key =pairComponents.First();
//					string value =pairComponents.Last();
//					queryStringDictionary.Add(key,value);
//				}
//
//				_paymentService.CompleteDSecure (ReceiptID,queryStringDictionary["PaRes"],queryStringDictionary["MD"]).ContinueWith (reponse => {
//					var result = reponse.Result as PaymentReceiptViewModel;
//
//				});
//			}
//
//		}



	}
}

