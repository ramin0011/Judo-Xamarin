using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinAndroidSDK.Ui;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarinAndroidSDK.Activies
{
    [Activity (Label = "PaymentTokenActivity")]
    public class PaymentTokenActivity : BaseActivity
    {

        protected string judoPaymentRef;
        protected decimal judoAmount;
        protected string judoId;
        protected string judoCurrency;
        protected MetaData judoMetaData;
        protected CardToken judoCardToken;
        protected Models.Consumer judoConsumer;
        protected CV2EntryView cv2EntryView;
        private ClientService clientService;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.token_payment);
            SetTitle (Resource.String.title_payment_token);
            cv2EntryView = FindViewById<CV2EntryView> (Resource.Id.cv2EntryView);

            SetHelpText (Resource.String.help_info, Resource.String.help_cv2_text);
            SetHelpText (Resource.String.help_postcode_title, Resource.String.help_postcode_text, Resource.Id.postCodeHelpButton);

            judoPaymentRef = Intent.GetStringExtra (JudoSDKManager.JUDO_PAYMENT_REF);

            judoAmount = decimal.Parse (Intent.GetStringExtra (JudoSDKManager.JUDO_AMOUNT));
            judoId = Intent.GetStringExtra (JudoSDKManager.JUDO_ID);
            judoCurrency = Intent.GetStringExtra (JudoSDKManager.JUDO_CURRENCY);
            judoCardToken = Intent.GetParcelableExtra (JudoSDKManager.JUDO_CARD_DETAILS).JavaCast<CardToken> ();
            judoConsumer = Intent.GetParcelableExtra (JudoSDKManager.JUDO_CONSUMER).JavaCast<Models.Consumer> ();

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
            if (judoCardToken == null) {
                throw new ArgumentException ("JUDO_CARD_DETAILS must be supplied");
            }

            cv2EntryView.SetCardDetails (judoCardToken);

            judoMetaData = Intent.GetParcelableExtra (JudoSDKManager.JUDO_META_DATA).JavaCast<MetaData> ();

            var payButton = FindViewById<Button> (Resource.Id.payButton);

            payButton.Text = Resources.GetString (Resource.String.token_payment);
            payButton.Click += (sender, args) => TransactClickHandler (MakeTokenPayment);
            clientService = new ClientService ();
        }

        public override void OnBackPressed ()
        {
            SetResult (JudoSDKManager.JUDO_CANCELLED);
            base.OnBackPressed ();
        }

        public virtual void MakeTokenPayment ()
        {
            TokenPaymentModel payment = new TokenPaymentModel () {
                JudoId = judoId,
                Currency = judoCurrency,
                Amount = judoAmount,
                YourPaymentReference = judoPaymentRef,
                ConsumerToken = judoConsumer.ConsumerToken,
                YourConsumerReference = judoConsumer.YourConsumerReference,
                YourPaymentMetaData = judoMetaData.Metadata,
                CardToken = judoCardToken.Token,
                CV2 = cv2EntryView.GetCV2 (),
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion ()
            };

            ShowLoadingSpinner (true);

            // JudoSDKManager.JudoClient.Payments.Create(payment).ContinueWith(HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected override void ShowLoadingSpinner (bool show)
        {
            RunOnUiThread (() => {
                ((InputMethodManager)GetSystemService (Context.InputMethodService)).HideSoftInputFromWindow (
                    FindViewById (Resource.Id.payButton).WindowToken, 0);
                FindViewById (Resource.Id.loadingLayout).Visibility = show ? ViewStates.Visible : ViewStates.Gone;

            });
        }
    }
}