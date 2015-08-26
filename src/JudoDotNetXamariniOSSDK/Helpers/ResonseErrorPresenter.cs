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



		public void DisplayError (IResult<ITransactionResult> result,string failHeader)
		{
			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
				var errorText = "No Response from Server";
				if(result!=null)
				{
					errorText = result.Response.Message;
				}

				UIAlertView _error = new UIAlertView (failHeader, errorText, null, "ok", null);
				_error.Show ();
			});
		}

	}
}

