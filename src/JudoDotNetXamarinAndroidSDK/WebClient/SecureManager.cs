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

namespace JudoDotNetXamarinAndroidSDK
{
    public class SecureManager: WebViewClient
    {
        static long ReceiptID;

        string acsUrl;
        string postbackUrl;
        string javaScriptNamespace;
        const string RedirectUrl = "judo1234567890://threedsecurecallback/";
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


        public static object SummonThreeDSecure (PaymentRequiresThreeDSecureModel threedDSecureReceipt, WebView secureView)
        {
            
           
            List<Tuple<string,string>> test = new List<Tuple<string,string>> ();

            string postString = "MD=" + threedDSecureReceipt.Md + "&PaReq=" + Uri.EscapeDataString (threedDSecureReceipt.PaReq) + "&TermUrl=" + Uri.EscapeDataString (RedirectUrl) + "";

            var byteArray = EncodingUtils.GetBytes (postString, "utf-8");
            secureView.LoadUrl ("aboutblank");
            secureView.Visibility = ViewStates.Visible;
            secureView.PostUrl (threedDSecureReceipt.AcsUrl, byteArray);
            return null;
        }

        public override void OnPageFinished (WebView view, string url)
        {
            
            base.OnPageFinished (view, url);
        }

        public override WebResourceResponse ShouldInterceptRequest (WebView view, IWebResourceRequest request)
        {
            return  base.ShouldInterceptRequest (view, request);
        }

        public override void OnFormResubmission (WebView view, Android.OS.Message dontResend, Android.OS.Message resend)
        {
            base.OnFormResubmission (view, dontResend, resend);
        }

        public override void OnLoadResource (WebView view, string url)
        {
            base.OnLoadResource (view, url);
        }





        //        +
        //        +    private ThreeDSecureResultPageListener resultPageListener;
        //        +
        //        +    public ThreeDSecureWebViewClient(String acsUrl, String postbackUrl, String javaScriptNamespace, ThreeDSecureListener threeDSecureListener) {
        //            +        this.acsUrl = acsUrl;
        //            +        this.postbackUrl = postbackUrl;
        //            +        this.javaScriptNamespace = javaScriptNamespace;
        //            +        this.threeDSecureListener = threeDSecureListener;
        //            +    }
        //        +
        //        +    @Override
        //        +    public void onPageFinished(WebView view, String url) {
        //            +        super.onPageFinished(view, url);
        //            +
        //            +        if (url.equals(acsUrl)) {
        //                +            threeDSecureListener.onAuthorizationWebPageLoaded();
        //            +        } else if (url.equals(postbackUrl)) {
        //                +            view.loadUrl(String.format("javascript:window.%s.parseJsonFromHtml(document.getElementsByTagName('html')[0].innerHTML);", javaScriptNamespace));
        //                +        }
        //            +    }
        //        +
        //        +    @Override
        //        +    public void onPageStarted(WebView view, String url, Bitmap favicon) {
        //            +        super.onPageStarted(view, url, favicon);
        //            +
        //            +        if (url.equals(postbackUrl)) {
        //                +            if (resultPageListener != null) {
        //                    +                resultPageListener.onPageStarted();
        //                    +            }
        //                +            view.setVisibility(INVISIBLE);
        //                +        }
        //            +    }
        //        +
        //        +    public void setResultPageListener(ThreeDSecureResultPageListener resultPageListener) {
        //            +        this.resultPageListener = resultPageListener;
        //            +    }
        //        +
        //        +    @Override
        //        +    public void onReceivedError(WebView view, int errorCode, String description, String failingUrl) {
        //            +        super.onReceivedError(view, errorCode, description, failingUrl);
        //            +
        //            +        if (!failingUrl.startsWith(postbackUrl)) {
        //                +            threeDSecureListener.onAuthorizationWebPageLoadingError(errorCode, description, failingUrl);
        //                +        }
        //            +    }
    }
}


