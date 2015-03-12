using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JudoDotNetXamarin;
using JudoDotNetXamarinSDK.Activies;
using JudoDotNetXamarinSDK.Models;
using JudoDotNetXamarinSDK.Utils;
using JudoPayDotNet;
using JudoPayDotNet.Errors;
using Environment = JudoPayDotNet.Enums.Environment;
using Error = JudoDotNetXamarinSDK.Models.Error;

namespace JudoDotNetXamarinSDK
{
    public class JudoSDKManager
    {
        public static readonly Result JUDO_SUCCESS = Result.Ok;
        public static readonly Result JUDO_CANCELLED = Result.Canceled;
        public static readonly Result JUDO_ERROR = Result.FirstUser;

        public static String JUDO_PAYMENT_REF = "JudoPay-yourPaymentReference";
        public static String JUDO_AMOUNT = "JudoPay-amount";
        public static String JUDO_ID = "JudoPay-judoId";
        public static String JUDO_CURRENCY = "JudoPay-currency";
        public static String JUDO_META_DATA = "JudoPay-yourPaymentMetaData";

        public static String JUDO_RECEIPT = "JudoPay-receipt";

        public static String JUDO_CARD_DETAILS = "JudoPay-CardToken";
        public static String JUDO_CONSUMER = "JudoPay-consumer";

        public static String JUDO_ERROR_MESSAGE = "ERROR_MESSAGE";
        public static String JUDO_ERROR_EXCEPTION = "ERROR_EXCEPTION";

        private static string REGULAR_CARD_FORMAT_HINT = "0000 0000 0000 0000";
        private static string AMEX_CARD_FORMAT_HINT = "0000 000000 00000";
        private static String REGULAR_EXPIRY_AND_VALIDATION_FORMAT_HINT = "MM/YY CV2";
        private static String AMEX_EXPIRY_AND_VALIDATION_FORMAT_HINT = "MM/YY CIDV";
        private static String REGULAR_EXPIRY_AND_VALIDATION_ERROR_MESSAGE = "Invalid CV2";
        private static String AMEX_EXPIRY_AND_VALIDATION_ERROR_MESSAGE = "Invalid CIDV";

        private static volatile bool avsEnabled = false;
        public static bool IsAVSEnabled { get { return avsEnabled; } set { avsEnabled = value; } }
        
        private static volatile bool maestroEnabled = false;
        public static bool IsMaestroEnabled { get { return maestroEnabled; } set { maestroEnabled = value; } }

        private static volatile bool fraudMonitoringSignals = false;
        public static bool IsFraudMonitoringSignals { get { return fraudMonitoringSignals; } set { fraudMonitoringSignals = value; } }

        private static volatile bool isSSLPinningEnabled = false;
        public static bool IsSSLPinningEnabled { get { return isSSLPinningEnabled; } set { isSSLPinningEnabled = value; } }

        private static readonly object _clientLock = new object();
        private static JudoPayApi _judoClient;

        internal static JudoPayApi JudoClient
        {
            get
            {
                lock (_clientLock)
                {
                    return _judoClient;
                }
            }
        }

        private static Environment _environment;

        public static Environment Environment
        {
            get
            {
                lock (_clientLock)
                {
                    return _environment;
                }
            }
        }


        /// <summary>
        /// Sets the configuration to access judo servers
        /// </summary>
        /// <param name="apiToken">The apiToken of the merchant</param>
        /// <param name="apiSecret">The apiSecret of the merchant</param>
        /// <param name="environment">The environment to use</param>
        public static void SetApiTokenAndSecret(string apiToken, string apiSecret, Environment environment = Environment.Live)
        {
            lock(_clientLock)
            {
                _environment = environment;
                _judoClient = JudoPaymentsFactory.Create(_environment, apiToken, apiSecret);
            }
        }

        public static string DEBUG_TAG = "com.judopay.android";

        public static string CardHintFormat(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_CARD_FORMAT_HINT;
                default:
                    return REGULAR_CARD_FORMAT_HINT;
            }
        }

        public static int GetCardResourceId(Context context, CardBase.CardType cardType, bool showFront)
        {
            if (showFront)
            {
                switch (cardType)
                {
                    case CardBase.CardType.VISA:
                        return Resource.Drawable.ic_card_visa;
                    case CardBase.CardType.MASTERCARD:
                        return Resource.Drawable.ic_card_mastercard;
                    case CardBase.CardType.AMEX:
                        return Resource.Drawable.ic_card_amex;
                    case CardBase.CardType.MASTRO:
                        return Resource.Drawable.ic_card_maestro;
                    case CardBase.CardType.UNKNOWN:
                    default:
                        return Resource.Drawable.ic_card_visa;
                }
            }
            else
            {
                switch (cardType)
                {
                    case CardBase.CardType.AMEX:
                        return Resource.Drawable.ic_card_cv2_amex;
                    default:
                        return Resource.Drawable.ic_card_cv2;
                }
            }
        }

        public static string GetCardHintFormat(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_CARD_FORMAT_HINT;
                default:
                    return REGULAR_CARD_FORMAT_HINT;
            }
        }

        public static string GetExpiryAndValidationHintFormat(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_EXPIRY_AND_VALIDATION_FORMAT_HINT;
                default:
                    return REGULAR_EXPIRY_AND_VALIDATION_FORMAT_HINT;
            }
        }

        public static string GetExpiryAndValidationErrorMessage(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_EXPIRY_AND_VALIDATION_ERROR_MESSAGE;
                default:
                    return REGULAR_EXPIRY_AND_VALIDATION_ERROR_MESSAGE;
            }
        }

        public static Intent CreateErrorIntent(string message, Exception exception, JudoApiErrorModel apiErrorModel)
        {
            Intent intent = new Intent();
            intent.PutExtra(JUDO_ERROR_MESSAGE, message);
            intent.PutExtra(JUDO_ERROR_EXCEPTION, new Error(exception, apiErrorModel));

            return intent;
        }

        public static Intent makeAPayment(Context context, string judoId, string currency, string amount,
                                          string yourPaymentRef, string consumerRef, Dictionary<string, string> metaData)
        {
            Intent intent = new Intent(context, typeof(PaymentActivity));
            intent.PutExtra(JUDO_PAYMENT_REF, yourPaymentRef);
            intent.PutExtra(JUDO_CONSUMER, new Consumer(consumerRef));
            intent.PutExtra(JUDO_AMOUNT, amount);
            intent.PutExtra(JUDO_ID, judoId);
            intent.PutExtra(JUDO_CURRENCY, currency);


            intent.PutExtra(JUDO_META_DATA, new MetaData(metaData));

            return intent;
        }

        public static Intent makeATokenPayment(Context context, string judoId, string currency, string amount,
                                          string yourPaymentRef, string consumerRef, CardToken cardToken, Dictionary<string, string> metaData, string consumerToken = null)
        {
            Intent intent = new Intent(context, typeof(PaymentTokenActivity));
            intent.PutExtra(JUDO_PAYMENT_REF, yourPaymentRef);
            intent.PutExtra(JUDO_CONSUMER, new Consumer(consumerRef, consumerToken));
            intent.PutExtra(JUDO_AMOUNT, amount);
            intent.PutExtra(JUDO_ID, judoId);
            intent.PutExtra(JUDO_CURRENCY, currency);
            intent.PutExtra(JUDO_CARD_DETAILS, cardToken);


            intent.PutExtra(JUDO_META_DATA, new MetaData(metaData));

            return intent;
        }

        public static Intent makeATokenPreAuth(Context context, string judoId, string currency, string amount,
                                          string yourPaymentRef, string consumerRef, CardToken cardToken, Dictionary<string, string> metaData, string consumerToken = null)
        {
            Intent intent = new Intent(context, typeof(PreAuthTokenActivity));
            intent.PutExtra(JUDO_PAYMENT_REF, yourPaymentRef);
            intent.PutExtra(JUDO_CONSUMER, new Consumer(consumerRef, consumerToken));
            intent.PutExtra(JUDO_AMOUNT, amount);
            intent.PutExtra(JUDO_ID, judoId);
            intent.PutExtra(JUDO_CURRENCY, currency);
            intent.PutExtra(JUDO_CARD_DETAILS, cardToken);


            intent.PutExtra(JUDO_META_DATA, new MetaData(metaData));

            return intent;
        }

        public static Intent registerCard(Context context, string consumerRef)
        {
            Intent intent = new Intent(context, typeof(RegisterCardActivity));
            intent.PutExtra(JUDO_CONSUMER, new Consumer(consumerRef));

            return intent;
        }
    }
}