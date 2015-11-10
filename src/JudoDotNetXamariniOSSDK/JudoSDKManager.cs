using System;
using System.Collections.Generic;
using JudoPayDotNet.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using JudoDotNetXamarin;
using UIKit;
using JudoDotNetXamariniOSSDK;

namespace JudoDotNetXamariniOSSDK
{
    /// <summary>
    /// Callback for success transaction, this should be known blocking call
    /// </summary>
	/// 
	/// 
    //public delegate void SuccessCallback(PaymentReceiptModel receipt);

    /// <summary>
    /// Callback for fail transaction, this should be known blocking call
    /// </summary>
   // public delegate void FailureCallback(JudoError error, PaymentReceiptModel receipt = null);


	public class JudoSDKManager : IJudoSDKManager
	{
		internal static readonly UIFont FIXED_WIDTH_FONT_SIZE_20 = UIFont.FromName ("Courier", 17.0f);

        /// <summary>
        /// Enable 3D security process
        /// </summary>
		public static bool ThreeDSecureEnabled{ get; set; }

        /// <summary>
        /// Enable/Disable AVS check
        /// </summary>
		public static bool AVSEnabled { get; set; }

        /// <summary>
        /// Enable/Disable Amex card support
        /// </summary>
		public static bool AmExAccepted { get; set; }

        /// <summary>
        /// Enable/Disable Mestro card support
        /// </summary>
		public static bool MaestroAccepted { get; set; }


        /// <summary>
        /// Enable/Disable risk signal to pass fruad monitoring device data
        /// default is true
        /// </summary>
		public static bool RiskSignals{ get; set; }
		
        /// <summary>
        /// SSLPinningEnabled
        /// </summary>
        public static bool SSLPinningEnabled { get; set; }

       

		private static readonly Lazy<JudoSDKManager> _singleton = new Lazy<JudoSDKManager> (() => new JudoSDKManager ());

        public static JudoSDKManager Instance
        {
            get { return _singleton.Value; }
        }

        static JudoSDKManager()
        {
            // setting up UI mode by default
            UIMode = true;
            RiskSignals = true;
        }


        internal static JObject GetClientDetails()
        {
            if(RiskSignals)
                return JObject.FromObject(ClientDetailsProvider.GetClientDetails());

            return null;
        }


		internal static void SetUserAgent ()
		{
			
		}

		public static bool ShouldCheckUserAgent ()
		{
			return false;
		}

        private static IJudoSDKApi _judoSdkApi;
		private static readonly ServiceFactory ServiceFactory = new ServiceFactory ();
		private static readonly IPaymentService PaymentService = ServiceFactory.GetPaymentService ();
		private static readonly IApplePayService ApplePaymentService = ServiceFactory.GetApplePaymentService ();


		private static bool _uiMode { get; set; }

        /// <summary>
        /// Enable UI Mode
        /// By default this property is set to True
        /// </summary>
		public static bool UIMode {
			get { return _uiMode; }
			set {
			    if (value)
					_judoSdkApi = new UIMethods(ApplePaymentService,new ViewLocator(PaymentService));
			    else
					_judoSdkApi = new NonUIMethods(ApplePaymentService,PaymentService);

			    _uiMode = value;
			}
		}
       
        /// <summary>
        /// Process a card payment
        /// </summary>
        /// <param name="payment">PaymentViewModel to pass Amount and card detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
			var innerModel = payment.Clone ();
			if (UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				
				_judoSdkApi.Payment (innerModel, success, failure, vc as UINavigationController);
			}
		}

        /// <summary>
        /// Process a pre-authorisation payment
        /// </summary>
        /// <param name="preAuthorisation">PaymentViewModel to pass Amount and card detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public  void PreAuth (PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
			var innerModel = preAuthorisation.Clone ();
			if (UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.PreAuth (innerModel, success, failure, vc as UINavigationController);
			}
		}

        /// <summary>
        /// Process a card payment
        /// </summary>
        /// <param name="payment">TokenPaymentViewModel to pass Amount and Token detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public void TokenPayment (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
			var innerModel = payment.Clone ();
			if (UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.TokenPayment (innerModel, success, failure, vc as UINavigationController);
			}
		}

        /// <summary>
        /// Process a token authorisation payment
        /// </summary>
        /// <param name="payment">TokenPaymentViewModel to pass Amount and Token detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public void TokenPreAuth (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
			var innerModel = payment.Clone ();
			if (UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.TokenPreAuth (innerModel, success, failure, vc as UINavigationController);
			}
		}

        /// <summary>
        /// Process a RegisterCard
        /// </summary>
        /// <param name="registerCard">TokenPaymentViewModel to pass Amount and Token detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public void RegisterCard (PaymentViewModel registerCard, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
			var innerModel = registerCard.Clone ();
			if (UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.RegisterCard(innerModel, success, failure, vc as UINavigationController);
			}
		}

		public void MakeApplePayment (ApplePayViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();

			if (vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null.") };
				failure (error);
			} else {
				_judoSdkApi.ApplePayment(payment,success,failure,vc as UINavigationController,ApplePaymentType.Payment);
			}

		}

		public void MakeApplePreAuth (ApplePayViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
			if (vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null.") };
				failure (error);
			} else {
				_judoSdkApi.ApplePayment(payment,success,failure,vc as UINavigationController,ApplePaymentType.PreAuth);
			}


		}

	
		UIViewController GetCurrentViewController ()
		{
			var window= UIApplication.SharedApplication.KeyWindow;
			var vc = window.RootViewController;
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}
			return vc;
		}
    }
}

