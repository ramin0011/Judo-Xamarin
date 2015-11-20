using JudoDotNetXamarin;
using Android.Content;
using Android.App;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinAndroidSDK.Activies;
using System;

namespace JudoDotNetXamarinAndroidSDK
{
    [Activity]
    internal class UIMethods : Activity,JudoAndroidSDKAPI
    {
        private const int ACTION_CARD_PAYMENT = 101;
        private const int ACTION_TOKEN_PAYMENT = 102;
        private const int ACTION_PREAUTH = 201;
        private const int ACTION_TOKEN_PREAUTH = 202;

        private static Lazy<JudoSuccessCallback> _judoSuccessCallback;
        /*
        public static JudoSuccessCallback _judoSuccessCallbackInstance {
            get { return _judoSuccessCallbackInstance; }
        }*/

        private static Lazy<JudoFailureCallback> _judoFailureCallback;
        /*
        public static JudoSuccessCallback _judoFailureCallbackInstance {
            get { return _judoSuccessCallbackInstance; }
        }*/

        public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context)
        {
            Intent test = new Intent (context, typeof(UIMethods));
            test.PutExtra ("requestCode", ACTION_CARD_PAYMENT);
            Intent i = new Intent (context, typeof(PaymentActivity));
            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, payment.PaymentReference);
            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, new Consumer (payment.ConsumerReference));
            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (JudoSDKManager.JUDO_ID, JudoConfiguration.Instance.JudoId);
            i.PutExtra (JudoSDKManager.JUDO_CURRENCY, payment.Currency);
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);
            //  intent.PutExtra (JudoSDKManager.JUDO_META_DATA, new MetaData (payment.YourPaymentMetaData));
            //PaymentActivity pa = new PaymentActivity ();

            context.StartActivityForResult (test, ACTION_CARD_PAYMENT);

            // context.StartActivityForResult (i, ACTION_CARD_PAYMENT);

        }

        protected override void OnCreate (Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            var reqCode = Intent.GetStringExtra ("requestCode");
            // if (reqCode == ACTION_CARD_PAYMENT.ToString ()) {
            Intent i = new Intent (this, typeof(PaymentActivity));
            i.PutExtra (JudoSDKManager.JUDO_PAYMENT_REF, "1.01");
            i.PutExtra (JudoSDKManager.JUDO_CONSUMER, new Consumer ("1.01"));
            i.PutExtra (JudoSDKManager.JUDO_AMOUNT, "1.01");
            i.PutExtra (JudoSDKManager.JUDO_ID, "1.01");
            i.PutExtra (JudoSDKManager.JUDO_CURRENCY, "GBP");
            //i.PutExtra (JudoSDKManager.SUCCESS_CALLBACK, success);
            //i.PutExtra (JudoSDKManager.FAILURE_CALLBACK, failure);
               

            this.StartActivityForResult (i, ACTION_CARD_PAYMENT);
            // }

            //
        }

        public void PreAuth (JudoDotNetXamarin.PaymentViewModel preAuthorisation, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }


        public void TokenPayment (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }


        public void TokenPreAuth (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
        }


        public void RegisterCard (JudoDotNetXamarin.PaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure)
        {
            throw new System.NotImplementedException ();
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


        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            Receipt receipt = null;
            string msg_prefix = "";

            //            if (resultCode == Result.Canceled) {
            //                Toast.MakeText (this, "Payment Canceled.", ToastLength.Long).Show ();
            //                return;
            //            } else if (resultCode == JudoSDKManager.JUDO_ERROR) {
            //                Error err = data.GetParcelableExtra (JudoSDKManager.JUDO_ERROR_EXCEPTION) as Error;
            //        
            //                Toast.MakeText (this, string.Format ("Error: {0} {1}", data.GetStringExtra (JudoSDKManager.JUDO_ERROR_MESSAGE),
            //                   // err != null && err.Exception != null ? "\r\nException: " + err.Exception.Message : ""), ToastLength.Long).Show ();
            //                return;
            //            }
            //        
            //            if (data != null)
            //                receipt = data.GetParcelableExtra (JudoSDKManager.JUDO_RECEIPT) as Receipt;
            //        
            //            if (receipt == null) {
            //                Toast.MakeText (this, string.Format ("Error: {0}", data.GetStringExtra (JudoSDKManager.JUDO_ERROR_MESSAGE)), ToastLength.Long).Show ();
            //                return;
            //            }
            //        
            //            switch (requestCode) {
            //            case ACTION_CARD_PAYMENT:
            //                if (resultCode == Result.Ok && receipt.Result != "Declined") {
            //                    PaymentReceiptModel paymentReceipt;
            //                    if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null) {
            //                        cardToken = paymentReceipt.CardDetails.CardToken;
            //                        consumerToken = paymentReceipt.Consumer.ConsumerToken;
            //                        rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
            //                        lastFour = paymentReceipt.CardDetails.CardLastfour;
            //                        cardType = (CardType)paymentReceipt.CardDetails.CardType;
            //                    }
            //                    msg_prefix = "Payment succeeded";
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
            //            }
            //        
            //            if (receipt != null) {
            //                Toast.MakeText (this, string.Format ("{0}: id: {1},\r\nMessage: {2},\r\nresult: {3}", msg_prefix, receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show ();
            //            }
            //        
            //        }


        }
    }
}