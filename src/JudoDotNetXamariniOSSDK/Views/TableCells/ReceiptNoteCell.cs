
using System;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public partial class ReceiptNoteCell : UITableViewCell
	{

		public string Text { get; set;}



		public ReceiptNoteCell (IntPtr handle) : base (handle)
		{
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

