using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using JudoDotNetXamarin;
using JudoPayDotNet;
using JudoPayDotNet.Models;
using Newtonsoft.Json;
using Environment = JudoPayDotNet.Enums.Environment;

namespace IntermediateClassLibraryWithUi
{
    public class Payment : Activity
    {
        private TextView result;
        private JudoPayApi judo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Payment);
            // Create your application here

            judo = JudoPaymentsFactory.Create(Environment.Live, "MsFzv4MO2qs88qdw",
                "72f508cbd37793279b0b29fe6dfc3b118010167551e85a2ace446d7fcf7ff46a");

			result = FindViewById<TextView>(Resource.Id.result);
			var payButton = FindViewById<Button> (Resource.Id.payButton);

			payButton.Click += (sender, args) =>
			{
			    result.Text = String.Empty;

                CardPaymentModel card = new CardPaymentModel
                {
                    JudoId = "10014835",
                YourPaymentReference = "578543",
                YourConsumerReference    =  Guid.NewGuid().ToString(),
                Amount = 25,
                CardNumber = "4976000000003436",
                CV2 = "452",
                ExpiryDate = "12/15",
                CardAddress = new CardAddressModel
                                {
                                    Line1 = "Test Street",
                                    PostCode = "TR14 8PA",
                                    Town = "Town"
                                }
                };

                var that = this;

                judo.Payments.Create(card).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Toast.MakeText(that, "Ups failure on payment", ToastLength.Short).Show();
                    }

                    Toast.MakeText(that, t.Result.Response.ReceiptId, ToastLength.Long).Show();

                    this.result.Text = JsonConvert.SerializeObject(t.Result.Response);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            };

        }
    }
}