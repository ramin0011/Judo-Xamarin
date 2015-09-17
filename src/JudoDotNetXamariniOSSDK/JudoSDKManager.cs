using System;
using System.Collections.Generic;
using CoreGraphics;
using JudoDotNetXamariniOSSDK.Clients;
using JudoDotNetXamariniOSSDK.Utils;
using JudoPayDotNet.Models;
using Newtonsoft.Json.Linq;
using UIKit;
using Environment = JudoPayDotNet.Enums.Environment;

namespace JudoDotNetXamariniOSSDK
{
    /// <summary>
    /// Callback for success transaction, this should be known blocking call
    /// </summary>
    public delegate void SuccessCallback(PaymentReceiptModel receipt);

    /// <summary>
    /// Callback for fail transaction, this should be known blocking call
    /// </summary>
    public delegate void FailureCallback(JudoError error, PaymentReceiptModel receipt = null);

	public class JudoSDKManager
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

        // get the top most view 
		private static UIView appView = UIApplication.SharedApplication.Windows [0].RootViewController.View;

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
		private static LoadingOverlay _loadPop;

		private static bool _uiMode { get; set; }

        /// <summary>
        /// Enable UI Mode
        /// By default this property is set to True
        /// </summary>
		public static bool UIMode {
			get { return _uiMode; }
			set {
			    if (value)
			        _judoSdkApi = new UIMethods(new ViewLocator(PaymentService));
			    else
			        _judoSdkApi = new NonUIMethods(PaymentService);

			    _uiMode = value;
			}
		}

        /// <summary>
        /// shows loading screen while processing payment
        /// </summary>
        /// <param name="view"></param>
		internal static void ShowLoading (UIView view)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				_loadPop = new LoadingOverlay (new CGRect ((view.Frame.Width / 2) - 75f, (view.Frame.Height / 2) - 75f, 150f, 150f), true);
			} else {
                view = UIApplication.SharedApplication.Windows[0].RootViewController.View;
                _loadPop = new LoadingOverlay();
			}
			view.Add (_loadPop);
		}

        /// <summary>
        /// hides loading screen while processing payment
        /// </summary>
		internal static void HideLoading ()
		{
			if (_loadPop != null)
            {
				_loadPop.Hide (appView);
                _loadPop.Dispose();
            }
		}

        /// <summary>
        /// Process a card payment
        /// </summary>
        /// <param name="payment">PaymentViewModel to pass Amount and card detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public static void Payment (PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.Payment (payment, success, failure, navigationController);
			}
		}

        /// <summary>
        /// Process a pre-authorisation payment
        /// </summary>
        /// <param name="preAuthorisation">PaymentViewModel to pass Amount and card detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public static void PreAuth (PaymentViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.PreAuth (preAuthorisation, success, failure, navigationController);
			}
		}

        /// <summary>
        /// Process a card payment
        /// </summary>
        /// <param name="payment">TokenPaymentViewModel to pass Amount and Token detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public static void TokenPayment (TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.TokenPayment (payment, success, failure, navigationController);
			}
		}

        /// <summary>
        /// Process a token authorisation payment
        /// </summary>
        /// <param name="payment">TokenPaymentViewModel to pass Amount and Token detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public static void TokenPreAuth (TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.TokenPreAuth (payment, success, failure, navigationController);
			}
		}

        /// <summary>
        /// Process a RegisterCard
        /// </summary>
        /// <param name="registerCard">TokenPaymentViewModel to pass Amount and Token detail</param>
        /// <param name="success">Callback for success transaction</param>
        /// <param name="failure">Callback for fail transaction</param>
        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
		public static void RegisterCard (PaymentViewModel registerCard, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
                _judoSdkApi.RegisterCard(registerCard, success, failure, navigationController);
			}
		}

    }
}

