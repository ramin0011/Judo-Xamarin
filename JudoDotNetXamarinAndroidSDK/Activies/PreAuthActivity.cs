using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using JudoDotNetXamarin;
using JudoDotNetXamarinSDK.Models;
using JudoDotNetXamarinSDK.Ui;
using JudoDotNetXamarinSDK.Utils;
using JudoPayDotNet.Models;
using Consumer = JudoDotNetXamarinSDK.Models.Consumer;
using Environment = JudoPayDotNet.Enums.Environment;

namespace JudoDotNetXamarinSDK.Activies
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

            if (JudoSDKManager.IsAVSEnabled)
            {
                var country = avsEntryView.GetCountry();
                cardAddress.PostCode = avsEntryView.GetPostCode();
            }

            string startDate = null;
            string issueNumber = null;

            if (JudoSDKManager.IsMaestroEnabled)
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
                CV2 = cv2
            };

            ShowLoadingSpinner(true);


            var judoPay = JudoSDKManager.JudoClient;

            judoPay.PreAuths.Create(cardPayment).ContinueWith(t =>
            {
                ShowLoadingSpinner(false);

                if (t.IsFaulted || t.Result == null || t.Result.HasError)
                {
                    var errorMessage = t.Result != null ? t.Result.Error.ErrorMessage : t.Exception.Message;
                    Log.Error("com.judopay.android", "ERROR: " + errorMessage);
                    SetResult(JudoSDKManager.JUDO_ERROR, JudoSDKManager.CreateErrorIntent(errorMessage, t.Exception, t.Result != null ? t.Result.Error : null));
                    Finish();
                    return;
                }

                var receipt = t.Result.Response;

                Intent intent = new Intent();
                intent.PutExtra(JudoSDKManager.JUDO_RECEIPT, new Receipt(receipt));
                SetResult(JudoSDKManager.JUDO_SUCCESS, intent);
                Log.Debug("com.judopay.android", "SUCCESS: " + receipt.Result);
                Finish();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}