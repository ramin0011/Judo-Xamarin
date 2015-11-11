using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
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
    internal interface IJudoSDKApi 
    {
		void Payment(PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void PreAuth(PaymentViewModel preAuthorisation, JudoSuccessCallback success, JudoFailureCallback failure);
		void TokenPayment(TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void TokenPreAuth(TokenPaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
		void RegisterCard(PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);

		void ApplePayment (ApplePayViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure,ApplePaymentType type);
    }
}