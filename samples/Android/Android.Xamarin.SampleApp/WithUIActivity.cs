using Android.App;
using Android.OS;
using JudoDotNetXamarin;
using JudoPayDotNet.Enums;
using System;
using Android.Widget;
using JudoPayDotNet.Models;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinAndroidSDK;
using JudoDotNetXamarin.Models;
using Android.Content;

namespace Android.Xamarin.SampleApp
{
    [Activity (Label = "@string/app_name_ui", MainLauncher = true, Icon = "@drawable/ic_app_icon")]
    public class WithUIActivity : Activity
    {
        // Configure your JudoID and payment detail
        private const string ApiToken = "[Application ApiToken]";
        //retrieve from JudoPortal
        private const string ApiSecret = "[Application ApiSecret]";
        //retrieve from JudoPortal
        private string MY_JUDO_ID = "[Judo ID]";
        //Received when registering an account with Judo
        private string currency = "GBP";
        private string amount = "1.99";
        //        private string paymentReference = "payment101010102";
        //        private string consumerRef      = "consumer1010102";

        private string paymentReference = "payment101010102";
        private string consumerRef = "consumer1010102";
        private const string cardNumber = "4976000000003436";
        private const string addressPostCode = "TR14 8PA";
        private const string startDate = "";
        private  const string expiryDate = "12/15";
        private const string cv2 = "452";

        private const int ACTION_CARD_PAYMENT = 101;
        private const int ACTION_TOKEN_PAYMENT = 102;
        private const int ACTION_PREAUTH = 201;
        private const int ACTION_TOKEN_PREAUTH = 202;
        private const int ACTION_REGISTER_CARD = 301;

        private volatile string cardToken;
        private volatile string consumerToken;
        private volatile string rcp_consumerRef;
        private volatile string lastFour;
        private volatile JudoPayDotNet.Models.CardType cardType;

        private volatile string preAuth_cardToken;
        private volatile string preAuth_consumerToken;
        private volatile string preAuth_rcp_consumerRef;
        private volatile string preAuth_lastFour;
        private volatile JudoPayDotNet.Models.CardType preAuth_cardType;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.withui);

            // setting up API token/secret 
            var configInstance = JudoConfiguration.Instance;

            //setting for Sandnox
            configInstance.Environment = JudoEnvironment.Live;


            /*
			configInstance.ApiToken = "[Application ApiToken]"; //retrieve from JudoPortal
			configInstance.ApiSecret = "[Application ApiSecret]"; //retrieve from JudoPortal
			configInstance.JudoId = "[Judo ID]"; //Received when registering an account with Judo
			*/
            // //Salatha
            configInstance.ApiToken = "MzEtkQK1bHi8v8qy";
            configInstance.ApiSecret = "c158b4997dfc7595a149a20852f7af2ea2e70bd2df794b8bdbc019cc5f799aa1";
            configInstance.JudoId = "100915867";
            if (configInstance.ApiToken == null) {
                throw(new Exception ("Judo Configuration settings have not been set on the config Instance.i.e JudoID Token,Secret"));
            }

            // Get our button from the layout resource,
            // and attach an event to it
            Button payCard = FindViewById<Button> (Resource.Id.payCard);
            Button payToken = FindViewById<Button> (Resource.Id.payToken);
            Button payPreAuth = FindViewById<Button> (Resource.Id.payPreAuth);
            Button payTokenPreAuth = FindViewById<Button> (Resource.Id.payTokenPreAuth);
            Button registerCard = FindViewById<Button> (Resource.Id.registerCard);
            Button nonUiExamples = FindViewById<Button> (Resource.Id.nonUiExamples);

            // Assigning click delegates
            payCard.Click += new EventHandler (payCard_Click);
            payToken.Click += new EventHandler (payToken_Click);
            payPreAuth.Click += new EventHandler (payPreAuth_Click);
            payTokenPreAuth.Click += new EventHandler (payTokenPreAuth_Click);
            registerCard.Click += new EventHandler (registerCard_Click);
            nonUiExamples.Click += new EventHandler (nonUiExamples_Click);

            FindViewById<TextView> (Resource.Id.sdk_version_label).Text = "";
        }

        private void SuccessPayment (PaymentReceiptModel receipt)
        {
//			cardToken = receipt.CardDetails.CardToken;
//			consumerToken = receipt.Consumer.ConsumerToken;
//			lastFour = receipt.CardDetails.CardLastfour;
//			cardType = receipt.CardDetails.CardType;
//			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
//				// move back to home screen
//				CloseView ();
//
//
//				// show receipt
//				ShowMessage ("Transaction Successful", "Receipt ID - " + receipt.ReceiptId);
//
//				// store token to further use
//			});
        }

        private void FailurePayment (JudoError error, PaymentReceiptModel receipt)
        {
//			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
//				// move back to home screen
//				CloseView ();
//				// show receipt
//				string message = "";
//				if (error != null && error.ApiError != null)
//					message += error.ApiError.ErrorMessage + Environment.NewLine;
//
//				if (error != null && error.Exception != null)
//					message += error.Exception.Message + Environment.NewLine;
//
//				if (receipt != null) {
//					message += "Transaction : " + receipt.Result + Environment.NewLine;
//					message += receipt.Message + Environment.NewLine;
//					message += "Receipt ID - " + receipt.ReceiptId;
//				}
//
//				ShowMessage ("Transaction Failed: ", message);
//				// store token to further use
//			});
        }

        private void payCard_Click (object sender, EventArgs e)
        {
            var cardModel = GetCardViewModel ();

            // Optional: Supply meta data about this transaction, pass as last argument instead of null.
            // Dictionary<string, string> metaData = new Dictionary<string, string>{{"test1", "test2"}};

            JudoSDKManager.AmExAccepted = true;
            JudoSDKManager.Instance.Payment (cardModel, SuccessPayment, FailurePayment, this);


            //var intent = JudoSDKManagerA.UIMethods.Payment( MY_JUDO_ID, currency, amount, paymentReference, consumerRef, metaData);

            //StartActivityForResult(intent, ACTION_CARD_PAYMENT);
        }

        private void payPreAuth_Click (object sender, EventArgs e)
        {
            JudoSDKManagerA.Instance.Payment (GetCardViewModel (), SuccessPayment, FailurePayment);

            // Optional: Supply meta data about this transaction, pass as last argument instead of null.
            //  Intent intent = JudoSDKManagerA.UIMethods.PreAuth(this, MY_JUDO_ID, currency, amount, paymentReference, consumerRef, null);

            //  StartActivityForResult(intent, ACTION_PREAUTH);
        }

        private void payToken_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace (consumerToken) || string.IsNullOrWhiteSpace (cardToken)
                || string.IsNullOrWhiteSpace (rcp_consumerRef) || string.IsNullOrWhiteSpace (lastFour)) {
                Toast.MakeText (this,
                    "Can't make a token payment before making a full card payment to save card token",
                    ToastLength.Short).Show ();
                return;
            }

            var consumerReference = rcp_consumerRef;
            var token = new CardToken () {
                CardLastFour = lastFour,
                Token = cardToken,
                ConsumerToken = consumerToken,
                CardType = cardType
            };

            // Optional: Supply meta data about this transaction, pass as last argument instead of null.
            // var intent = JudoSDKManagerA.UIMethods.TokenPayment(this, MY_JUDO_ID, currency, amount, paymentReference, consumerReference, token, null, consumerToken);

            // StartActivityForResult(intent, ACTION_TOKEN_PAYMENT);

        }

        private void payTokenPreAuth_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace (preAuth_consumerToken) || string.IsNullOrWhiteSpace (preAuth_cardToken)
                || string.IsNullOrWhiteSpace (preAuth_rcp_consumerRef) || string.IsNullOrWhiteSpace (preAuth_lastFour)) {
                Toast.MakeText (this,
                    "Can't make a Preauth token payment before making a full preauth card payment to save card token",
                    ToastLength.Short).Show ();
                return;
            }

            var consumerReference = preAuth_rcp_consumerRef;
            var token = new CardToken () {
                CardLastFour = preAuth_lastFour,
                Token = preAuth_cardToken,
                ConsumerToken = preAuth_consumerToken,
                CardType = preAuth_cardType
            };

            // Optional: Supply meta data about this transaction, pass as last argument instead of null.
            //  var intent = JudoSDKManagerA.UIMethods.TokenPreAuth(this, MY_JUDO_ID, currency, amount, paymentReference, consumerReference, token, null, preAuth_consumerToken);

            //  StartActivityForResult(intent, ACTION_TOKEN_PREAUTH);
        }

        private void registerCard_Click (object sender, EventArgs e)
        {
            // var intent = JudoSDKManagerA.UIMethods.RegisterCard(this, consumerRef);

            //   StartActivityForResult(intent, ACTION_REGISTER_CARD);
        }

        private void nonUiExamples_Click (object sender, EventArgs e)
        {
            StartActivity (typeof(WithoutUIActivity));
        }

        private void UpdateCount ()
        {
        }

        protected override void OnActivityResult (int requestCode, Android.App.Result resultCode, Intent data)
        {
            
        }

        //        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //        {
        //            Receipt receipt = null;
        //            string msg_prefix = "";
        //
        //            if (resultCode == Result.Canceled)
        //            {
        //                Toast.MakeText(this, "Payment Canceled.", ToastLength.Long).Show();
        //                return;
        //            }
        //            else if (resultCode == JudoSDKManagerA.JUDO_ERROR)
        //            {
        //                Error err =  data.GetParcelableExtra(JudoSDKManagerA.JUDO_ERROR_EXCEPTION) as Error;
        //
        //                Toast.MakeText(this, string.Format("Error: {0} {1}", data.GetStringExtra(JudoSDKManagerA.JUDO_ERROR_MESSAGE),
        //                    err != null && err.Exception != null ? "\r\nException: " + err.Exception.Message : ""), ToastLength.Long).Show();
        //                return;
        //            }
        //
        //            if(data != null)
        //                receipt = data.GetParcelableExtra(JudoSDKManagerA.JUDO_RECEIPT) as Receipt;
        //
        //            if (receipt == null)
        //            {
        //                Toast.MakeText(this, string.Format("Error: {0}", data.GetStringExtra(JudoSDKManagerA.JUDO_ERROR_MESSAGE)), ToastLength.Long).Show();
        //                return;
        //            }
        //
        //            switch (requestCode)
        //            {
        //                case ACTION_CARD_PAYMENT:
        //                    if (resultCode == Result.Ok && receipt.Result != "Declined")
        //                    {
        //                        PaymentReceiptModel paymentReceipt;
        //                        if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null)
        //                        {
        //                            cardToken = paymentReceipt.CardDetails.CardToken;
        //                            consumerToken = paymentReceipt.Consumer.ConsumerToken;
        //                            rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
        //                            lastFour = paymentReceipt.CardDetails.CardLastfour;
        //                            cardType = (CardType)paymentReceipt.CardDetails.CardType;
        //                        }
        //                        msg_prefix = "Payment succeeded";
        //                    }
        //                    else
        //                    {
        //                        msg_prefix = "Payment failed";
        //                    }
        //                    break;
        //                case ACTION_PREAUTH:
        //                    if (resultCode == Result.Ok && receipt.Result != "Declined")
        //                    {
        //                        PaymentReceiptModel paymentReceipt;
        //                        if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null)
        //                        {
        //                            preAuth_cardToken = paymentReceipt.CardDetails.CardToken;
        //                            preAuth_consumerToken = paymentReceipt.Consumer.ConsumerToken;
        //                            preAuth_rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
        //                            preAuth_lastFour = paymentReceipt.CardDetails.CardLastfour;
        //                            preAuth_cardType = (CardType)paymentReceipt.CardDetails.CardType;
        //                        }
        //
        //                        msg_prefix = "PreAuth card payment succeeded";
        //                    }
        //                    else
        //                    {
        //                        msg_prefix = "PreAuth card payment failed";
        //                    }
        //                    break;
        //                case ACTION_TOKEN_PAYMENT:
        //                    if (resultCode == Result.Ok && receipt.Result != "Declined")
        //                    {
        //                        msg_prefix = "Token payment succeeded";
        //                    }
        //                    else
        //                    {
        //                        msg_prefix = "Token payment failed";
        //                    }
        //                    break;
        //                case ACTION_TOKEN_PREAUTH:
        //                    if (resultCode == Result.Ok && receipt.Result != "Declined")
        //                    {
        //                        msg_prefix = "PreAuth Token payment succeeded";
        //                    }
        //                    else
        //                    {
        //                        msg_prefix = "PreAuth Token payment failed";
        //                    }
        //                    break;
        //                case ACTION_REGISTER_CARD:
        //                    if (resultCode == Result.Ok && receipt.Result != "Declined")
        //                    {
        //                        PaymentReceiptModel paymentReceipt;
        //                        if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null)
        //                        {
        //                            cardToken = paymentReceipt.CardDetails.CardToken;
        //                            consumerToken = paymentReceipt.Consumer.ConsumerToken;
        //                            rcp_consumerRef = paymentReceipt.Consumer.YourConsumerReference;
        //                            lastFour = paymentReceipt.CardDetails.CardLastfour;
        //                            cardType = (CardType)paymentReceipt.CardDetails.CardType;
        //                        }
        //
        //                        msg_prefix = "Register card succeeded";
        //                    }
        //                    else
        //                    {
        //                        msg_prefix = "Register card failed";
        //                    }
        //                    break;
        //            }
        //
        //            if (receipt != null)
        //            {
        //                Toast.MakeText(this, string.Format("{0}: id: {1},\r\nMessage: {2},\r\nresult: {3}", msg_prefix, receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
        //            }
        //
        //        }

        private PaymentViewModel GetCardViewModel ()
        {
            var cardPayment = new PaymentViewModel {
                Amount = 4.5m, 
                ConsumerReference = consumerRef,
                PaymentReference = paymentReference,
                Currency = "GBP",
                // Non-UI API needs to pass card detail
                Card = new CardViewModel {
                    CardNumber = cardNumber,
                    CV2 = cv2,
                    ExpireDate = expiryDate,
                    PostCode = addressPostCode,
                    CountryCode = ISO3166CountryCodes.UK
                }
            };
            return cardPayment;
        }
    }
}

