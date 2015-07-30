
using System;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public partial class ReceiptStringItemCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("ReceiptStringItemCell");
		public static readonly UINib Nib;

		public string Label { get; set;}
		public string Value { get; set;}


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

		public override void RemoveFromSuperview ()
		{
			base.RemoveFromSuperview ();
		}

//		public override void LayoutSubviews ()
//		{
//			base.LayoutSubviews ();
//			ItemLabel.Text = this.Label;
//			ItemValue.Text = this.Value;
//
//		}
	}
}

