using System;
using System.Collections.Generic;
using JudoPayDotNet.Models;
using Newtonsoft.Json.Linq;
using System.Drawing;

using System.Diagnostics;
using JudoDotNetXamarinAndroidSDK;
using JudoDotNetXamarin;
using JudoDotNetXamarinAndroidSDK.Clients;
using Android.App;


namespace JudoDotNetXamarinAndroidSDK
{
    /// <summary>
    /// Callback for success transaction, this should be known blocking call
    /// </summary>

    public class JudoSDKManager
    {

        private static readonly Lazy<JudoSDKManager> _singleton = new Lazy<JudoSDKManager> (() => new JudoSDKManager ());

        public static JudoSDKManager Instance {
            get { return _singleton.Value; }
        }
        //internal static readonly UIFont FIXED_WIDTH_FONT_SIZE_20 = UIFont.FromName ("Courier", 17.0f);

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



        public JudoSDKManager ()
        {
            // setting up UI mode by default
            UIMode = true;
            RiskSignals = true;
        }


        //		internal static JObject GetClientDetails()
        //		{
        //			if(RiskSignals)
        //				return JObject.FromObject(ClientService.GetClientDetails());
        //
        //			return null;
        //		}

        //		public static string GetSDKVersion ()
        //		{
        //			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        //			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        //			string version = fvi.FileVersion;
        //
        //			return "Xamarin-Android-" + version;
        //		}

        private static JudoAndroidSDKAPI _judoSdkApi;
        //		private static readonly ServiceFactory ServiceFactory = new ServiceFactory ();
        //		private static readonly IPaymentService PaymentService = ServiceFactory.GetPaymentService ();


        private static bool _uiMode { get; set; }

        /// <summary>
        /// Enable UI Mode
        /// By default this property is set to True
        /// </summary>
        public static bool UIMode {
            get { return _uiMode; }
            set {
                if (value) {
                    _judoSdkApi = new UIMethods ();
                } else {
                    _judoSdkApi = new NonUIMethods ();

                    _uiMode = value;
                }
            }
        }

        /// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
        public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context)
        {
            var innerModel = payment.Clone ();
//            if (!UIMode) {
//                //var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
//                //failure (error);
//            } else {
            //   TestInterface test = new UiService ();
            _judoSdkApi.Payment (innerModel, success, failure, context);
//                var uiMethod = new UIMethods ();
//
//                uiMethod.Payment (innerModel, success, failure);
        }





        //   }

        //		/// <summary>
        //		/// Process a pre-authorisation payment
        //		/// </summary>
        //		/// <param name="preAuthorisation">PaymentViewModel to pass Amount and card detail</param>
        //		/// <param name="success">Callback for success transaction</param>
        //		/// <param name="failure">Callback for fail transaction</param>
        //		/// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
        //		public static void PreAuth (PaymentViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        //		{
        //			var innerModel = preAuthorisation.Clone ();
        //			if (UIMode && navigationController == null) {
        //				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
        //				failure (error);
        //			} else {
        //				_judoSdkApi.PreAuth (innerModel, success, failure, navigationController);
        //			}
        //		}
        //
        //		/// <summary>
        //		/// Process a card payment
        //		/// </summary>
        //		/// <param name="payment">TokenPaymentViewModel to pass Amount and Token detail</param>
        //		/// <param name="success">Callback for success transaction</param>
        //		/// <param name="failure">Callback for fail transaction</param>
        //		/// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
        //		public static void TokenPayment (TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        //		{
        //			var innerModel = payment.Clone ();
        //			if (UIMode && navigationController == null) {
        //				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
        //				failure (error);
        //			} else {
        //				_judoSdkApi.TokenPayment (innerModel, success, failure, navigationController);
        //			}
        //		}
        //
        //		/// <summary>
        //		/// Process a token authorisation payment
        //		/// </summary>
        //		/// <param name="payment">TokenPaymentViewModel to pass Amount and Token detail</param>
        //		/// <param name="success">Callback for success transaction</param>
        //		/// <param name="failure">Callback for fail transaction</param>
        //		/// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
        //		public static void TokenPreAuth (TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        //		{
        //			var innerModel = payment.Clone ();
        //			if (UIMode && navigationController == null) {
        //				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
        //				failure (error);
        //			} else {
        //				_judoSdkApi.TokenPreAuth (innerModel, success, failure, navigationController);
        //			}
        //		}
        //
        //		/// <summary>
        //		/// Process a RegisterCard
        //		/// </summary>
        //		/// <param name="registerCard">TokenPaymentViewModel to pass Amount and Token detail</param>
        //		/// <param name="success">Callback for success transaction</param>
        //		/// <param name="failure">Callback for fail transaction</param>
        //		/// <param name="navigationController">Navigation controller from UI this can be Null for non-UI Mode API</param>
        //		public static void RegisterCard (PaymentViewModel registerCard, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        //		{
        //			var innerModel = registerCard.Clone ();
        //			if (UIMode && navigationController == null) {
        //				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
        //				failure (error);
        //			} else {
        //				_judoSdkApi.RegisterCard(innerModel, success, failure, navigationController);
        //			}
        //		}
    }
}