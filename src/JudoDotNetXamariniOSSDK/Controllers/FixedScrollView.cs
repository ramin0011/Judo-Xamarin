﻿using System;
using UIKit;
using Foundation;

namespace JudoDotNetXamariniOSSDK
{
	[Register("FixedScrollView")]
	public class FixedScrollView :UIScrollView
	{


		public FixedScrollView(IntPtr p) : base(p)
		{
		}
		public override void ScrollRectToVisible (CoreGraphics.CGRect rect, bool animated)
		{
			
		}
			
	}
}
