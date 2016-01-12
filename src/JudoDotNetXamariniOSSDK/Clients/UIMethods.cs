using System;
using JudoDotNetXamarin;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Clients
{
    internal class UIMethods : IJudoSDKApi
    {
        private readonly ViewLocator _viewLocator;

        public UIMethods (ViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }

        public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
        {
            var vc = GetCurrentViewController ();
		
            if (Judo.UIMode && vc == null) {
                var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
                failure (error);
            } else {
                var view = _viewLocator.GetPaymentView ();
                view.successCallback = success;
                view.failureCallback = failure;
                view.cardPayment = payment;

                PresentView (vc, view);
            }
        }


        public void PreAuth (PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure)
        {
            var vc = GetCurrentViewController ();

            if (Judo.UIMode && vc == null) {
                var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
                failure (error);
            } else {

                var view = _viewLocator.GetPreAuthView ();

                // register card and pre Auth sharing same view so we need to set this property to false
                view.successCallback = success;
                view.failureCallback = failure;
                view.authorisationModel = preAuthorisation;
                PresentView (vc, view);
            }
        }

        public void TokenPayment (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
        {

            var vc = GetCurrentViewController ();

            if (Judo.UIMode && vc == null) {
                var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
                failure (error);
            } else {
				
                var view = _viewLocator.GetTokenPaymentView ();
                view.successCallback = success;
                view.failureCallback = failure;
                view.tokenPayment = payment;
                PresentView (vc, view);
			
            }
        }

        public void TokenPreAuth (TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
        {
            var vc = GetCurrentViewController ();

            if (Judo.UIMode && vc == null) {
                var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
                failure (error);
            } else {
				
                var view = _viewLocator.GetTokenPreAuthView ();
                view.successCallback = success;
                view.failureCallback = failure;
                view.tokenPayment = payment;
                PresentView (vc, view);
            }
        }

        public void RegisterCard (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)
        {
            var vc = GetCurrentViewController ();

            if (Judo.UIMode && vc == null) {
                var error = new JudoError { Exception = new Exception ("Navigation controller cannot be null with UIMode enabled.") };
                failure (error);
            } else {
                var view = _viewLocator.GetRegisterCardView ();
                view.successCallback = success;
                view.failureCallback = failure;
                view.registerCardModel = payment;
                PresentView (vc, view);
            }
        }

        private void PresentView (UIViewController rootView, UIViewController view)
        {
            
           
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
                view.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
                view.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                rootView.PresentViewController (view, true, null);
            } else {
                var nv = new UINavigationController (view);
                nv.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
                rootView.PresentViewController (nv, true, null);
            }
        }

        UIViewController GetCurrentViewController ()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null) {
                vc = vc.PresentedViewController;
            }
            if (vc is UISplitViewController) {
                var splitView = vc as UISplitViewController;
                return splitView.ViewControllers [0];
            }
            return vc;
        }
    }
}