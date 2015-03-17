using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using JudoDotNetXamarinSDK;
using JudoDotNetXamarinSDK.Models;
using JudoDotNetXamarinSDK.Utils;
using JudoPayDotNet.Models;
using Result = Android.App.Result;

namespace AndroidTestApp
{
    public class MainActivity : Activity
    {
        int count = 1;
        private const string ApiToken = "4eVWyZQnO5DyaXZy";
        private const string ApiSecret = "1d5e8381ed9ef3cc1ecc1daaf8ce550bdc97ea058ac804be4b68c28d02fdb791";

        private const int ACTION_PAYMENT = 1;
        private const int ACTION_PAYMENT_TOKEN = 2;
        private const int ACTION_PREAUTH_TOKEN = 4;
        private const int ACTION_REGISTER_CARD = 5;

        private volatile string cardToken;
        private volatile CardBase.CardType cardType;
        private volatile string consumerToken;
        private volatile string consumerRef;
        private volatile string lastFour;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            JudoSDKManager.Configuration.SetApiTokenAndSecret(ApiToken, ApiSecret);
            JudoSDKManager.Configuration.IsAVSEnabled = true;
            JudoSDKManager.Configuration.IsFraudMonitoringSignals = true;

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout
            Button makeAPaymentButton = FindViewById<Button>(Resource.Id.MakePayment);
            Button makeATokenPaymentButton = FindViewById<Button>(Resource.Id.MakePaymentToken);
            Button makeATokenPreAuthButton = FindViewById<Button>(Resource.Id.MakePreAuthToken);
            Button registerCard = FindViewById<Button>(Resource.Id.RegisterCard);

            var that = this;

            makeAPaymentButton.Click += (sender, e) =>
            {
                var judoId = "100016";
                var currency = "GBP";
                var amount = "4.99";
                var paymentReference = "payment101010102";
                var consumerRef = "consumer1010102";

                var intent = JudoSDKManager.UIMethods.Payment(that, judoId, currency, amount, paymentReference, consumerRef, new Dictionary<string, string>{{"test", "ValueTest"}});

                StartActivityForResult(intent, ACTION_PAYMENT);
            };

            makeATokenPaymentButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(consumerToken) || string.IsNullOrWhiteSpace(cardToken) || string.IsNullOrWhiteSpace(consumerRef) || string.IsNullOrWhiteSpace(lastFour))
                {
                    Toast.MakeText(that,
                        "Can't make a token payment before making a full card payment to save card token",
                        ToastLength.Short).Show();
                    return;
                }

                var judoId = "100016";
                var currency = "GBP";
                var amount = "4.99";
                var paymentReference = "payment101010102";
                var consumerReference = consumerRef;
                var token = new CardToken()
                {
                    CardLastFour = lastFour,
                    Token = cardToken,
                    ConsumerToken = consumerToken,
                    CardType = cardType
                };

                var intent = JudoSDKManager.UIMethods.TokenPayment(that, judoId, currency, amount, paymentReference, consumerReference, token, null, consumerToken);

                StartActivityForResult(intent, ACTION_PAYMENT_TOKEN);
            };

            makeATokenPreAuthButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(consumerToken) || string.IsNullOrWhiteSpace(cardToken) || string.IsNullOrWhiteSpace(consumerRef) || string.IsNullOrWhiteSpace(lastFour))
                {
                    Toast.MakeText(that,
                        "Can't make a token payment before making a full card payment to save card token",
                        ToastLength.Short).Show();
                    return;
                }

                var judoId = "100016";
                var currency = "GBP";
                var amount = "4.99";
                var paymentReference = "payment101010102";
                var consumerReference = consumerRef;
                var token = new CardToken()
                {
                    CardLastFour = lastFour,
                    Token = cardToken,
                    ConsumerToken = consumerToken,
                    CardType = cardType
                };

                var intent = JudoSDKManager.UIMethods.TokenPreAuth(that, judoId, currency, amount, paymentReference, consumerReference, token, null, consumerToken);

                StartActivityForResult(intent, ACTION_PREAUTH_TOKEN);
            };

            registerCard.Click += (sender, e) =>
            {
                var consumerReference = "consumer1010102";

                var intent = JudoSDKManager.UIMethods.RegisterCard(that, consumerReference);

                StartActivityForResult(intent, ACTION_REGISTER_CARD);
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Receipt receipt = null;

            if (data != null)
            {
                receipt = data.GetParcelableExtra(JudoSDKManager.JUDO_RECEIPT) as Receipt;
            }
            else
            {
                Toast.MakeText(this, "Operation canceled", ToastLength.Long).Show();
                return;
            }

            switch (requestCode)
            {
                case ACTION_PAYMENT:
                    

                    if (resultCode == Result.Ok && receipt.Result != "Declined")
                    {
                        PaymentReceiptModel paymentReceipt;
                        if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null)
                        {
                            cardToken = paymentReceipt.CardDetails.CardToken;
                            consumerToken = paymentReceipt.Consumer.ConsumerToken;
                            consumerRef = paymentReceipt.Consumer.YourConsumerReference;
                            lastFour = paymentReceipt.CardDetails.CardLastfour;
                            cardType = (CardBase.CardType) paymentReceipt.CardDetails.CardType;
                        }
                        
                        Toast.MakeText(this, string.Format("Payment succeeded: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                    }
                    else
                    {
                        if (receipt != null)
                        {
                            Toast.MakeText(this, string.Format("Payment failed: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();    
                        }
                        else
                        {
                            Toast.MakeText(this,
                                string.Format("Error: {0}", data.GetStringExtra(JudoSDKManager.JUDO_ERROR_MESSAGE)), ToastLength.Long).Show();
                        }    
                    }
                    break;
                case ACTION_PAYMENT_TOKEN:
                    if (resultCode == Result.Ok && receipt.Result != "Declined")
                    {
                        Toast.MakeText(this, string.Format("Payment succeeded: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                    }
                    else
                    {
                        if (receipt != null)
                        {
                            Toast.MakeText(this, string.Format("Payment failed: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(this,
                                string.Format("Error: {0}", data.GetStringExtra(JudoSDKManager.JUDO_ERROR_MESSAGE)), ToastLength.Long).Show();
                        }
                    }
                    break;
                case ACTION_PREAUTH_TOKEN:
                    if (resultCode == Result.Ok && receipt.Result != "Declined")
                    {
                        Toast.MakeText(this, string.Format("PreAuth succeeded: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                    }
                    else
                    {
                        if (receipt != null)
                        {
                            Toast.MakeText(this, string.Format("PreAuth failed: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(this,
                                string.Format("Error: {0}", data.GetStringExtra(JudoSDKManager.JUDO_ERROR_MESSAGE)), ToastLength.Long).Show();
                        }
                    }
                    break;
                case ACTION_REGISTER_CARD:
                    if (resultCode == Result.Ok && receipt.Result != "Declined")
                    {
                        PaymentReceiptModel paymentReceipt;
                        if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null)
                        {
                            cardToken = paymentReceipt.CardDetails.CardToken;
                            consumerToken = paymentReceipt.Consumer.ConsumerToken;
                            consumerRef = paymentReceipt.Consumer.YourConsumerReference;
                            lastFour = paymentReceipt.CardDetails.CardLastfour;
                            cardType = (CardBase.CardType)paymentReceipt.CardDetails.CardType;
                        }

                        Toast.MakeText(this, string.Format("Register card succeeded: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                    }
                    else
                    {
                        if (receipt != null)
                        {
                            Toast.MakeText(this, string.Format("Register card failed: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(this,
                                string.Format("Error: {0}", data.GetStringExtra(JudoSDKManager.JUDO_ERROR_MESSAGE)), ToastLength.Long).Show();
                        }
                    }
                    break;

            }
        }
    }
}

