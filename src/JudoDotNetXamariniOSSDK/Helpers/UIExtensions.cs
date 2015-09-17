using System;
using UIKit;

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
	}
}

