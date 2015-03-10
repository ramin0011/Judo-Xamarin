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
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using JudoDotNetXamarinSDK.Models;
using JudoDotNetXamarinSDK.Ui;
using JudoPayDotNet.Models;
using Consumer = JudoDotNetXamarinSDK.Models.Consumer;
using Exception = System.Exception;

namespace JudoDotNetXamarinSDK.Activies
{
    [Activity(Label = "PaymentTokenActivity")]
    public class PaymentTokenActivity : BaseActivity
    {

        protected string judoPaymentRef;
        protected decimal judoAmount;
        protected string judoId;
        protected string judoCurrency;
        protected Bundle judoMetaData;
        protected CardToken judoCardToken;
        protected Consumer judoConsumer;
        protected CV2EntryView cv2EntryView;
    

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.token_payment);
            SetTitle(Resource.String.title_payment_token);
            cv2EntryView = FindViewById<CV2EntryView>(Resource.Id.cv2EntryView);

            SetHelpText(Resource.String.help_info, Resource.String.help_cv2_text);
            SetHelpText(Resource.String.help_postcode_title, Resource.String.help_postcode_text, Resource.Id.postCodeHelpButton);

            judoPaymentRef = Intent.GetStringExtra(JudoSDKManager.JUDO_PAYMENT_REF);

            judoAmount = decimal.Parse(Intent.GetStringExtra(JudoSDKManager.JUDO_AMOUNT));
            judoId = Intent.GetStringExtra(JudoSDKManager.JUDO_ID);
            judoCurrency = Intent.GetStringExtra(JudoSDKManager.JUDO_CURRENCY);
            judoCardToken = Intent.GetParcelableExtra(JudoSDKManager.JUDO_CARD_DETAILS).JavaCast<CardToken>();
            judoConsumer = Intent.GetParcelableExtra(JudoSDKManager.JUDO_CONSUMER).JavaCast<Consumer>();

            if (judoPaymentRef == null)
            {
                throw new ArgumentException("JUDO_PAYMENT_REF must be supplied");
            }
            if (judoConsumer == null)
            {
                throw new ArgumentException("JUDO_CONSUMER must be supplied");
            }
            if (judoAmount == null)
            {
                throw new ArgumentException("JUDO_AMOUNT must be supplied");
            } 
            if (judoId == null)
            {
                throw new ArgumentException("JUDO_ID must be supplied");
            }
            if (judoCurrency == null)
            {
                throw new ArgumentException("JUDO_CURRENCY must be supplied");
            }
            if (judoCardToken == null)
            {
                throw new ArgumentException("JUDO_CARD_DETAILS must be supplied");
            }

            cv2EntryView.SetCardDetails(judoCardToken);

            judoMetaData = Intent.GetBundleExtra(JudoSDKManager.JUDO_META_DATA);

            var payButton = FindViewById<Button>(Resource.Id.payButton);

            var that = this;

            payButton.Text = Resources.GetString(Resource.String.token_payment);
            payButton.Click += (sender, args) =>
            {
                try
                {
                    MakeTokenPayment();
                }
                catch (Exception e)
                {
                    Toast.MakeText(that, e.Message, ToastLength.Short).Show();
                    Log.Error(JudoSDKManager.DEBUG_TAG, "Exception", e);
                }
            };
        }

        public override void OnBackPressed()
        {
            SetResult(JudoSDKManager.JUDO_CANCELLED);
            base.OnBackPressed();
        }

        public virtual void MakeTokenPayment()
        {
            TokenPaymentModel payment = new TokenPaymentModel()
            {
                JudoId = judoId,
                Currency = judoCurrency,
                Amount =  judoAmount,
                YourPaymentReference = judoPaymentRef,
                ConsumerToken = judoConsumer.ConsumerToken,
                YourConsumerReference = judoConsumer.YourConsumerReference,
                //YourPaymentMetaData = judoMetaData TODO Solve how handle the metadata
                CardToken = judoCardToken.Token,
                CV2 = cv2EntryView.GetCV2(),

            };

            ShowLoadingSpinner(true);

            JudoSDKManager.JudoClient.Payments.Create(payment).ContinueWith(t =>
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

        public void ShowLoadingSpinner(bool show)
        {
            RunOnUiThread(() =>
            {
                ((InputMethodManager)GetSystemService(Context.InputMethodService)).HideSoftInputFromWindow(
                    FindViewById(Resource.Id.payButton).WindowToken, 0);
                FindViewById(Resource.Id.loadingLayout).Visibility = show ? ViewStates.Visible : ViewStates.Gone;

            });
        }
    }
}