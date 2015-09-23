using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using JudoPayDotNet.Models;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Clients
{
    internal interface IJudoSDKApi
    {
        void Payment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void PreAuth(PaymentViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void TokenPayment(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void TokenPreAuth(TokenPaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
    }
}