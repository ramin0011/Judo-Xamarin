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
using JudoDotNetXamarinSDK.Ui;
using JudoDotNetXamarinSDK.Utils;
using JudoPayDotNet.Models;
using Environment = JudoPayDotNet.Enums.Environment;

namespace JudoDotNetXamarinSDK.Activies
{
    public class PaymentActivity : BaseActivity
    {
        private string judoPaymentRef;
        private decimal judoAmount;
        private string judoId;
        private string judoCurrency;
        private Bundle judoMetaData;
        private CardEntryView cardEntryView;
        private Utils.Consumer judoConsumer;
        private AVSEntryView avsEntryView;
        private HelpButton cv2ExpiryHelpInfoButton;
        private StartDateIssueNumberEntryView startDateEntryView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.payment);
            Title = GetString(Resource.String.title_payment);
            cardEntryView = FindViewById<CardEntryView>(Resource.Id.cardEntryView);
            TextView hintTextView = FindViewById<TextView>(Resource.Id.hintTextView);
            cardEntryView.HintTextView = hintTextView;
            avsEntryView = FindViewById<AVSEntryView>(Resource.Id.avsEntryView);
            startDateEntryView = FindViewById<StartDateIssueNumberEntryView>(Resource.Id.startDateEntryView);

            cv2ExpiryHelpInfoButton = FindViewById<HelpButton>(Resource.Id.infoButtonID);
            cv2ExpiryHelpInfoButton.Visibility = ViewStates.Gone;

            SetHelpText(Resource.String.help_info, Resource.String.help_card_text);
            SetHelpText(Resource.String.help_postcode_title, Resource.String.help_postcode_text,
                Resource.Id.postCodeHelpButton);

            judoPaymentRef = Intent.GetStringExtra(JudoSDKManager.JUDO_PAYMENT_REF);
            judoConsumer = Intent.GetParcelableExtra(JudoSDKManager.JUDO_CONSUMER).JavaCast<Utils.Consumer>();

            judoAmount = decimal.Parse(Intent.GetStringExtra(JudoSDKManager.JUDO_AMOUNT));
            judoId = Intent.GetStringExtra(JudoSDKManager.JUDO_ID);
            judoCurrency = Intent.GetStringExtra(JudoSDKManager.JUDO_CURRENCY);

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
            } if (judoPaymentRef == null)
            {
                throw new ArgumentException("JUDO_PAYMENT_REF must be supplied");
            }
            if (judoId == null)
            {
                throw new ArgumentException("JUDO_ID must be supplied");
            }
            if (judoCurrency == null)
            {
                throw new ArgumentException("JUDO_CURRENCY must be supplied");
            }

            judoMetaData = Intent.Extras.GetBundle(JudoSDKManager.JUDO_META_DATA);

            var payButton = FindViewById<Button>(Resource.Id.payButton);

            payButton.Text = Resources.GetString(Resource.String.payment);
            payButton.Click += (sender, args) =>
            {
                try
                {
                    MakeCardPayment();
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, "" + e.Message, ToastLength.Short).Show();
                    Log.Error(JudoSDKManager.DEBUG_TAG, "Exception", e);
                }
            };

            cardEntryView.OnCreditCardEntered = cardNumber =>
            {
                cv2ExpiryHelpInfoButton.Visibility = ViewStates.Visible;
            };

            cardEntryView.OnExpireAndCV2Entered = (expiryDate, cv2) =>
            {
                string cardNumber = null;

                try
                {
                    cardNumber = cardEntryView.GetCardNumber();
                }
                catch (Exception e)
                {
                    Console.Error.Write(e.StackTrace);
                }


                //TODO Implement infrastructure to suport Enabled and disable settings for avs and maestro
                //if (ValidationHelper.IsStartDateRequiredForCardNumber(cardNumber) && IsMastroEnabled)
                //{
                //    startDateEntryView.Visibility = ViewStates.Visible;
                //    startDateEntryView.RequestFocus();
                //}

                //if (IsAVSEnabled && avsEntryView != null)
                //{
                //    avsEntryView.Visibility = ViewStates.Visible;
                //}
            };

            cardEntryView.OnReturnToCreditCardNumberEntry = () =>
            {
                cv2ExpiryHelpInfoButton.Visibility = ViewStates.Gone;
            };
        }

        public override void OnBackPressed()
        {
            SetResult(JudoSDKManager.JUDO_CANCELLED);
            base.OnBackPressed();
        }

        public void MakeCardPayment()
        {
            var cardNumber = cardEntryView.GetCardNumber();
            var expiryDate = cardEntryView.GetCardExpiry();
            var cv2 = cardEntryView.GetCardCV2();

            CardAddressModel cardAddress = new CardAddressModel();

            //TODO infrastructure to support flags for AVS and Maestro card types
            //if (IsAvsEnable)
            //{
            //    var country = avsEntryView.getCountry();
            //    cardAddress.PostCode = avsEntryView.GetPostCode();
            //}

            string startDate = null;
            string issueNumber = null;

            //TODO infrastructure to support flags for AVS and Maestro card types
            //if (IsMaestroEnabled)
            //{
            //    issueNumber = startDateEntryView.GetIssueNumber();
            //    startDate = startDateEntryView.GetStartDate();
            //}

            var cardPayment = new CardPaymentModel()
            {
                JudoId = judoId,
                Currency = judoCurrency,
                Amount = judoAmount,
                YourPaymentReference = judoPaymentRef,
                //YourPaymentMetaData = judoMetaData, TODO Need to find a way to create a dictionary that can be putted inside an intent
                CardNumber = cardNumber,
                CardAddress = cardAddress,
                StartDate = startDate,
                ExpiryDate = expiryDate
            };

            ShowLoadingSpinner(true);

            //Todo create infrasctruture to hold JudoPaymentApi instead of always creating it
            var judoPay = JudoPaymentsFactory.Create(Environment.Live, "", "");

            judoPay.Payments.Create(cardPayment).ContinueWith(t =>
            {
                ShowLoadingSpinner(false);

                if (t.IsFaulted || t.Result == null || t.Result.HasError)
                {
                    var errorMessage = t.Result != null ? t.Result.Error.ErrorMessage : t.Exception.Message;
                    Log.Error("com.judopay.android", "ERROR: " + errorMessage);
                    SetResult(JudoSDKManager.JUDO_ERROR, JudoSDKManager.CreateErrorIntent(errorMessage, t.Exception));
                    return;
                }

                var receipt = t.Result.Response;

                Intent intent = new Intent();
                intent.PutExtra(JudoSDKManager.JUDO_RECEIPT, receipt.ReceiptId); //TODO this is not valid it is just the receipt id
                SetResult(JudoSDKManager.JUDO_SUCCESS, intent);
                Finish();
                Log.Debug("com.judopay.android", "SUCCESS: " + receipt.Result);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ShowLoadingSpinner(bool show)
        {
            RunOnUiThread(() =>
            {
                ((InputMethodManager) GetSystemService(Context.InputMethodService)).HideSoftInputFromWindow(
                    FindViewById(Resource.Id.payButton).WindowToken, 0);
                FindViewById(Resource.Id.loadingLayout).Visibility = show ? ViewStates.Visible : ViewStates.Gone;

            });
        }
    }
}