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
	public delegate void SuccessCallback (PaymentReceiptModel receipt);
	public delegate void FailureCallback (JudoError error, PaymentReceiptModel receipt = null);

	public class JudoSDKManager
	{
		internal static readonly UIFont FIXED_WIDTH_FONT_SIZE_20 = UIFont.FromName ("Courier", 17.0f);

		public static bool ThreeDSecureEnabled{ get; set; }

		public static bool AVSEnabled { get; set; }

		public static bool AmExAccepted { get; set; }

		public static bool MaestroAccepted { get; set; }

		public static bool RiskSignals{ get; set; }
		
        	public static bool SSLPinningEnabled { get; set; }

		private static UIView appView = UIApplication.SharedApplication.Windows [0].RootViewController.View;

		private static readonly Lazy<JudoSDKManager> _singleton = new Lazy<JudoSDKManager> (() => new JudoSDKManager ());

		public static JudoSDKManager Instance {
			get { return _singleton.Value; }
		}


        internal static JObject GetClientDetails()
        {
            if(RiskSignals)
                return JObject.FromObject(ClientDetailsProvider.GetClientDetails());

            return null;
        }

		public static void SetUserAgent ()
		{
			
		}

		public static bool ShouldCheckUserAgent ()
		{
			return false;
		}

		public static void HandleApplicationOpenURL (string url)
		{
			
		}

		private static IJudoSDKApi _judoSdkApi;
		private static readonly ServiceFactory ServiceFactory = new ServiceFactory ();
		private static readonly IPaymentService PaymentService = ServiceFactory.GetPaymentService ();
		private static LoadingOverlay _loadPop;

		private static bool _uiMode { get; set; }

		public static bool UIMode {
			get { return _uiMode; }
			set {
				if (value)
					_judoSdkApi = new UIMethods (new ViewLocator (PaymentService));
				else
					_judoSdkApi = new NonUIMethods (PaymentService);

				_uiMode = value;
			}
		}

		internal static void ShowLoading (UIView view = null)
		{
			if (view == null) {
				view = UIApplication.SharedApplication.Windows [0].RootViewController.View;
			}

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				_loadPop = new LoadingOverlay (new CGRect ((view.Frame.Width / 2) - 75f, (view.Frame.Height / 2) - 75f, 150f, 150f), true);
			} else {
				_loadPop = new LoadingOverlay ();
			}
			view.Add (_loadPop);
		}

		internal static void HideLoading ()
		{
			if (_loadPop != null)
            {
				_loadPop.Hide (appView);
                _loadPop.Dispose();
            }
		}

		public static void Payment (PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.Payment (payment, success, failure, navigationController);
			}
		}

		public static void PreAuth (PaymentViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.PreAuth (preAuthorisation, success, failure, navigationController);
			}
		}


		public static void TokenPayment (TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.TokenPayment (payment, success, failure, navigationController);
			}
		}

		public static void TokenPreAuth (TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.TokenPreAuth (payment, success, failure, navigationController);
			}
		}

		public static void RegisterCard (PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
		{
			if (UIMode && navigationController == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				_judoSdkApi.RegisterCard (payment, success, failure, navigationController);
			}
		}

    }
}

