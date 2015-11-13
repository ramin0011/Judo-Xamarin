using System.Threading.Tasks;
using Android.OS;
using Android.Widget;
using JudoDotNetXamarinSDK;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarinAndroidSDK.Activies
{
    public class PreAuthActivity : PaymentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetTitle(Resource.String.title_pre_auth);
            FindViewById<Button>(Resource.Id.payButton).Text = Resources.GetString(Resource.String.preauth);
            // Create your application here
        }

        public override void MakeCardPayment()
        {
            var cardNumber = cardEntryView.GetCardNumber();
            var expiryDate = cardEntryView.GetCardExpiry();
            var cv2 = cardEntryView.GetCardCV2();

            CardAddressModel cardAddress = new CardAddressModel();

			if (JudoSDKManagerA.Instance.AVSEnabled)
            {
                var country = avsEntryView.GetCountry();
                cardAddress.PostCode = avsEntryView.GetPostCode();
            }

            string startDate = null;
            string issueNumber = null;

			if (JudoSDKManagerA.Instance.MaestroAccepted)
            {
                issueNumber = startDateEntryView.GetIssueNumber();
                startDate = startDateEntryView.GetStartDate();
            }

            var cardPayment = new CardPaymentModel()
            {
                JudoId = judoId,
                Currency = judoCurrency,
                Amount = judoAmount,
                YourPaymentReference = judoPaymentRef,
                YourConsumerReference = judoConsumer.YourConsumerReference,
                YourPaymentMetaData = judoMetaData.Metadata,
                CardNumber = cardNumber,
                CardAddress = cardAddress,
                StartDate = startDate,
                ExpiryDate = expiryDate,
                CV2 = cv2,
                ClientDetails = JudoSDKManagerA.GetClientDetails(this),
				UserAgent = JudoSDKManagerA.GetSDKVersion()
            };

            ShowLoadingSpinner(true);


			var judoPay = JudoSDKManagerA.JudoClient;

            judoPay.PreAuths.Create(cardPayment).ContinueWith(HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}