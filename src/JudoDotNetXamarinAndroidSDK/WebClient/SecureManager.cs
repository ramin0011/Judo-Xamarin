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



        private  void JsonListenerCallback (string json)
        {
            var test = "test";
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
            if (url == RedirectUrl)
                view.LoadUrl (String.Format ("javascript:window.JudoPay.parseJsonFromHtml(document.documentElement.innerHTML);", javaScriptNamespace));
        }

        
       

        public override void OnFormResubmission (WebView view, Android.OS.Message dontResend, Android.OS.Message resend)
        {
            base.OnFormResubmission (view, dontResend, resend);
        }

        public override void OnLoadResource (WebView view, string url)
        {
//            try {
//                // string url = "http://mydomain...";
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
//                request.Method = "GET/POST";
//                //using GET - request.Headers.Add ("Authorization","Authorizaation value");
//                request.ContentType = "application/json";
//                HttpWebResponse myResp = (HttpWebResponse)request.GetResponse ();
//                string responseText;
//
//                using (var response = request.GetResponse ()) {
//                    using (var reader = new StreamReader (response.GetResponseStream ())) {
//                        responseText = reader.ReadToEnd ();
//                        //                        Console.WriteLine(responseText);
//                        //                        var forth = new Intent (this, typeof(SecondActivity));
//                        //                        forth.PutExtra ("responseText2", responseText);
//                        //                        StartActivity (forth);
//                    }
//                }
//
//            } catch (WebException exception) {
//                string responseText;
//                using (var reader = new StreamReader (exception.Response.GetResponseStream ())) {
//                    responseText = reader.ReadToEnd ();
//                    // Console.WriteLine (responseText);
//                }
//            }

            base.OnLoadResource (view, url);
        }


    }
}


