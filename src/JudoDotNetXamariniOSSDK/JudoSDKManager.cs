using System;
using CoreLocation;
using System.Collections.Generic;
using JudoDotNetXamariniOSSDK.Clients;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
    public delegate void SuccessCallback(PaymentReceiptViewModel receipt);
    public delegate void FailureCallback(JudoError error);

    public class JudoSDKManager
	{
		internal static readonly UIFont FIXED_WIDTH_FONT_SIZE_20 = UIFont.FromName("Courier", 17.0f);

		Dictionary<string, string> clientDetails {get; set;}
		public static bool LocationEnabled{ get; set; }
		public static bool ThreeDSecureEnabled{ get; set; }
		public static bool AVSEnabled { get; set; }
		public static bool AmExAccepted { get; set; }
		public static bool MaestroAccepted { get; set; }
		public static bool RiskSignals{ get; set; }

		private static readonly Lazy<JudoSDKManager> _singleton = new Lazy<JudoSDKManager>(() => new JudoSDKManager());

		public static JudoSDKManager Instance
		{
			get { return _singleton.Value; }
		}

		public static void SetSandboxMode()
		{
			
		}

		public static void SetProductionMode()
		{

		}

		public static void SetToken(string key, string secret)
		{

		}

		public static void SetOAuthToken(string oAuthToken)
		{

		}

		public static void EnableNavBar(bool navEnabled)
		{

		}

		public static void SetCurrency(string currency)
		{

		}

		public static void EnableFraudSignals(string deviceIdentifier)
		{

		}


		public static Dictionary<string, string> GetClientDetails(string deviceId)
		{
		    return null;
		}

		public static void SetUserAgent()
		{
			
		}

		public static bool ShouldCheckUserAgent()
		{
		    return false;
		}

		public static void HandleApplicationOpenURL(string url)
		{
			
		}

        private static IJudoSDKApi _judoSdkApi;
        private static readonly ServiceFactory ServiceFactory = new ServiceFactory();
        private static readonly IPaymentService PaymentService = ServiceFactory.GetPaymentService();
        
        private static bool _uiMode { get; set; }
        
        public static bool UIMode 
        {
            get { return _uiMode; }
            set
            {
                if (value)
                    _judoSdkApi = new UIMethods(new ViewLocator(PaymentService));
                else
                    _judoSdkApi = new NonUIMethods(PaymentService);

                _uiMode = value;
            }
        }

        public static void Payment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            if (UIMode && navigationController == null)
            {
                var error = new JudoError { Exception = new Exception("Navigation controller cannot be null with UIMode enabled.") };
                failure(error);
            }
            else
            {
                _judoSdkApi.Payment(payment, success, failure, navigationController);
            }
        }

        public static void PreAuth(PreAuthorisationViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            if (UIMode && navigationController == null)
            {
                var error = new JudoError { Exception = new Exception("Navigation controller cannot be null with UIMode enabled.") };
                failure(error);
            }
            else
            {
                _judoSdkApi.PreAuth(preAuthorisation, success, failure, navigationController);
            }
        }


        public static void TokenPayment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            if (UIMode && navigationController == null)
            {
                var error = new JudoError { Exception = new Exception("Navigation controller cannot be null with UIMode enabled.") };
                failure(error);
            }
            else
            {
                _judoSdkApi.TokenPayment(payment, success, failure, navigationController);
            }
        }

        public static void TokenPreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            if (UIMode && navigationController == null)
            {
                var error = new JudoError { Exception = new Exception("Navigation controller cannot be null with UIMode enabled.") };
                failure(error);
            }
            else
            {
                _judoSdkApi.TokenPreAuth(payment, success, failure, navigationController);
            }
        }

        public static void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            if (UIMode && navigationController == null)
            {
                var error = new JudoError { Exception = new Exception("Navigation controller cannot be null with UIMode enabled.") };
                failure(error);
            }
            else
            {
                _judoSdkApi.RegisterCard(payment, success, failure, navigationController);
            }
        }

        public static void MakeATokenPayment(decimal amount, Dictionary<string, string> cardDetails, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, 
										UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public static void MakeAPreAuth(decimal amount, Dictionary<string, string> cardDetails, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, UIViewController parentViewController, 
									Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public static void MakeATokenPreAuth(decimal amount, Dictionary<string, string> cardDetails, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, 
										UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public static void RegisterCard(Card card, string consumerReference, string deviceID, UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public void GetPath(string path, Dictionary<string, string> parameters, Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public void PostPath(string path, Dictionary<string, string> parameters, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public void PutPath(string path, Dictionary<string, string> parameters, Action<string> successBlock, Action<string> failureBlock)
		{

		}
	}
}

