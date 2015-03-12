using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JudoDotNetXamarinSDK;
using JudoDotNetXamarinSDK.Models;
using JudoPayDotNet.Models;
using Result = Android.App.Result;

namespace Android.Xamarin.SampleApp
{
    [Activity(Label = "@string/app_name")]
    public class WithoutUIActivity : Activity
    {
        private const string ApiToken = "4eVWyZQnO5DyaXZy";
        private const string ApiSecret = "1d5e8381ed9ef3cc1ecc1daaf8ce550bdc97ea058ac804be4b68c28d02fdb791";

        private const int ACTION_PAYMENT = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // setting up API token/secret 
            JudoSDKManager.SetApiTokenAndSecret(ApiToken, ApiSecret);
            JudoSDKManager.IsAVSEnabled = true;

            // Set our view from the "withoutui" layout resource
            SetContentView(Resource.Layout.withoutui);

            var that = this;
            // Get our button from the layout resource,
            // and attach an event to it
            Button payCard = FindViewById<Button>(Resource.Id.payCard);
            Button threeDsecure = FindViewById<Button>(Resource.Id.threeDsecure);
            Button payToken = FindViewById<Button>(Resource.Id.payToken);
            Button payPreAuth = FindViewById<Button>(Resource.Id.payPreAuth);
            Button payTokenPreAuth = FindViewById<Button>(Resource.Id.payTokenPreAuth);
            Button registerCard = FindViewById<Button>(Resource.Id.registerCard);
            Button getTransactions = FindViewById<Button>(Resource.Id.getTransactions);

            // Assigning click delegates
            payCard.Click += new EventHandler(payCard_Click);
            threeDsecure.Click += new EventHandler(threeDsecure_Click);
            payToken.Click += new EventHandler(payToken_Click);
            payPreAuth.Click += new EventHandler(payPreAuth_Click);
            payTokenPreAuth.Click += new EventHandler(payTokenPreAuth_Click);
            registerCard.Click += new EventHandler(registerCard_Click);
            getTransactions.Click += new EventHandler(getTransactions_Click);
        }


#region Button_Click_Events

        private void payCard_Click(object sender, EventArgs e)
        {
            var judoId = "100016";
            var currency = "GBP";
            var amount = "4.99";
            var paymentReference = "payment101010102";
            var consumerRef = "consumer1010102";

            var intent = JudoSDKManager.makeAPayment(this, judoId, currency, amount, paymentReference, consumerRef, null);

            StartActivityForResult(intent, ACTION_PAYMENT);
        }

        private void threeDsecure_Click(object sender, EventArgs e)
        {
            UpdateCount(sender);
        }

        private void payToken_Click(object sender, EventArgs e)
        {
        }

        private void payPreAuth_Click(object sender, EventArgs e)
        {
            UpdateCount(sender);
        }

        private void payTokenPreAuth_Click(object sender, EventArgs e)
        {
            UpdateCount(sender);
        }

        private void registerCard_Click(object sender, EventArgs e)
        {
            UpdateCount(sender);
        }

        private void getTransactions_Click(object sender, EventArgs e)
        {
            UpdateCount(sender);
        }

#endregion //Button_Click_Events

        private void UpdateCount(object sender)
        {
            Console.WriteLine(sender.ToString());
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