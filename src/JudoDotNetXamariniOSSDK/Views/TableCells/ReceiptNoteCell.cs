
using System;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public partial class ReceiptNoteCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("ReceiptNoteCell");
		public static readonly UINib Nib;
		public string Text { get; set;}

		static ReceiptNoteCell ()
		{
			Nib = UINib.FromName ("ReceiptNoteCell", NSBundle.MainBundle);
		}

		public ReceiptNoteCell (IntPtr handle) : base (handle)
		{
		}

		public static ReceiptNoteCell Create ()
		{
			return (ReceiptNoteCell)Nib.Instantiate (null, null) [0];

		}
		public override void RemoveFromSuperview ()
		{
			base.RemoveFromSuperview ();
		}

//		public override void LayoutSubviews ()
//		{
//
//		
//			base.LayoutSubviews ();
//			NoteText.Text = this.Text;
//		}

	}
}

