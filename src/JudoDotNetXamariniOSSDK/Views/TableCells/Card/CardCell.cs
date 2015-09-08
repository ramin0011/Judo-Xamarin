using System;
using UIKit;
using Foundation;

namespace JudoDotNetXamariniOSSDK
{
	public abstract  class CardCell : UITableViewCell
	{
		public string Key{ get; set; }

		public Action UpdateUI{ get; set; }

		public CardCell (IntPtr handle) : base (handle)
		{
		}

		public abstract CardCell Create ();

		public abstract void SetUpCell ();

		public abstract void DismissKeyboardAction();

	}


}

