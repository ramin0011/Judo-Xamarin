using System;
using PassKit;

namespace JudoDotNetXamariniOSSDK
{
	public class JudoPKPaymentViewControllerDelegate : PKPaymentAuthorizationViewControllerDelegate
	{
		public JudoPKPaymentViewControllerDelegate ()
		{
		}
		public override void DidAuthorizePayment (PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
		{
			throw new NotImplementedException ();
		}
	}
}

