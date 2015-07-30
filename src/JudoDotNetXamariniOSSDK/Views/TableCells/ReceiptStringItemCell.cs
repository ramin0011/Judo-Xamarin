
using System;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public partial class ReceiptStringItemCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("ReceiptStringItemCell");
		public static readonly UINib Nib;
		public string Label {get{return ItemLabel.Text; } set{ItemLabel.Text = value; }}
		public string Value {get{return ItemValue.Text; } set{ItemValue.Text = value; }}


		static ReceiptStringItemCell ()
		{
			Nib = UINib.FromName ("ReceiptStringItemCell", NSBundle.MainBundle);
		}

		public ReceiptStringItemCell (IntPtr handle) : base (handle)
		{
		}

		public static ReceiptStringItemCell Create ()
		{
			return (ReceiptStringItemCell)Nib.Instantiate (null, null) [0];
		}
	}
}

