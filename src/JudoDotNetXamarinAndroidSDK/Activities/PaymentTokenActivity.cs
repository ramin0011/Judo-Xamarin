using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using JudoDotNetXamarinAndroidSDK.Ui;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;
using Newtonsoft.Json;

namespace JudoDotNetXamarinAndroidSDK.Activities
{
    [Activity (Label = "PaymentTokenActivity")]
    public class PaymentTokenActivity : BaseActivity
    {

       
        protected decimal judoAmount;
        protected string judoId;
        protected string judoCurrency;
        protected CardToken judoCardToken;
        protected Consumer judoConsumer;
        protected CV2EntryView cv2EntryView;
        private ClientService clientService;
        IPaymentService _paymentService;
        ServiceFactory factory;

        Button payButton;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.token_payment);

            SetResources ();

            UnbundleIntent ();

            WireUpUI ();

            SetUpDelegates ();

            cv2EntryView.SetCardDetails (judoCardToken);

            clientService = new ClientService ();
            factory = new ServiceFactory ();
            _paymentService = factory.GetPaymentService (); 

           
            if (bundle != null) {
                RestoreState (bundle);
            }
        }

        void SetUpDelegates ()
        {
            cv2EntryView.NoLongerComplete += () => {
                payButton.Enabled = false;
            };
            cv2EntryView.OnCreditCardEntered += (creditCardNumber) => {
                payButton.Enabled = true;
            };

        }

        void WireUpUI ()
        {

            payButton = FindViewById<Button> (Resource.Id.payButton);
            payButton.Enabled = false;

            payButton.Text = Resources.GetString (Resource.String.token_payment);
            payButton.Click += (sender, args) => TransactClickHandler (MakeTokenPayment);
        }

        void UnbundleIntent ()
        {

            judoAmount = decimal.Parse (Intent.GetStringExtra (Judo.JUDO_AMOUNT));
            judoId = Intent.GetStringExtra (Judo.JUDO_ID);
            judoCurrency = Intent.GetStringExtra (Judo.JUDO_CURRENCY);
            judoCardToken = JsonConvert.DeserializeObject<CardToken> (Intent.GetStringExtra (Judo.JUDO_CARD_DETAILS));
            judoConsumer = JsonConvert.DeserializeObject<Consumer> (Intent.GetStringExtra (Judo.JUDO_CONSUMER));
     
            if (judoCardToken.CardType != null) {
                cv2EntryView.CurrentCard = judoCardToken.CardType;  
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
        }

        void SetResources ()
        {
            SetTitle (Resource.String.title_payment_token);
            cv2EntryView = FindViewById<CV2EntryView> (Resource.Id.cv2EntryView);

            SetHelpText (Resource.String.help_info, Resource.String.help_cv2_text);
            SetHelpText (Resource.String.help_postcode_title, Resource.String.help_postcode_text, Resource.Id.postCodeHelpButton);
        }

        public override void OnBackPressed ()
        {
            SetResult (Judo.JUDO_CANCELLED);
            base.OnBackPressed ();
        }

        public virtual void MakeTokenPayment ()
        {
            TokenPaymentViewModel payment = new TokenPaymentViewModel () {
                Currency = judoCurrency,
                Amount = judoAmount,
                ConsumerToken = judoConsumer.ConsumerToken,
                JudoID = judoId,
                CardType = judoCardToken.CardType,
                Token = judoCardToken.Token,
                ConsumerReference = judoConsumer.YourConsumerReference,
                CV2 = cv2EntryView.GetCV2 ()

            };

            ShowLoadingSpinner (true);
          
            _paymentService.MakeTokenPayment (payment, new ClientService ()).ContinueWith (HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext ());
        }


        protected override void OnSaveInstanceState (Bundle outState)
        {
            var expiryDate = cv2EntryView.GetExpiry ();
            var cv2 = cv2EntryView.GetCV2 ();
          
            outState.PutString ("EXPIRYDATE", expiryDate);
            outState.PutString ("CV2", cv2);

            base.OnSaveInstanceState (outState);    
        }

        void RestoreState (Bundle bundle)
        {
            var expiry = bundle.GetString ("EXPIRYDATE", "");
            var cv2 = bundle.GetString ("CV2", "");
            cv2EntryView.RestoreState (expiry, cv2);

        }
    }
}