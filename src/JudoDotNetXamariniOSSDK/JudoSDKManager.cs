using System;
using CoreLocation;
using System.Collections.Generic;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public class JudoSDKManager
	{
		internal static readonly UIFont FIXED_WIDTH_FONT_SIZE_20 = UIFont.FromName("Courier", 17.0f);

		Card currentCard {get; set;}
		CLLocation location {get; set;}
		Dictionary<string, string> clientDetails {get; set;}
		public static bool LocationEnabled{ get; set; }
		public static bool ThreeDSecureEnabled{ get; set; }
		public static bool AVSEnabled { get; set; }
		public static bool AmExAccepted { get; set; }
		public static bool MaestroAccepted { get; set; }
        private static UIWindow window;

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

		public static CreditCardController GetCreditCardController()
		{
		    return null;
		}

		//TODO: correct the parameter for failureBlock action to be something meaningful instead of a string
		public static void MakeAPaymentCustomUI(decimal amount, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, UIViewController viewController, 
								 Card card, UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public static void MakeAPayment(decimal amount, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, 
									UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{
            // call UI 
			ServiceFactory serviceFactory = new ServiceFactory();
			IPaymentService paymentService = serviceFactory.GetPaymentService ();

			CreditCardView2 ctrl = new CreditCardView2(paymentService);
            // create a new window instance based on the screen size
           window = window ?? new UIWindow(UIScreen.MainScreen.Bounds);

           // var controller = new UIViewController();
            //controller.View.BackgroundColor = UIColor.White;

			window.RootViewController = ctrl;
			//UIApplication.SharedApplication.Windows[0].RootViewController.NavigationController.PushViewController(ctrl,true);

            // make the window visible
            window.MakeKeyAndVisible();
        }

		public static void ShowReceipt(PaymentReceiptViewModel receipt)
		{
			PaymentReceipt receiptView = new PaymentReceipt (receipt);
			UIApplication.SharedApplication.Windows[0].RootViewController.NavigationController.PushViewController (receiptView, true);

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

		void SetAVSEnabled(bool enabled)
		{
			AVSEnabled = enabled;
		}

		bool GetAVSEnabled()
		{
			return AVSEnabled;
		}
			
	}
}

