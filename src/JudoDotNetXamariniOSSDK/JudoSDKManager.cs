using System;
using CoreLocation;
using System.Collections.Generic;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
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
			

		public static void MakeAPaymentCustomUI(decimal amount, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, UIViewController viewController, 
								 Card card, UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public static CreditCardView GetPaymentView()
		{

			ServiceFactory serviceFactory = new ServiceFactory();
			IPaymentService paymentService = serviceFactory.GetPaymentService ();

			CreditCardView ctrl = new CreditCardView(paymentService);

			return ctrl;
        }

		public static PaymentReceipt GetReceiptView(PaymentReceiptViewModel receipt)
		{
			PaymentReceipt receiptView = new PaymentReceipt (receipt);
			return receiptView;

		}
			

		public static PreAuthorisationView GetPreAuthView ()
		{
			ServiceFactory serviceFactory = new ServiceFactory();
			IPaymentService paymentService = serviceFactory.GetPaymentService ();

			PreAuthorisationView ctrl = new PreAuthorisationView(paymentService);

			return ctrl;
		}

		public static TokenPaymentView GetTokenPaymentView ()
		{
			ServiceFactory serviceFactory = new ServiceFactory();
			IPaymentService paymentService = serviceFactory.GetPaymentService ();

			TokenPaymentView ctrl = new TokenPaymentView(paymentService);

			return ctrl;
		}

		public static TokenPreAuthorisationView GetTokenPreAuthView ()
		{
			ServiceFactory serviceFactory = new ServiceFactory();
			IPaymentService paymentService = serviceFactory.GetPaymentService ();

			TokenPreAuthorisationView ctrl = new TokenPreAuthorisationView(paymentService);

			return ctrl;
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

