
using System;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public partial class ReassuringTextCell : CardCell
	{
		public static readonly UINib Nib = UINib.FromName ("ReassuringTextCell", NSBundle.MainBundle);
		//public static readonly NSString Key = new NSString ("");



		public ReassuringTextCell (IntPtr handle) : base (handle)
		{
			Key = "ReassuringTextCell";
		}

		public override CardCell Create ()
		{
			return (ReassuringTextCell)Nib.Instantiate (null, null) [0];
		}
	
	}
}
