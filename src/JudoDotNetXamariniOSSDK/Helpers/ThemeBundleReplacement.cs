using System;
using System.Threading;

#if__UNIFIED__
using Foundation;
using UIKit;
using CoreFoundation;
using CoreGraphics;
// Mappings Unified CoreGraphic classes to MonoTouch classes
using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace JudoDotNetXamariniOSSDK
{
	public enum BundledOrReplacementOptions { Bundled, BundledOrReplacement };

	public static class ThemeBundleReplacement
	{

		private static NSBundle frameworkBundle;

		private static NSBundle getFrameworkBundle() 
		{
			NSBundle bundle;
			//Check if the bundle was already loaded
			if ((bundle = Volatile.Read(ref frameworkBundle))!= null) 
			{
				var path = NSBundle.MainBundle.ResourcePath;
				var frameworkBundlePath = path + "JudoPay.bundle";

				//Try to set the bundle value, if the bundle was already loaded, discard this load and use the existing, else safely read the existing
				if (Interlocked.CompareExchange (ref frameworkBundle, NSBundle.FromPath (frameworkBundlePath), null) == null) 
				{
					bundle = frameworkBundle;
				} 
				else 
				{
					bundle = Volatile.Read (ref frameworkBundle);
				}
					
			}

			return bundle;
		}

		private static UIColor getColor(int hex)
		{
			const int mask = 0xFF;
			nfloat divisor = 255.0f;

			nfloat r = (hex >> 24) & mask;
            nfloat g = (hex >> 16) & mask;
            nfloat b = (hex >> 8) & mask;
            nfloat a = hex & mask;

            return UIColor.FromRGB(r/divisor, g / divisor, b / divisor);
		}

		private static UIColor getColor(string color)
		{
			int hex;
			if (int.TryParse (color, out hex)) {
				return getColor(hex);
			}

			return null;
		}

		public static UIImage BundledOrReplacementImage(string imageName, BundledOrReplacementOptions bundledOrReplacement)
		{
			const string pngPrefix = ".png";
			string name = imageName;

			switch (bundledOrReplacement) 
			{
				case BundledOrReplacementOptions.BundledOrReplacement:
				{
					if (name.EndsWith (pngPrefix)) 
					{
						name = imageName.Substring(0, imageName.Length - pngPrefix.Length);
					}
						
					var image = UIImage.FromFile (name);

					if (image != null) 
					{
						return image;
					}
				}
                break;
				case BundledOrReplacementOptions.Bundled:
				{
					return UIImage.FromBundle (getFrameworkBundle().PathForResource (imageName, "png"));
				}
					
			}

		    return null;
		}

		public static string BundledOrReplacementString(string stringName, BundledOrReplacementOptions bundledOrReplacement)
		{
			switch(bundledOrReplacement)
			{
				case BundledOrReplacementOptions.BundledOrReplacement:
				{
					var replacedString = NSBundle.MainBundle.LocalizedString (stringName, null, "JudoName");

					if (replacedString != stringName) 
					{
						return replacedString;
					}
				}
                    break;
				case BundledOrReplacementOptions.Bundled:
				{
					return getFrameworkBundle().LocalizedString (stringName, stringName, "JudoTheme", null);
				}
			}

		    return null;
		}

		public static UIColor BundledOrReplacementColor(string colourName, BundledOrReplacementOptions bundledOrReplacement)
		{
			switch(bundledOrReplacement)
			{
				case BundledOrReplacementOptions.BundledOrReplacement:
				{
					return getColor(BundledOrReplacementString(colourName, BundledOrReplacementOptions.BundledOrReplacement));
				}
				case BundledOrReplacementOptions.Bundled:
				{
					return getColor(BundledOrReplacementString(colourName, BundledOrReplacementOptions.Bundled));
				}
			}

		    return null;
		}



	}
}

