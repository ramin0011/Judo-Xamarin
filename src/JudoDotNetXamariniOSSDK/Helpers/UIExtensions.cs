using System;
using UIKit;
using CoreGraphics;

namespace JudoDotNetXamariniOSSDK
{
	public static class UIExtensions
	{
		public static void Disable(this UIButton button)
		{
			button.Alpha = 0.25f;
			button.Enabled = false;
		}

		public static void Enable(this UIButton button)
		{
			button.Alpha = 1f;
			button.Enabled = true;
		}

		public static void RepositionFormSheetForiPad(this UIView superview)
		{
			superview.Bounds = new CGRect (0, 0, 320f, 460f);

			CGRect frame = superview.Frame;
			frame.Location = new CGPoint (frame.Location.X, 180f);
			superview.Frame = frame;
		}
	}
}

