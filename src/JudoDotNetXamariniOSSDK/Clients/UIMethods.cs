using System;
using System.Runtime.Remoting.Contexts;
using JudoPayDotNet.Models;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Clients
{
    internal class UIMethods : IJudoSDKApi
    {
        private readonly ViewLocator _viewLocator;
        public UIMethods(ViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }

        public void Payment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetPaymentView();
            view.successCallback = success;
            view.failureCallback = failure;
            view.cardPayment = payment;

			PresentView (navigationController, view);
           
        }


        public void PreAuth(PaymentViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetPreAuthView();

            // register card and pre Auth sharing same view so we need to set this property to false
            view.successCallback = success;
            view.failureCallback = failure;
            view.authorisationModel = preAuthorisation;
			PresentView (navigationController, view);
        }

        public void TokenPayment(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetTokenPaymentView();
            view.successCallback = success;
            view.failureCallback = failure;
            view.tokenPayment = payment;
			PresentView (navigationController, view);
        }

        public void TokenPreAuth(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetTokenPreAuthView();
            view.successCallback = success;
            view.failureCallback = failure;
            view.tokenPayment = payment;
			PresentView (navigationController, view);
        }

        public void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetRegisterCardView();
            view.successCallback = success;
            view.failureCallback = failure;
            view.registerCardModel = payment;
			PresentView (navigationController, view);
        }

		private void PresentView (UINavigationController navigationController, UIViewController view)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				view.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				view.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				navigationController.PresentViewController (view, true, null);
			}
			else {
				navigationController.PushViewController (view, true);
			}
		}
    }
}