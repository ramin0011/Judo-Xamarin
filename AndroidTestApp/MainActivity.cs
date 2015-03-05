using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using JudoDotNetXamarinSDK;

namespace AndroidTestApp
{
    public class MainActivity : Activity
    {
        int count = 1;

        private const int ACTION_PAYMENT = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
                    if (resultCode == Result.Ok)
                    {
                        var receiptId = data.GetStringExtra(JudoSDKManager.JUDO_RECEIPT);
                        Toast.MakeText(this, string.Format("Payment succeeded: id: {0}", receiptId), ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Payment failed", ToastLength.Long).Show();
                    }
                    break;
            }
        }
    }
}

