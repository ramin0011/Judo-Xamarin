using System.Collections.Generic;
using Android.App;
using Android.Content;
using JudoDotNetXamarinAndroidSDK.Activies;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinSDK;

namespace JudoDotNetXamarinAndroidSDK.Clients
{
    internal class UIMethods : IUIMethods
    {
		private const int ACTION_CARD_PAYMENT   = 101;
		private const int ACTION_TOKEN_PAYMENT  = 102;
		private const int ACTION_PREAUTH        = 201;
		private const int ACTION_TOKEN_PREAUTH  = 202;
		private const int ACTION_REGISTER_CARD  = 301;

        public void Payment(string judoId, string currency, string amount,
            string paymentReference, string consumerReference, Dictionary<string, string> metaData)
        {
            Intent intent = new Intent(Application.Context, typeof(PaymentActivity));
            intent.PutExtra(JudoSDKManagerA.JUDO_PAYMENT_REF, paymentReference);
            intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference));
            intent.PutExtra(JudoSDKManagerA.JUDO_AMOUNT, amount);
            intent.PutExtra(JudoSDKManagerA.JUDO_ID, judoId);
            intent.PutExtra(JudoSDKManagerA.JUDO_CURRENCY, currency);


            intent.PutExtra(JudoSDKManagerA.JUDO_META_DATA, new MetaData(metaData));

			Activity.StartActivityForResult(intent, ACTION_CARD_PAYMENT);
        }

        public Intent PreAuth(Context context, string judoId, string currency, string amount,
            string paymentReference, string consumerReference, Dictionary<string, string> metaData)
        {
            Intent intent = new Intent(context, typeof(PreAuthActivity));
            intent.PutExtra(JudoSDKManagerA.JUDO_PAYMENT_REF, paymentReference);
            intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference));
            intent.PutExtra(JudoSDKManagerA.JUDO_AMOUNT, amount);
            intent.PutExtra(JudoSDKManagerA.JUDO_ID, judoId);
            intent.PutExtra(JudoSDKManagerA.JUDO_CURRENCY, currency);


            intent.PutExtra(JudoSDKManagerA.JUDO_META_DATA, new MetaData(metaData));

            return intent;
        }

        public Intent TokenPayment(Context context, string judoId, string currency, string amount,
            string paymentReference, string consumerReference, CardToken cardToken, Dictionary<string, string> metaData, string consumerToken = null)
        {
            Intent intent = new Intent(context, typeof(PaymentTokenActivity));
            intent.PutExtra(JudoSDKManagerA.JUDO_PAYMENT_REF, paymentReference);
            intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference, consumerToken));
            intent.PutExtra(JudoSDKManagerA.JUDO_AMOUNT, amount);
            intent.PutExtra(JudoSDKManagerA.JUDO_ID, judoId);
            intent.PutExtra(JudoSDKManagerA.JUDO_CURRENCY, currency);
            intent.PutExtra(JudoSDKManagerA.JUDO_CARD_DETAILS, cardToken);


            intent.PutExtra(JudoSDKManagerA.JUDO_META_DATA, new MetaData(metaData));

            return intent;
        }

        public Intent TokenPreAuth(Context context, string judoId, string currency, string amount,
            string paymentReference, string consumerReference, CardToken cardToken, Dictionary<string, string> metaData, string consumerToken = null)
        {
            Intent intent = new Intent(context, typeof(PreAuthTokenActivity));
            intent.PutExtra(JudoSDKManagerA.JUDO_PAYMENT_REF, paymentReference);
            intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference, consumerToken));
            intent.PutExtra(JudoSDKManagerA.JUDO_AMOUNT, amount);
            intent.PutExtra(JudoSDKManagerA.JUDO_ID, judoId);
            intent.PutExtra(JudoSDKManagerA.JUDO_CURRENCY, currency);
            intent.PutExtra(JudoSDKManagerA.JUDO_CARD_DETAILS, cardToken);


            intent.PutExtra(JudoSDKManagerA.JUDO_META_DATA, new MetaData(metaData));

            return intent;
        }

        public Intent RegisterCard(Context context, string consumerReference)
        {
            Intent intent = new Intent(context, typeof(RegisterCardActivity));
            intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference));

            return intent;
        }
    }
}