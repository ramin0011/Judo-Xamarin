using System;
using Android.Webkit;
using JudoPayDotNet.Models;
using System.Collections.Generic;
using Java.IO;
using Org.Apache.Http.Protocol;
using Org.Apache.Http.Client.Entity;
using Org.Apache.Http.Util;
using Android.Views;
using System.Text;
using Java.Util;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using JudoDotNetXamarin;

namespace JudoDotNetXamarinAndroidSDK
{
    public class SecureManager: WebViewClient , JsonParsingJavaScriptInterface.JsonListener
    {

        static long ReceiptID;

        string acsUrl;
        string postbackUrl;
        string javaScriptNamespace;
        const string RedirectUrl = "https://pay.judopay.com/Android/Parse3DS";
        internal IPaymentService _paymentService;



        internal SecureManager (IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        SecureViewCallback _secureCallback;

        public override bool ShouldOverrideUrlLoading (WebView view, string url)
        {
            view.LoadUrl (url);

           
            return true;
        }

        public void SetCallBack (SecureViewCallback secureViewCallback)
        {
            _secureCallback = secureViewCallback;
        }


        void JsonParsingJavaScriptInterface.JsonListener.onJsonReceived (string json)
        {
            
            Dictionary<string,string> queryStringDictionary = new Dictionary<string,string> ();

            var TrackTraceDataArray = json.Split (new char[] { ',' });

            foreach (string keyValuePair in TrackTraceDataArray) {
                var pairComponents = keyValuePair.Split (new char[] { ':' });
                string key = pairComponents.First ();
                string value = pairComponents.Last ();
                queryStringDictionary.Add (key.Replace ("\"", string.Empty), value.Replace ("\"", string.Empty));
            }
                
            var paRes = queryStringDictionary ["PaRes"];
            var MD = queryStringDictionary ["MD"];
            _paymentService.CompleteDSecure (ReceiptID, paRes, MD).ContinueWith (reponse => {
                var result = reponse.Result;

                var paymentreceipt = result != null ? result.Response as PaymentReceiptModel : null;
                if (result.Error != null) {
                    var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                    _secureCallback (paymentreceipt, judoError);
                } else {
                    _secureCallback (paymentreceipt);
                }
            });
        }

        public void SummonThreeDSecure (PaymentRequiresThreeDSecureModel threedDSecureReceipt, WebView secureView)
        {
            
            secureView.AddJavascriptInterface (new JsonParsingJavaScriptInterface (this), "JudoPay");
            ReceiptID = threedDSecureReceipt.ReceiptId;
            string postString = "MD=" + threedDSecureReceipt.Md + "&PaReq=" + Uri.EscapeDataString (threedDSecureReceipt.PaReq) + "&TermUrl=" + Uri.EscapeDataString (RedirectUrl) + "";

            var byteArray = EncodingUtils.GetBytes (postString, "utf-8");
            secureView.LoadUrl ("aboutblank");
            secureView.Visibility = ViewStates.Visible;
            secureView.PostUrl (threedDSecureReceipt.AcsUrl, byteArray);
         
        }

        public override void OnPageFinished (WebView view, string url)
        {
           
            base.OnPageFinished (view, url);
            if (url == RedirectUrl) {
                view.LoadUrl (String.Format ("javascript:window.JudoPay.parseJsonFromHtml(document.documentElement.innerHTML);", javaScriptNamespace));
            }
        }

        public override void OnFormResubmission (WebView view, Android.OS.Message dontResend, Android.OS.Message resend)
        {
            base.OnFormResubmission (view, dontResend, resend);
        }

    }
}


