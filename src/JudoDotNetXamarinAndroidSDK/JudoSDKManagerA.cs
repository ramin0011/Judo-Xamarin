using System;
using Android.Content;
using JudoPayDotNet;
using JudoPayDotNet.Errors;
using Newtonsoft.Json.Linq;
using Configuration = JudoDotNetXamarinAndroidSDK.Configurations.Configuration;
using Error = JudoDotNetXamarinAndroidSDK.Models.Error;
using Result = Android.App.Result;
using System.Diagnostics;
using JudoPayDotNet.Enums;
using JudoDotNetXamarin;
using JudoDotNetXamarin.Clients;
using JudoDotNetXamarin.Delegates;
using JudoDotNetXamarin.ViewModels;
using JudoDotNetXamarinAndroidSDK;
using JudoDotNetXamarinAndroidSDK.Clients;
using JudoDotNetXamarinAndroidSDK.Configurations;
using JudoDotNetXamarinAndroidSDK.Utils;

namespace JudoDotNetXamarinSDK
{
	public sealed class JudoSDKManagerA : IJudoSDKManager
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
		private static IJudoSDKApi _judoSdkApi;
        

        private readonly object _clientLock = new object();
        internal IJudoPayApi _judoClient;

        internal static IJudoPayApi JudoClient
        {
            get
            {
                lock (Instance._clientLock)
                {
                    return Instance._judoClient;
                }
            }
        }

        private JudoEnvironment _environment;

        public JudoEnvironment Environment
        {
            get
            {
                lock (_clientLock)
                {
                    return _environment;
                }
            }
        }

		#region IJudoSDKManager implementation

		public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			throw new NotImplementedException ();
		}

		public void PreAuth (PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			throw new NotImplementedException ();
		}

		public void TokenPayment (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			throw new NotImplementedException ();
		}

		public void TokenPreAuth (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			throw new NotImplementedException ();
		}

		public void RegisterCard (PaymentViewModel registerCard, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			throw new NotImplementedException ();
		}

		#endregion

        private readonly IUIMethods _uIMethods;
        public static IUIMethods UIMethods
        {
            get { return Instance._uIMethods; }
        }

        private readonly INonUIMethods _nonUIMethods;
        public static INonUIMethods NonUIMethods
        {
            get { return Instance._nonUIMethods; }
        }

        private readonly IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            get { return Instance._configuration; }
        }

        private static readonly Lazy<JudoSDKManagerA> _singleton = new Lazy<JudoSDKManagerA>(() => new JudoSDKManagerA());

        public static JudoSDKManagerA Instance
        {
            get { return _singleton.Value; }
        }

        static JudoSDKManagerA()
        {

        }

        private JudoSDKManagerA()
        {
            _uIMethods = new UIMethods();
            _nonUIMethods = new NonUIMethods();
            _configuration = new Configuration();
        }

        internal void SetJudoClient(IJudoPayApi judoClient)
        {
            lock (_clientLock)
            {
                _judoClient = judoClient;
            }
        }

        internal void SetEnvironment(JudoEnvironment environment)
        {
            lock (_clientLock)
            {
                _environment = environment;
            }  
        }

        internal static string DEBUG_TAG = "com.judopay.android";

        internal static string CardHintFormat(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_CARD_FORMAT_HINT;
                default:
                    return REGULAR_CARD_FORMAT_HINT;
            }
        }

        internal static int GetCardResourceId(Context context, CardBase.CardType cardType, bool showFront)
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
                        return Resource.Drawable.ic_card_unknown;
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

        internal static string GetCardHintFormat(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_CARD_FORMAT_HINT;
                default:
                    return REGULAR_CARD_FORMAT_HINT;
            }
        }

        internal static string GetExpiryAndValidationHintFormat(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_EXPIRY_AND_VALIDATION_FORMAT_HINT;
                default:
                    return REGULAR_EXPIRY_AND_VALIDATION_FORMAT_HINT;
            }
        }

        internal static string GetExpiryAndValidationErrorMessage(CardBase.CardType cardType)
        {
            switch (cardType)
            {
                case CardBase.CardType.AMEX:
                    return AMEX_EXPIRY_AND_VALIDATION_ERROR_MESSAGE;
                default:
                    return REGULAR_EXPIRY_AND_VALIDATION_ERROR_MESSAGE;
            }
        }

        internal static Intent CreateErrorIntent(string message, Exception exception, JudoApiErrorModel apiErrorModel)
        {
            Intent intent = new Intent();
            intent.PutExtra(JUDO_ERROR_MESSAGE, message);
            intent.PutExtra(JUDO_ERROR_EXCEPTION, new Error(exception, apiErrorModel));

            return intent;
        }

        internal static JObject GetClientDetails(Context context)
        {
            return JObject.FromObject(ClientDetailsProvider.GetClientDetails(context));
        }

		internal static string GetSDKVersion ()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion;

			return "Xamarin-Android-" + version;
		}
    }
}