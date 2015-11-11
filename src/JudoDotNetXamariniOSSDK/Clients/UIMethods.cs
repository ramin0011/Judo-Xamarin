using System;
using System.Runtime.Remoting.Contexts;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;

#if __UNIFIED__
using Foundation;
using UIKit;
using CoreFoundation;
using CoreGraphics;

// Mappings Unified CoreGraphic classes to MonoTouch classes
using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;

#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace JudoDotNetXamariniOSSDK
{
	internal class UIMethods :ApplePayMethods, IJudoSDKApi
	{
		private readonly ViewLocator _viewLocator;

		public UIMethods (IApplePayService applePayService, ViewLocator viewLocator) : base (applePayService)
		{
			_viewLocator = viewLocator;
		}

		public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();
		
			if (JudoSDKManager.UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				var view = _viewLocator.GetPaymentView ();
				view.successCallback = success;
				view.failureCallback = failure;
				view.cardPayment = payment;

				PresentView (vc as UINavigationController, view);
			}
		}


		public void PreAuth (PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();

			if (JudoSDKManager.UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {

				var view = _viewLocator.GetPreAuthView ();

				// register card and pre Auth sharing same view so we need to set this property to false
				view.successCallback = success;
				view.failureCallback = failure;
				view.authorisationModel = preAuthorisation;
				PresentView (vc as UINavigationController, view);
			}
		}

		public void TokenPayment (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{

			var vc = GetCurrentViewController ();

			if (JudoSDKManager.UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				
				var view = _viewLocator.GetTokenPaymentView ();
				view.successCallback = success;
				view.failureCallback = failure;
				view.tokenPayment = payment;
				PresentView (vc as UINavigationController, view);
			
			}
		}

		public void TokenPreAuth (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();

			if (JudoSDKManager.UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				
				var view = _viewLocator.GetTokenPreAuthView ();
				view.successCallback = success;
				view.failureCallback = failure;
				view.tokenPayment = payment;
				PresentView (vc as UINavigationController, view);
			}
		}

		public void RegisterCard (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
		{
			var vc = GetCurrentViewController ();

			if (JudoSDKManager.UIMode && vc == null) {
				var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
				failure (error);
			} else {
				var view = _viewLocator.GetRegisterCardView ();
				view.successCallback = success;
				view.failureCallback = failure;
				view.registerCardModel = payment;
				PresentView (vc as UINavigationController, view);
			}
		}

		private void PresentView (UINavigationController navigationController, UIViewController view)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				view.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				view.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
				navigationController.PresentViewController (view, true, null);
			} else {
				navigationController.PushViewController (view, true);
			}
		}

		UIViewController GetCurrentViewController ()
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			var vc = window.RootViewController;
			while (vc.PresentedViewController != null) {
				vc = vc.PresentedViewController;
			}
			return vc;
		}
	}
}