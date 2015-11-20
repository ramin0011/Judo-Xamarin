using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarinAndroidSDK.Activies
{
    [Activity (Label = "PreAuthTokenActivity")]
    public class PreAuthTokenActivity : PaymentTokenActivity
    {
        private ClientService clientService;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetTitle (Resource.String.title_pre_auth_token);
            FindViewById<Button> (Resource.Id.payButton).Text = Resources.GetString (Resource.String.token_preauth);
            clientService = new ClientService ();
        }

        public override void MakeTokenPayment ()
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

            //   JudoSDKManager.JudoClient.PreAuths.Create(payment).ContinueWith(HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}