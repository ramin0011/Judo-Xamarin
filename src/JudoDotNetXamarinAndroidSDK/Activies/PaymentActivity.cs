using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinAndroidSDK.Ui;
using JudoDotNetXamarinAndroidSDK.Utils;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;

namespace JudoDotNetXamarinAndroidSDK.Activies
{
    public class PaymentActivity : BaseActivity
    {
        protected string judoPaymentRef;
        protected decimal judoAmount;
        protected string judoId;
        protected string judoCurrency;
        protected MetaData judoMetaData;
        protected CardEntryView cardEntryView;
        protected Models.Consumer judoConsumer;
        protected AVSEntryView avsEntryView;
        protected HelpButton cv2ExpiryHelpInfoButton;
        protected StartDateIssueNumberEntryView startDateEntryView;
        //		protected JudoSuccessCallback successCallback;
        //		protected JudoFailureCallback failureCallback;
        IPaymentService _paymentService;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.payment);
            Title = GetString (Resource.String.title_payment);
            cardEntryView = FindViewById<CardEntryView> (Resource.Id.cardEntryView);
            TextView hintTextView = FindViewById<TextView> (Resource.Id.hintTextView);
            cardEntryView.HintTextView = hintTextView;
            avsEntryView = FindViewById<AVSEntryView> (Resource.Id.avsEntryView);
            startDateEntryView = FindViewById<StartDateIssueNumberEntryView> (Resource.Id.startDateEntryView);

            cv2ExpiryHelpInfoButton = FindViewById<HelpButton> (Resource.Id.infoButtonID);
            cv2ExpiryHelpInfoButton.Visibility = ViewStates.Gone;

            SetHelpText (Resource.String.help_info, Resource.String.help_card_text);
            SetHelpText (Resource.String.help_postcode_title, Resource.String.help_postcode_text,
                Resource.Id.postCodeHelpButton);

            judoPaymentRef = Intent.GetStringExtra (JudoSDKManagerA.JUDO_PAYMENT_REF);
            judoConsumer = Intent.GetParcelableExtra (JudoSDKManagerA.JUDO_CONSUMER).JavaCast<Models.Consumer> ();

            judoAmount = decimal.Parse (Intent.GetStringExtra (JudoSDKManagerA.JUDO_AMOUNT));
            judoId = Intent.GetStringExtra (JudoSDKManagerA.JUDO_ID);
            judoCurrency = Intent.GetStringExtra (JudoSDKManagerA.JUDO_CURRENCY);

            if (judoPaymentRef == null) {
                throw new ArgumentException ("JUDO_PAYMENT_REF must be supplied");
            }
            if (judoConsumer == null) {
                throw new ArgumentException ("JUDO_CONSUMER must be supplied");
            }
            if (judoAmount == null) {
                throw new ArgumentException ("JUDO_AMOUNT must be supplied");
            } 
            if (judoId == null) {
                throw new ArgumentException ("JUDO_ID must be supplied");
            }
            if (judoCurrency == null) {
                throw new ArgumentException ("JUDO_CURRENCY must be supplied");
            }

            judoMetaData = Intent.Extras.GetParcelable (JudoSDKManagerA.JUDO_META_DATA).JavaCast<MetaData> ();

            var payButton = FindViewById<Button> (Resource.Id.payButton);

            payButton.Text = Resources.GetString (Resource.String.payment);

            payButton.Click += (sender, args) => TransactClickHandler (MakeCardPayment);

            cardEntryView.OnCreditCardEntered += cardNumber => {
                cv2ExpiryHelpInfoButton.Visibility = ViewStates.Visible;
            };

            cardEntryView.OnExpireAndCV2Entered += (expiryDate, cv2) => {
                string cardNumber = null;

                try {
                    cardNumber = cardEntryView.GetCardNumber ();
                } catch (Exception e) {
                    Console.Error.Write (e.StackTrace);
                }

                if (ValidationHelper.IsStartDateRequiredForCardNumber (cardNumber) && JudoSDKManagerA.Instance.MaestroAccepted) {
                    startDateEntryView.Visibility = ViewStates.Visible;
                    startDateEntryView.RequestFocus ();
                    avsEntryView.InhibitFocusOnFirstShowOfCountrySpinner ();
                }

                if (JudoSDKManagerA.Instance.AVSEnabled && avsEntryView != null) {
                    avsEntryView.Visibility = ViewStates.Visible;
                }
            };

            cardEntryView.OnReturnToCreditCardNumberEntry += () => {
                cv2ExpiryHelpInfoButton.Visibility = ViewStates.Gone;
            };
        }

        public override void OnBackPressed ()
        {
            SetResult (JudoSDKManagerA.JUDO_CANCELLED);
            base.OnBackPressed ();
        }

        public virtual void MakeCardPayment ()
        {
            PaymentViewModel cardPayment = new PaymentViewModel ();
            cardPayment.Card = GatherCardDetails ();
            cardPayment.Currency = judoCurrency;
            cardPayment.Amount = judoAmount;
            cardPayment.PaymentReference = judoPaymentRef;
            cardPayment.ConsumerReference = judoConsumer.YourConsumerReference;
            cardPayment.YourPaymentMetaData = judoMetaData.Metadata;


            ShowLoadingSpinner (true);

            _paymentService.MakePayment (cardPayment, new ClientService ()).ContinueWith (reponse => {
                var result = reponse.Result;

                if (result != null && !result.HasError && result.Response.Result != "Declined") {
                    var paymentreceipt = result.Response as PaymentReceiptModel;

                    if (paymentreceipt != null) {
                        // call success callback
//                        if (successCallback != null)
//                            successCallback (paymentreceipt);
                    } else {
                        var threedDSecureReceipt = result.Response as PaymentRequiresThreeDSecureModel;
                        if (threedDSecureReceipt != null) {
//                            failureCallback (new JudoError { ApiError = new JudoPayDotNet.Errors.JudoApiErrorModel {
//                                    ErrorMessage = "Account requires 3D Secure but application is not configured to accept it",
//                                    ErrorType = JudoApiError.General_Error,
//                                    ModelErrors = null
//                                } });
                        } else {
                            throw new Exception ("JudoXamarinSDK: unable to find the receipt in response.");
                        }
                    }

                } else {
//                    // Failure callback
//                    if (failureCallback != null) {
//                        var judoError = new JudoError { ApiError = result != null ? result.Error : null };
//                        var paymentreceipt = result != null ? result.Response as PaymentReceiptModel : null;
//
//                        if (paymentreceipt != null) {
//                            // send receipt even we got card declined
//
//                            failureCallback (judoError, paymentreceipt);
//                        } else {
//
//                            failureCallback (judoError);
//                        }
                    //  }
                }

                ShowLoadingSpinner (false);
            });
            //var judoPay = JudoSDKManagerA.JudoClient;
            //judoPay.Payments.Create(cardPayment).ContinueWith(HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected override void ShowLoadingSpinner (bool show)
        {
            RunOnUiThread (() => {
                ((InputMethodManager)GetSystemService (Context.InputMethodService)).HideSoftInputFromWindow (
                    FindViewById (Resource.Id.payButton).WindowToken, 0);
                FindViewById (Resource.Id.loadingLayout).Visibility = show ? ViewStates.Visible : ViewStates.Gone;

            });
        }

        CardViewModel GatherCardDetails ()
        {
            var cardNumber = cardEntryView.GetCardNumber ();
            var expiryDate = cardEntryView.GetCardExpiry ();
            var cv2 = cardEntryView.GetCardCV2 ();
            BillingCountryOptions country = BillingCountryOptions.BillingCountryOptionUK;
            CardAddressModel cardAddress = new CardAddressModel ();

            if (JudoSDKManagerA.Instance.MaestroAccepted) {
                country = avsEntryView.GetCountry ();
                cardAddress.PostCode = avsEntryView.GetPostCode ();
            }

            string startDate = null;
            string issueNumber = null;

            if (JudoSDKManagerA.Instance.MaestroAccepted) {
                issueNumber = startDateEntryView.GetIssueNumber ();
                startDate = startDateEntryView.GetStartDate ();
            }

		
            var cardPayment = new CardViewModel () {
                CardNumber = cardNumber,
                CountryCode = country.GetISOCode (),
                CV2 = cv2,
                ExpireDate = expiryDate,
                IssueNumber = issueNumber,
                StartDate = startDate,
                PostCode = cardAddress.PostCode,	
            };

            return cardPayment;
        }
    }
}