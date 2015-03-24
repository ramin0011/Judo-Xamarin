using System;
using UIKit;
using CoreGraphics;

namespace JudoDotNetXamariniOSSDK
{
	public class PlaceHolderTextView : UIView
	{
		public string Text { get; set;}
		public UIFont Font { get; set;}
		public int ShowTextOffess { get; set; }
		public CGRect Offset { get; set;}

		public float WidthToOffset { get; set; }
		public float WidthFromOffset { get; set; }

		public PlaceHolderTextView (CGRect frame) : base(frame)
		{
			
		}
	}
}

