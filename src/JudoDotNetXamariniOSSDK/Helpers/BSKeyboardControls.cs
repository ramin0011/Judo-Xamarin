using System;
using UIKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;

namespace JudoDotNetXamariniOSSDK
{

	public interface IBSkeyboardControlsEvents
	{
		void FieldSelection(BSKeyboardControls keyboardControls, UIView field, BSKeyboardControlsDirection direction);
		void DonePressed(BSKeyboardControls keyboardControls);
	}
	
	public class BSKeyboardControls : UIView
	{
		public UISegmentedControl SegmentedControl { get; set;}
		public IBSkeyboardControlsEvents VisibleControls { get; set;}
		public NSArray Fields { get; set;}
		public UIView ActiveField { get; set;}
		public UIBarStyle BarStyle { get; set; }
		public UIColor BarTintColor { get; set; }
		public UIColor SegmentedControlTintControl { get; set;}
		public string PreviousTitle { get; set; }
		public string NextTitle { get; set; }
		public string DoneTitle { get; set; }
		public UIColor DoneTintColor { get; set; }
		private UIToolbar Toolbar { get; set;}
		private UIBarButtonItem DoneButton { get; set; }
		private UIBarButtonItem SegmentedControlItem { get; set; }

		public BSKeyboardControls (NSArray fields) : base(new CGRect(0.0f, 0.0f, 320.0f, 44.0f))
		{
			Toolbar = new UIToolbar (Frame);
			Toolbar.TintColor = UIColor.FromWhiteAlpha (0.961f, 1);
			AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			AddSubview (Toolbar);

			SegmentedControl.AddTarget (this, new Selector (segmentedControlValueChanged), UIControlEvent.ValueChanged);
			SegmentedControl.Momentary = true;
			SegmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			SegmentedControl.SetEnabled (false, (int)BSKeyboardControlsDirection.DirectionPrevious);
			SegmentedControl.SetEnabled (false, (int)BSKeyboardControlsDirection.DirectionNext);
			SegmentedControlItem = new UIBarButtonItem (SegmentedControl);

			UIImage buttonImage = new UIImage ("BTN_done_normal");

			UIBarButtonItem doneButton = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Bordered, this, new Selector (doneButtonPressed));
			doneButton.SetBackgroundImage (buttonImage, UIControlState.Normal, UIBarMetrics.Default);
			doneButton.AccessibilityLabel = "Done Button";

			var titleTextAttributes = new UITextAttributes ();
			titleTextAttributes.TextColor = UIColor.FromWhiteAlpha (0.369f, 1);
			titleTextAttributes.TextShadowColor = UIColor.FromRGBA (0, 0, 0, 0.8);
			titleTextAttributes.TextShadowOffset = new UIOffset (0, 0);
			titleTextAttributes.Font = UIFont.FromName ("Proxima Nova Regular", 10);


			doneButton.SetTitleTextAttributes(
		}

		private void segmentedControlValueChanged(object sender)
		{

		}

		private void doneButtonPressed(object sender)
		{

		}


	}
}

