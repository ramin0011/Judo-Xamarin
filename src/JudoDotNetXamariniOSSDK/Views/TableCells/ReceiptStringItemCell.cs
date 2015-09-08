
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

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
			
		static ReceiptStringItemCell ()
		{
			Nib = UINib.FromName (UserInterfaceIdiomIsPhone ? "ReceiptStringItemCell_iphone":"ReceiptStringItemCell_ipad", NSBundle.MainBundle);

		}

		public ReceiptStringItemCell() : base()
		{
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

