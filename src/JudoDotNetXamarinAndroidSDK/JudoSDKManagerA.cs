using JudoDotNetXamarin;
using Android.App;
using JudoDotNetXamarin.Clients;
using System;
using JudoPayDotNet.Models;
//using System.Runtime.Remoting.Contexts;
using JudoDotNetXamarinAndroidSDK;
using Android.Content;
using Newtonsoft.Json.Linq;
using JudoDotNetXamarinAndroidSDK.Utils;
using System.Diagnostics;
using JudoPayDotNet.Errors;
using JudoDotNetXamarinAndroidSDK.Models;

namespace JudoDotNetXamarinSDK
{
	public sealed class JudoSDKManagerA //: IJudoSDKManager
    {
		public static readonly Android.App.Result JUDO_SUCCESS = Android.App.Result.Ok;
		public static readonly Android.App.Result JUDO_CANCELLED = Android.App.Result.Canceled;
		public static readonly Android.App.Result JUDO_ERROR = Android.App.Result.FirstUser;

        public static string JUDO_PAYMENT_REF = "JudoPay-yourPaymentReference";
		public static string JUDO_AMOUNT = "JudoPay-amount";
		public static string JUDO_ID = "JudoPay-judoId";
		public static string JUDO_CURRENCY = "JudoPay-currency";
		public static string JUDO_META_DATA = "JudoPay-yourPaymentMetaData";

		public static string JUDO_RECEIPT = "JudoPay-receipt";

		public static string JUDO_CARD_DETAILS = "JudoPay-CardToken";
		public static string JUDO_CONSUMER = "JudoPay-consumer";

		public static string JUDO_ERROR_MESSAGE = "ERROR_MESSAGE";
		public static string JUDO_ERROR_EXCEPTION = "ERROR_EXCEPTION";

        private static string REGULAR_CARD_FORMAT_HINT = "0000 0000 0000 0000";
        private static string AMEX_CARD_FORMAT_HINT = "0000 000000 00000";
		private static string REGULAR_EXPIRY_AND_VALIDATION_FORMAT_HINT = "MM/YY CV2";
		private static string AMEX_EXPIRY_AND_VALIDATION_FORMAT_HINT = "MM/YY CIDV";
		private static string REGULAR_EXPIRY_AND_VALIDATION_ERROR_MESSAGE = "Invalid CV2";
		private static string AMEX_EXPIRY_AND_VALIDATION_ERROR_MESSAGE = "Invalid CIDV";

		private static IJudoSDKApi _judoSdkApi;



		/// <summary>
		/// Enable 3D security process
		/// </summary>
		public  bool ThreeDSecureEnabled{ get; set; }

		/// <summary>
		/// Enable/Disable AVS check
		/// </summary>
		public  bool AVSEnabled { get; set; }

		/// <summary>
		/// Enable/Disable Amex card support
		/// </summary>
		public  bool AmExAccepted { get; set; }

		/// <summary>
		/// Enable/Disable Mestro card support
		/// </summary>
		public  bool MaestroAccepted { get; set; }


		/// <summary>
		/// Enable/Disable risk signal to pass fruad monitoring device data
		/// default is true
		/// </summary>
		public  bool RiskSignals{ get; set; }

		/// <summary>
		/// SSLPinningEnabled
		/// </summary>
		public bool SSLPinningEnabled { get; set; }

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
			// setting up UI mode by default
			Instance.UIMode = true;
			Instance.RiskSignals = true;
		}

		private  bool _uiMode { get; set; }

		public bool UIMode {
			get { return _uiMode; }
			set {
				if (value) {
				}
				//	_judoSdkApi = new UIMethods(new ViewLocator(PaymentService));
				else {
					//	_judoSdkApi = new NonUIMethods(PaymentService);
					_uiMode = value;
				}
			}
		}
			

		public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var innerModel = payment.Clone ();

			_judoSdkApi.Payment (innerModel, success, failure);

//			Intent intent = new Intent(Application.Context, typeof(PaymentActivity));
//			intent.PutExtra(JudoSDKManagerA.JUDO_PAYMENT_REF, paymentReference);
//			intent.PutExtra(JudoSDKManagerA.JUDO_CONSUMER, new Consumer(consumerReference));
//			intent.PutExtra(JudoSDKManagerA.JUDO_AMOUNT, payment.Amount);
//			intent.PutExtra(JudoSDKManagerA.JUDO_ID, judoId);
//			intent.PutExtra(JudoSDKManagerA.JUDO_CURRENCY, currency);
//
//
//			intent.PutExtra(JudoSDKManagerA.JUDO_META_DATA, new MetaData(metaData));
//			Activity.StartActivityForResult(intent, ACTION_CARD_PAYMENT);
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


//        private readonly object _clientLock = new object();
//        internal IJudoPayApi _judoClient;
//
//        internal static IJudoPayApi JudoClient
//        {
//            get
//            {
//                lock (Instance._clientLock)
//                {
//                    return Instance._judoClient;
//                }
//            }
//        }
//
//        private JudoEnvironment _environment;
//
//        public JudoEnvironment Environment
//        {
//            get
//            {
//                lock (_clientLock)
//                {
//                    return _environment;
//                }
//            }
//        }



//		
//
//        private readonly IUIMethods _uIMethods;
//        public static IUIMethods UIMethods
//        {
//            get { return Instance._uIMethods; }
//        }
//
//        private readonly INonUIMethods _nonUIMethods;
//        public static INonUIMethods NonUIMethods
//        {
//            get { return Instance._nonUIMethods; }
//        }
//
//        private readonly IConfiguration _configuration;
//        public static IConfiguration Configuration
//        {
//            get { return Instance._configuration; }
//        }


//        static JudoSDKManagerA()
//        {
//
//        }

//        private JudoSDKManagerA()
//        {
//            _uIMethods = new UIMethods();
//            _nonUIMethods = new NonUIMethods();
//            _configuration = new Configuration();
//        }
//
//        internal void SetJudoClient(IJudoPayApi judoClient)
//        {
//            lock (_clientLock)
//            {
//                _judoClient = judoClient;
//            }
//        }
//
//        internal void SetEnvironment(JudoEnvironment environment)
//        {
//            lock (_clientLock)
//            {
//                _environment = environment;
//            }  
//        }

        internal static string DEBUG_TAG = "com.judopay.android";

        internal static string CardHintFormat(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.AMEX:
                    return AMEX_CARD_FORMAT_HINT;
                default:
                    return REGULAR_CARD_FORMAT_HINT;
            }
        }

        internal static int GetCardResourceId(Context context, CardType cardType, bool showFront)
        {
            if (showFront)
            {
                switch (cardType)
                {
                    case CardType.VISA:
                        return Resource.Drawable.ic_card_visa;
                    case CardType.MASTERCARD:
                        return Resource.Drawable.ic_card_mastercard;
                    case CardType.AMEX:
                        return Resource.Drawable.ic_card_amex;
                    case CardType.MAESTRO:
                        return Resource.Drawable.ic_card_maestro;
                    case CardType.UNKNOWN:
                    default:
                        return Resource.Drawable.ic_card_unknown;
                }
            }
            else
            {
                switch (cardType)
                {
                    case CardType.AMEX:
                        return Resource.Drawable.ic_card_cv2_amex;
                    default:
                        return Resource.Drawable.ic_card_cv2;
                }
            }
        }

        internal static string GetCardHintFormat(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.AMEX:
                    return AMEX_CARD_FORMAT_HINT;
                default:
                    return REGULAR_CARD_FORMAT_HINT;
            }
        }

        internal static string GetExpiryAndValidationHintFormat(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.AMEX:
                    return AMEX_EXPIRY_AND_VALIDATION_FORMAT_HINT;
                default:
                    return REGULAR_EXPIRY_AND_VALIDATION_FORMAT_HINT;
            }
        }

        internal static string GetExpiryAndValidationErrorMessage(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.AMEX:
                    return AMEX_EXPIRY_AND_VALIDATION_ERROR_MESSAGE;
                default:
                    return REGULAR_EXPIRY_AND_VALIDATION_ERROR_MESSAGE;
            }
        }

        internal static Intent CreateErrorIntent(string message, Exception exception, JudoApiErrorModel apiErrorModel)
        {
            Intent intent = new Intent();
            intent.PutExtra(JUDO_ERROR_MESSAGE, message);
            intent.PutExtra(JUDO_ERROR_EXCEPTION, new JudoDroidError(exception, apiErrorModel));

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