using System.Threading.Tasks;
using Android.OS;
using Android.Widget;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;

namespace JudoDotNetXamarinAndroidSDK.Activies
{
    public class PreAuthActivity : PaymentActivity
    {
        private ClientService clientService;
        IPaymentService _paymentService;
        ServiceFactory factory;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetTitle (Resource.String.title_pre_auth);
            FindViewById<Button> (Resource.Id.payButton).Text = Resources.GetString (Resource.String.preauth);
            clientService = new ClientService ();
            factory = new ServiceFactory ();
            _paymentService = factory.GetPaymentService (); 
            // Create your application here
        }

        public override void MakeCardPayment ()
        {
//            var cardNumber = cardEntryView.GetCardNumber ();
//            var expiryDate = cardEntryView.GetCardExpiry ();
//            var cv2 = cardEntryView.GetCardCV2 ();
//
//            CardAddressModel cardAddress = new CardAddressModel ();
//
//            if (JudoSDKManager.AVSEnabled) {
//                var country = avsEntryView.GetCountry ();
//                cardAddress.PostCode = avsEntryView.GetPostCode ();
//            }
//
//            string startDate = null;
//            string issueNumber = null;
//
//            if (JudoSDKManager.MaestroAccepted) {
//                issueNumber = startDateEntryView.GetIssueNumber ();
//                startDate = startDateEntryView.GetStartDate ();
//            }
//
//            var cardPayment = new CardPaymentModel () {
//                JudoId = judoId,
//                Currency = judoCurrency,
//                Amount = judoAmount,
//                YourPaymentReference = judoPaymentRef,
//                YourConsumerReference = judoConsumer.YourConsumerReference,
//                YourPaymentMetaData = judoMetaData.Metadata,
//                CardNumber = cardNumber,
//                CardAddress = cardAddress,
//                StartDate = startDate,
//                ExpiryDate = expiryDate,
//                CV2 = cv2,
//                ClientDetails = clientService.GetClientDetails (),
//                UserAgent = clientService.GetSDKVersion ()
//            };
//
            ShowLoadingSpinner (true);
            PaymentViewModel cardPayment = new PaymentViewModel ();
            cardPayment.Card = GatherCardDetails ();
            cardPayment.Currency = judoCurrency;
            cardPayment.Amount = judoAmount;
            cardPayment.PaymentReference = judoPaymentRef;
            cardPayment.ConsumerReference = judoConsumer.YourConsumerReference;
            if (judoMetaData != null) {
                cardPayment.YourPaymentMetaData = judoMetaData.Metadata;
            }



            _paymentService.PreAuthoriseCard (cardPayment, clientService).ContinueWith (HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext ());
            //                var result = reponse.Result;
            //judoPay.PreAuths.Create(cardPayment).ContinueWith(HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext());
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