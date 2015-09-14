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
            navigationController.PushViewController(view, true);
        }


        public void PreAuth(PreAuthorisationViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetPreAuthView();
            view.successCallback = success;
            view.failureCallback = failure;
            navigationController.PushViewController(view, true);
        }

        public void TokenPayment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetTokenPaymentView();
            view.successCallback = success;
            view.failureCallback = failure;
            navigationController.PushViewController(view, true);
        }

        public void TokenPreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetTokenPreAuthView();
            view.successCallback = success;
            view.failureCallback = failure;
            navigationController.PushViewController(view, true);
        }

        public void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            //var view = _viewLocator.GetPaymentView();
            //view.successCallback = success;
            //view.failureCallback = failure;
            //navigationController.PushViewController(view, true);
        }
    }
}