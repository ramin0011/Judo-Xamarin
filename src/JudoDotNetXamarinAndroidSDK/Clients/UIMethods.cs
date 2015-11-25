using JudoDotNetXamarin;
using Android.Content;
using Android.App;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinAndroidSDK.Activies;
using System;
using Result = Android.App.Result;
using JudoPayDotNet.Models;
using Java.Interop;
using Java.Net;

namespace JudoDotNetXamarinAndroidSDK
{
    [Activity]
    internal class UIMethods : Activity,JudoAndroidSDKAPI
    {
        private const int ACTION_CARD_PAYMENT = 101;
        private const int ACTION_TOKEN_PAYMENT = 102;
        private const int ACTION_PREAUTH = 201;
        private const int ACTION_TOKEN_PREAUTH = 202;
        private const int ACTION_REGISTER_CARD = 301;

        private static Lazy<JudoSuccessCallback> _judoSuccessCallback;

        private static Lazy<JudoFailureCallback> _judoFailureCallback;

        protected override void OnCreate (Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);



            // var reqCode = Intent.GetStringExtra ("requestCode");
            Intent i; //= new Intent (this, typeof(PaymentActivity));
            var requestCode = Intent.GetStringExtra (JudoSDKManager.REQUEST_CODE);
            PopulateIntent (out i);


            //
            //            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, judoPaymentRef);
            //            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, judoConsumer);
            //            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, judoAmount);
            //            i.PutExtra (JudoSDKManager.JUDO_ID, judoId);
            //            i.PutExtra (JudoSDKManager.JUDO_CURRENCY,judoCurrency);


            this.StartActivityForResult (i, Int32.Parse (requestCode));

        }

        public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof(UIMethods));
            i.PutExtra (JudoSDKManager.REQUEST_CODE, ACTION_CARD_PAYMENT.ToString ());
          
            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, payment.PaymentReference);
            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, new SConsumer (payment.ConsumerReference));
            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (JudoSDKManager.JUDO_ID, JudoConfiguration.Instance.JudoId);
            i.PutExtra (JudoSDKManager.JUDO_CURRENCY, payment.Currency);
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);
        
            context.StartActivityForResult (i, ACTION_CARD_PAYMENT);

           

        }

       

        public void PreAuth (JudoDotNetXamarin.PaymentViewModel preAuthorisation, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof(UIMethods));
            i.PutExtra (JudoSDKManager.REQUEST_CODE, ACTION_PREAUTH.ToString ());

            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, preAuthorisation.PaymentReference);
            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, new SConsumer (preAuthorisation.ConsumerReference));
            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, preAuthorisation.Amount.ToString ());
            i.PutExtra (JudoSDKManager.JUDO_ID, JudoConfiguration.Instance.JudoId);
            i.PutExtra (JudoSDKManager.JUDO_CURRENCY, preAuthorisation.Currency);
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);

            context.StartActivityForResult (i, ACTION_PREAUTH);
        }


        public void TokenPayment (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof(UIMethods));
            i.PutExtra (JudoSDKManager.REQUEST_CODE, ACTION_TOKEN_PAYMENT.ToString ());

            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, payment.PaymentReference);
            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, new SConsumer (payment.ConsumerReference, payment.ConsumerToken));   
            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (JudoSDKManager.JUDO_ID, JudoConfiguration.Instance.JudoId);
            i.PutExtra (JudoSDKManager.JUDO_CURRENCY, payment.Currency);
            i.PutExtra (JudoSDKManager.JUDO_CARD_DETAILS, payment.Token);
            i.PutExtra (JudoSDKManager.JUDO_CARD_DETAILS, new SCardToken () {
                CardLastFour = payment.LastFour,
                CardType = payment.CardType,
                Token = payment.Token,
                ConsumerToken = payment.ConsumerToken
            });   
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);

            context.StartActivityForResult (i, ACTION_TOKEN_PAYMENT);
        }


        public void TokenPreAuth (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            throw new System.NotImplementedException ();
        }


        public void RegisterCard (JudoDotNetXamarin.PaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            throw new System.NotImplementedException ();
        }

        void PopulateIntent (out Intent i)
        {

            var requestCode = Intent.GetStringExtra (JudoSDKManager.REQUEST_CODE);
            var judoCurrency = Intent.GetStringExtra (JudoSDKManager.JUDO_CURRENCY);
            var judoPaymentRef = Intent.GetStringExtra (JudoSDKManager.JUDO_PAYMENT_REF);
            var judoConsumer = Intent.GetParcelableExtra (JudoSDKManager.JUDO_CONSUMER).JavaCast<Models.SConsumer> ();

            var judoAmount = decimal.Parse (Intent.GetStringExtra (JudoSDKManager.JUDO_AMOUNT));
            var judoId = Intent.GetStringExtra (JudoSDKManager.JUDO_ID);

            i = GetIntentForRequestCode (requestCode);
            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, judoPaymentRef);
            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, judoConsumer);
            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, judoAmount.ToString ());
            i.PutExtra (JudoSDKManager.JUDO_ID, judoId);
            i.PutExtra (JudoSDKManager.JUDO_CURRENCY, judoCurrency);
            var intCode = Int32.Parse (requestCode);
            if (intCode == ACTION_TOKEN_PAYMENT || intCode == ACTION_TOKEN_PREAUTH) {
                var judoCardToken = Intent.GetParcelableExtra (JudoSDKManager.JUDO_CARD_DETAILS).JavaCast<SCardToken> ();
                i.PutExtra (JudoSDKManager.JUDO_CARD_DETAILS, judoCardToken);
            }
        }

        Intent GetIntentForRequestCode (string requestCode)
        {
            Intent i = new Intent ();
            switch (Int32.Parse (requestCode)) {
            case ACTION_CARD_PAYMENT:
                i = new Intent (this, typeof(PaymentActivity));
                break;
            case ACTION_PREAUTH:
                i = new Intent (this, typeof(PreAuthActivity));
                break;
            case ACTION_TOKEN_PAYMENT:
                i = new Intent (this, typeof(PaymentTokenActivity));
                break;
            case ACTION_TOKEN_PREAUTH:
                i = new Intent (this, typeof(PreAuthTokenActivity));
                break;
            case ACTION_REGISTER_CARD:
                i = new Intent (this, typeof(RegisterCardActivity));
                break;

            }
            return i;
        }

        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            SReceipt receipt = null;
            string msg_prefix = "";

            if (data != null) {
                
//                
//                if(error==null)
//                {
//                    
//                }
                receipt = data.GetParcelableExtra (JudoSDKManager.JUDO_RECEIPT) as SReceipt;
              
             

                if (resultCode == Result.Canceled) {
                    //_judoFailureCallback.Value (new JudoError (){ Exception = new Exception ("Action was cancelled") });
                    Finish ();
                } else if (resultCode == JudoSDKManager.JUDO_ERROR) {
                    var error = data.GetParcelableExtra (JudoSDKManager.JUDO_ERROR_EXCEPTION) as SJudoError;
                    PaymentReceiptModel paymentReceipt = null;
                    if (receipt != null) {
                        paymentReceipt = receipt.FullReceipt as PaymentReceiptModel;
                    }
                    var innerError = new JudoError () {
                        Exception = error.Exception,
                        ApiError = error.ApiError
                    };
                    _judoFailureCallback.Value (innerError, paymentReceipt);
         
                    Finish ();
                } else {
                    HandleRequestCode (requestCode, resultCode, receipt);
                }
//                       
//                        if (receipt != null) {
//                            Toast.MakeText (this, string.Format ("{0}: id: {1},\r\nMessage: {2},\r\nresult: {3}", msg_prefix, receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show ();
//                        }
//
            }
            Finish ();
        }

       

        //  private const int ACTION_REGISTER_CARD = 301;

        //        public void Payment(string judoId, string currency, string amount,
        //            string paymentReference, string consumerReference, Dictionary<string, string> metaData)
        //        {
        //            Intent intent = new Intent(Application.Context, typeof(PaymentActivity));
        //            intent.PutExtra(JudoSDKManager.JUDO_PAYMENT_REF, paymentReference);
        //            intent.PutExtra(JudoSDKManager.JUDO_CONSUMER, new Consumer(consumerReference));
        //            intent.PutExtra(JudoSDKManager.JUDO_AMOUNT, amount);
        //            intent.PutExtra(JudoSDKManager.JUDO_ID, judoId);
        //            intent.PutExtra(JudoSDKManager.JUDO_CURRENCY, currency);
        //
        //
        //            intent.PutExtra(JudoSDKManager.JUDO_META_DATA, new MetaData(metaData));
        //			PaymentActivity pa = new PaymentActivity ();
        //			pa.StartActivityForResult(intent, ACTION_CARD_PAYMENT);
        //        }
        //
        //        public Intent PreAuth(Context context, string judoId, string currency, string amount,
        //            string paymentReference, string consumerReference, Dictionary<string, string> metaData)
        //        {
        //            Intent intent = new Intent(context, typeof(PreAuthActivity));
        //            intent.PutExtra(JudoSDKManager.JUDO_PAYMENT_REF, paymentReference);
        //            intent.PutExtra(JudoSDKManager.JUDO_CONSUMER, new Consumer(consumerReference));
        //            intent.PutExtra(JudoSDKManager.JUDO_AMOUNT, amount);
        //            intent.PutExtra(JudoSDKManager.JUDO_ID, judoId);
        //            intent.PutExtra(JudoSDKManager.JUDO_CURRENCY, currency);
        //
        //
        //            intent.PutExtra(JudoSDKManager.JUDO_META_DATA, new MetaData(metaData));
        //
        //            return intent;
        //        }
        //
        //        public Intent TokenPayment(Context context, string judoId, string currency, string amount,
        //            string paymentReference, string consumerReference, CardToken cardToken, Dictionary<string, string> metaData, string consumerToken = null)
        //        {
        //            Intent intent = new Intent(context, typeof(PaymentTokenActivity));
        //            intent.PutExtra(JudoSDKManager.JUDO_PAYMENT_REF, paymentReference);
        //            intent.PutExtra(JudoSDKManager.JUDO_CONSUMER, new Consumer(consumerReference, consumerToken));
        //            intent.PutExtra(JudoSDKManager.JUDO_AMOUNT, amount);
        //            intent.PutExtra(JudoSDKManager.JUDO_ID, judoId);
        //            intent.PutExtra(JudoSDKManager.JUDO_CURRENCY, currency);
        //            intent.PutExtra(JudoSDKManager.JUDO_CARD_DETAILS, cardToken);
        //
        //
        //            intent.PutExtra(JudoSDKManager.JUDO_META_DATA, new MetaData(metaData));
        //
        //            return intent;
        //        }
        //
        //        public Intent TokenPreAuth(Context context, string judoId, string currency, string amount,
        //            string paymentReference, string consumerReference, CardToken cardToken, Dictionary<string, string> metaData, string consumerToken = null)
        //        {
        //            Intent intent = new Intent(context, typeof(PreAuthTokenActivity));
        //            intent.PutExtra(JudoSDKManager.JUDO_PAYMENT_REF, paymentReference);
        //            intent.PutExtra(JudoSDKManager.JUDO_CONSUMER, new Consumer(consumerReference, consumerToken));
        //            intent.PutExtra(JudoSDKManager.JUDO_AMOUNT, amount);
        //            intent.PutExtra(JudoSDKManager.JUDO_ID, judoId);
        //            intent.PutExtra(JudoSDKManager.JUDO_CURRENCY, currency);
        //            intent.PutExtra(JudoSDKManager.JUDO_CARD_DETAILS, cardToken);
        //
        //
        //            intent.PutExtra(JudoSDKManager.JUDO_META_DATA, new MetaData(metaData));
        //
        //            return intent;
        //        }
        //
        //        public Intent RegisterCard(Context context, string consumerReference)
        //        {
        //            Intent intent = new Intent(context, typeof(RegisterCardActivity));
        //            intent.PutExtra(JudoSDKManager.JUDO_CONSUMER, new Consumer(consumerReference));
        //
        //            return intent;
        //        }


        void HandleRequestCode (int requestCode, Result resultCode, SReceipt receipt)
        {
            if (resultCode == Result.Ok && receipt.Result != "Declined") {
                PaymentReceiptModel paymentReceipt;
                if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null) {

                    _judoSuccessCallback.Value (paymentReceipt);
                    Finish ();

                }
            } else if (_judoFailureCallback.Value != null) {
                //var judoError = new JudoError { ApiError = receipt != null ? receipt : null };
                var judoError = new JudoError ();
                var paymentreceipt = receipt != null ? receipt.FullReceipt as PaymentReceiptModel : null;

                if (paymentreceipt != null) {
                    // send receipt even we got card declined

                    _judoFailureCallback.Value (judoError, paymentreceipt);
                    Finish ();
                } else {

                    _judoFailureCallback.Value (judoError);
                    Finish ();
                }
            }

        }



        //            switch (requestCode) {
        //            case ACTION_CARD_PAYMENT:
        //                if (resultCode == Result.Ok && receipt.Result != "Declined") {
        //                    PaymentReceiptModel paymentReceipt;
        //                    if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null) {
        //
        //                        _judoSuccessCallback.Value (paymentReceipt);
        //
        //                        cardToken = paymentReceipt.CardDetails.CardToken;
        //                        consumerToken = paymentReceipt.Consumer.ConsumerToken;
        //                        rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
        //                        lastFour = paymentReceipt.CardDetails.CardLastfour;
        //                        cardType = (CardType)paymentReceipt.CardDetails.CardType;
        //                    }
        //                    _judoSuccessCallback.Value (paymentReceipt);
        //                    //msg_prefix = "Payment succeeded";
        //                } else {
        //                    msg_prefix = "Payment failed";
        //                }
        //                break;
        //            case ACTION_PREAUTH:
        //                if (resultCode == Result.Ok && receipt.Result != "Declined") {
        //                    PaymentReceiptModel paymentReceipt;
        //                    if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null) {
        //                        preAuth_cardToken = paymentReceipt.CardDetails.CardToken;
        //                        preAuth_consumerToken = paymentReceipt.Consumer.ConsumerToken;
        //                        preAuth_rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
        //                        preAuth_lastFour = paymentReceipt.CardDetails.CardLastfour;
        //                        preAuth_cardType = (CardType)paymentReceipt.CardDetails.CardType;
        //                    }
        //
        //                    msg_prefix = "PreAuth card payment succeeded";
        //                } else {
        //                    msg_prefix = "PreAuth card payment failed";
        //                }
        //                break;
        //            case ACTION_TOKEN_PAYMENT:
        //                if (resultCode == Result.Ok && receipt.Result != "Declined") {
        //                    msg_prefix = "Token payment succeeded";
        //                } else {
        //                    msg_prefix = "Token payment failed";
        //                }
        //                break;
        //            case ACTION_TOKEN_PREAUTH:
        //                if (resultCode == Result.Ok && receipt.Result != "Declined") {
        //                    msg_prefix = "PreAuth Token payment succeeded";
        //                } else {
        //                    msg_prefix = "PreAuth Token payment failed";
        //                }
        //                break;
        //            case ACTION_REGISTER_CARD:
        //                if (resultCode == Result.Ok && receipt.Result != "Declined") {
        //                    PaymentReceiptModel paymentReceipt;
        //                    if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null) {
        //                        cardToken = paymentReceipt.CardDetails.CardToken;
        //                        consumerToken = paymentReceipt.Consumer.ConsumerToken;
        //                        rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
        //                        lastFour = paymentReceipt.CardDetails.CardLastfour;
        //                        cardType = (CardType)paymentReceipt.CardDetails.CardType;
        //                    }
        //
        //                    msg_prefix = "Register card succeeded";
        //                } else {
        //                    msg_prefix = "Register card failed";
        //                }
        //                break;

    }
}
    
