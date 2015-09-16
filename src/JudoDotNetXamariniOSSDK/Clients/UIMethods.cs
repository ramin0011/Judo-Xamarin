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
            navigationController.PushViewController(view, true);
        }


        public void PreAuth(PaymentViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetPreAuthView();

            // register card and pre Auth sharing same view so we need to set this property to false
            view.RegisterCardOnly = false;
            view.successCallback = success;
            view.failureCallback = failure;
            view.authorisationModel = preAuthorisation;
            navigationController.PushViewController(view, true);
        }

        public void TokenPayment(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetTokenPaymentView();
            view.successCallback = success;
            view.failureCallback = failure;
            view.tokenPayment = payment;
            navigationController.PushViewController(view, true);
        }

        public void TokenPreAuth(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetTokenPreAuthView();
            view.successCallback = success;
            view.failureCallback = failure;
            view.tokenPayment = payment;
            navigationController.PushViewController(view, true);
        }

        public void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController)
        {
            var view = _viewLocator.GetPreAuthView();
            // register card and pre Auth sharing same view so we need to set this property to true
            view.RegisterCardOnly = true;
            view.successCallback = success;
            view.failureCallback = failure;
            view.authorisationModel = payment;
            navigationController.PushViewController(view, true);
        }
    }
}