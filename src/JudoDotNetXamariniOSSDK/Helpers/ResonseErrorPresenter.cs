using System;
using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using CoreFoundation;
using JudoPayDotNet.Models;


namespace JudoDotNetXamariniOSSDK
{
	public class ResponseErrorPresenter :IErrorPresenter
	{
		public ResponseErrorPresenter ()
		{
		}



		public void DisplayError (string test)
		{
			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						
				var errorText = "";//result.Error.ErrorMessage;
				UIAlertView _error = new UIAlertView ("Token Payment has failed", errorText, null, "ok", null);
				_error.Show ();

			});
		}

	}
}

