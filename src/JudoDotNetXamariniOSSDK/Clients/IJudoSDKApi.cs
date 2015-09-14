using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using JudoPayDotNet.Models;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Clients
{
    public interface IJudoSDKApi
    {
        void Payment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void PreAuth(PreAuthorisationViewModel preAuthorisation, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void TokenPayment(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void TokenPreAuth(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
        void RegisterCard(PaymentViewModel payment, SuccessCallback success, FailureCallback failure, UINavigationController navigationController);
    }
}