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
        protected Models.SConsumer judoConsumer;
        protected AVSEntryView avsEntryView;
        protected HelpButton cv2ExpiryHelpInfoButton;
        protected StartDateIssueNumberEntryView startDateEntryView;
        IPaymentService _paymentService;
        ServiceFactory factory;

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

            judoPaymentRef = Intent.GetStringExtra (JudoSDKManager.JUDO_PAYMENT_REF);
            judoConsumer = Intent.GetParcelableExtra (JudoSDKManager.JUDO_CONSUMER).JavaCast<Models.SConsumer> ();

            judoAmount = decimal.Parse (Intent.GetStringExtra (JudoSDKManager.JUDO_AMOUNT));
            judoId = Intent.GetStringExtra (JudoSDKManager.JUDO_ID);
            judoCurrency = Intent.GetStringExtra (JudoSDKManager.JUDO_CURRENCY);

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

            judoMetaData = Intent.Extras.GetParcelable (JudoSDKManager.JUDO_META_DATA).JavaCast<MetaData> ();

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

                if (ValidationHelper.IsStartDateRequiredForCardNumber (cardNumber) && JudoSDKManager.MaestroAccepted) {
                    startDateEntryView.Visibility = ViewStates.Visible;
                    startDateEntryView.RequestFocus ();
                    avsEntryView.InhibitFocusOnFirstShowOfCountrySpinner ();
                }

                if (JudoSDKManager.AVSEnabled && avsEntryView != null) {
                    avsEntryView.Visibility = ViewStates.Visible;
                }
            };

            cardEntryView.OnReturnToCreditCardNumberEntry += () => {
                cv2ExpiryHelpInfoButton.Visibility = ViewStates.Gone;
            };

            factory = new ServiceFactory ();
            _paymentService = factory.GetPaymentService (); 
        }

        public override void OnBackPressed ()
        {
            SetResult (JudoSDKManager.JUDO_CANCELLED);
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
            if (judoMetaData != null) {
                cardPayment.YourPaymentMetaData = judoMetaData.Metadata;
            }

            ShowLoadingSpinner (true);

            _paymentService.MakePayment (cardPayment, new ClientService ()).ContinueWith (HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext ());

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

            if (JudoSDKManager.MaestroAccepted) {
                country = avsEntryView.GetCountry ();
                cardAddress.PostCode = avsEntryView.GetPostCode ();
            }

            string startDate = null;
            string issueNumber = null;

            if (JudoSDKManager.MaestroAccepted) {
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