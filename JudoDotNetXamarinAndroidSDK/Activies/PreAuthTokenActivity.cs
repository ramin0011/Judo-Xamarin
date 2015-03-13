using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using JudoDotNetXamarinSDK.Models;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarinSDK.Activies
{
    [Activity(Label = "PreAuthTokenActivity")]
    public class PreAuthTokenActivity : PaymentTokenActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetTitle(Resource.String.title_pre_auth_token);
            FindViewById<Button>(Resource.Id.payButton).Text = Resources.GetString(Resource.String.token_preauth);
        }

        public override void MakeTokenPayment()
        {
            TokenPaymentModel payment = new TokenPaymentModel()
            {
                JudoId = judoId,
                Currency = judoCurrency,
                Amount = judoAmount,
                YourPaymentReference = judoPaymentRef,
                ConsumerToken = judoConsumer.ConsumerToken,
                YourConsumerReference = judoConsumer.YourConsumerReference,
                YourPaymentMetaData = judoMetaData.Metadata,
                CardToken = judoCardToken.Token,
                CV2 = cv2EntryView.GetCV2(),

            };

            ShowLoadingSpinner(true);

            JudoSDKManager.JudoClient.PreAuths.Create(payment).ContinueWith(t =>
            {
                ShowLoadingSpinner(false);

                if (t.IsFaulted || t.Result == null || t.Result.HasError)
                {
                    var errorMessage = t.Result != null ? t.Result.Error.ErrorMessage : t.Exception.ToString();
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