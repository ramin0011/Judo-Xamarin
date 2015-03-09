using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using JudoDotNetXamarinSDK;
using JudoDotNetXamarinSDK.Models;
using JudoPayDotNet.Models;
using Result = Android.App.Result;

namespace AndroidTestApp
{
    public class MainActivity : Activity
    {
        int count = 1;
        private const string ApiToken = "4eVWyZQnO5DyaXZy";
        private const string ApiSecret = "1d5e8381ed9ef3cc1ecc1daaf8ce550bdc97ea058ac804be4b68c28d02fdb791";

        private const int ACTION_PAYMENT = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            JudoSDKManager.SetApiTokenAndSecret(ApiToken, ApiSecret);
            JudoSDKManager.IsAVSEnabled = true;

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout
            Button makeAPaymentButton = FindViewById<Button>(Resource.Id.MakePayment);

            var that = this;

            makeAPaymentButton.Click += (sender, e) =>
            {
                var judoId = "100016";
                var currency = "GBP";
                var amount = "4.99";
                var paymentReference = "payment101010102";
                var consumerRef = "consumer1010102";

                var intent = JudoSDKManager.makeAPayment(that, judoId, currency, amount, paymentReference, consumerRef, null);

                StartActivityForResult(intent, ACTION_PAYMENT);
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case ACTION_PAYMENT:
                    var receipt = data.GetParcelableExtra(JudoSDKManager.JUDO_RECEIPT) as Receipt;
                    if (resultCode == Result.Ok && receipt.Result != "Declined")
                    {
                        Toast.MakeText(this, string.Format("Payment succeeded: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, string.Format("Payment failed: id: {0}, Message: {1}, result: {2}", receipt.ReceiptId, receipt.Message, receipt.Result), ToastLength.Long).Show();
                    }
                    break;
            }
        }
    }
}

