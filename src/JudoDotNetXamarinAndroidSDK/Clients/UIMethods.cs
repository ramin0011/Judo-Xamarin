using System.Collections.Generic;
using Android.Content;
using JudoDotNetXamarinSDK.Activies;
using JudoDotNetXamarinSDK.Models;
using Android.App;

namespace JudoDotNetXamarinSDK
{
    internal class UIMethods : IUIMethods
    {
        public Intent Payment(string judoId, string currency, string amount,
            string paymentReference, string consumerReference, Dictionary<string, string> metaData)
        {
            Intent intent = new Intent(Application.Context, typeof(PaymentActivity));
            intent.PutExtra(JudoSDKManagerA.JUDO_PAYMENT_REF, paymentReference);
            intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference));
            intent.PutExtra(JudoSDKManagerA.JUDO_AMOUNT, amount);
            intent.PutExtra(JudoSDKManagerA.JUDO_ID, judoId);
            intent.PutExtra(JudoSDKManagerA.JUDO_CURRENCY, currency);


            intent.PutExtra(JudoSDKManagerA.JUDO_META_DATA, new MetaData(metaData));

            return intent;
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