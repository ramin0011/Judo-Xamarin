﻿using System;
using System.Drawing;
using System.Collections.Generic;

#if__UNIFIED__
using Foundation;
using UIKit;
using CoreFoundation;
using CoreAnimation;
using CoreGraphics;
using ObjCRuntime;
// Mappings Unified CoreGraphic classes to MonoTouch classes
using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using MonoTouch.CoreAnimation;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace JudoDotNetXamariniOSSDK
{
	[Register ("SlideUpMenu")]
	public partial class SlideUpMenu :UIView
	{
		bool ComponentExpanded;
		UIPanGestureRecognizer panGesture;

		public SlideUpMenu (IntPtr p) : base (p)
		{
			
		}

		public SlideUpMenu (RectangleF frame) : base (frame)
		{
			this.Frame = frame;
		}

		public override void WillMoveToSuperview (UIView newsuper)
		{
			
			base.WillMoveToSuperview (newsuper);
			SetUpToggle (AVSSwitch, JudoSDKManager.AVSEnabled, () => {
				JudoSDKManager.AVSEnabled = !JudoSDKManager.AVSEnabled;
			});
			SetUpToggle (ThreeDSwitch, JudoSDKManager.ThreeDSecureEnabled, () => {
				JudoSDKManager.ThreeDSecureEnabled = !JudoSDKManager.ThreeDSecureEnabled;
			});
			SetUpToggle (RiskSwitch, JudoSDKManager.RiskSignals, () => {
				JudoSDKManager.RiskSignals = !JudoSDKManager.RiskSignals;
			});
			SetUpToggle (MaestroSwitch, JudoSDKManager.MaestroAccepted, () => {
				JudoSDKManager.MaestroAccepted = !JudoSDKManager.MaestroAccepted;
			});
			SetUpToggle (AmexSwitch, JudoSDKManager.AmExAccepted, () => {
				JudoSDKManager.AmExAccepted = !JudoSDKManager.AmExAccepted;
			});
			SetUpToggle (NoneUISwitch, !JudoSDKManager.UIMode, () => {
				JudoSDKManager.UIMode = !JudoSDKManager.UIMode;
				if (JudoSDKManager.UIMode)
					return;

				UIAlertView nonUIWarning = new UIAlertView ("Non-UI Mode",
					                              "You are about to use non UI Mode so please look at the source code to understand the usage of Non-UI APIs.",
					                              null, "OK", null);
				nonUIWarning.Show ();
			});
				
			AttachTouchEvents ();
			ArrowIcon.Transform = CGAffineTransform.MakeRotation ((nfloat)(180.0f * Math.PI) / 180.0f);

		}

		public override void WillRemoveSubview (UIView uiview)
		{
			base.WillRemoveSubview (uiview);
			AVSSwitch.ValueChanged -= delegate {
			};
			ThreeDSwitch.ValueChanged -= delegate {
			};
			RiskSwitch.ValueChanged -= delegate {
			};
			MaestroSwitch.ValueChanged -= delegate {
			};
			AmexSwitch.ValueChanged -= delegate {
			};
			NoneUISwitch.ValueChanged -= delegate {
			};

		}



		void SetUpToggle (UISwitch switchRef, bool CurrentValue, Action flipAction)
		{
			switchRef.Transform = CGAffineTransform.MakeScale (0.75F, 0.75F);
			switchRef.On = CurrentValue;
			switchRef.ValueChanged += delegate {
				flipAction ();
			};    
		}

		public override void AwakeFromNib ()
		{
			var arr = NSBundle.MainBundle.LoadNib ("SlideUpMenu", this, null);
			var v = Runtime.GetNSObject (arr.ValueAt (0)) as UIView;
			v.Frame = new RectangleF (0, 0, Frame.Width, Frame.Height);
			AddSubview (v);
		}

		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);

		}

		void SlideAndFix (UIPanGestureRecognizer gesture)
		{

			UIView piece = gesture.View;
			nfloat yComponent = piece.Superview.Center.Y - 40f;
			if (!ComponentExpanded || piece.Frame.Top < piece.Superview.Center.Y) {
				//yComponent = piece.Superview.Center.Y;
				ComponentExpanded = true;

			} else {
				yComponent = piece.Superview.Frame.Height - 40f;
				ComponentExpanded = false;
			}

			UIImageView.Animate (
				duration: 0.25f, 
				delay: 0,
				options: UIViewAnimationOptions.TransitionNone,
				animation: () => {
					piece.Frame = new RectangleF (new PointF (piece.Frame.X, yComponent), piece.Frame.Size);

					gesture.SetTranslation (new PointF (0, 0), piece.Superview);
				},
				completion: () => {
					
				});			
		}

		public void ResetMenu ()
		{
			UIView piece = panGesture.View;
			var yComponent = piece.Superview.Frame.Height - 40f;
			ComponentExpanded = false;

			piece.Frame = new RectangleF (new PointF (piece.Frame.X, yComponent), piece.Frame.Size);

			panGesture.SetTranslation (new PointF (0, 0), piece.Superview);

			this.Draw (this.Frame);
		}

		void PanGestureMoveAround (UIPanGestureRecognizer gesture)
		{
			
			UIView piece = gesture.View;

			AdjustAnchorPointForGestureRecognizer (gesture);

			if (gesture.State == UIGestureRecognizerState.Began || gesture.State == UIGestureRecognizerState.Changed) {

				PointF translation = gesture.TranslationInView (piece.Superview);

				piece.Center = new PointF (piece.Center.X, piece.Center.Y + translation.Y);
				 
				gesture.SetTranslation (new PointF (0, 0), piece.Superview);

			} else if (gesture.State == UIGestureRecognizerState.Ended) {
				SlideAndFix (gesture);
			} 		
		}

		void AdjustAnchorPointForGestureRecognizer (UIGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began) {
				UIView piece = gestureRecognizer.View;
				PointF locationInView = gestureRecognizer.LocationInView (piece);
				PointF locationInSuperview = gestureRecognizer.LocationInView (piece.Superview);

				piece.Layer.AnchorPoint = new PointF (locationInView.X / piece.Bounds.Size.Width, locationInView.Y / piece.Bounds.Size.Height);			
				piece.Center = locationInSuperview;
			}
		}

		private void AttachTouchEvents ()
		{

			panGesture = new UIPanGestureRecognizer ();

			panGesture.AddTarget (() => { 				
				PanGestureMoveAround (panGesture);
			});
			panGesture.MaximumNumberOfTouches = 2;
			this.AddGestureRecognizer (panGesture);
		}


		public void DrawBevel (RectangleF rect)
		{
			using (CGContext context = UIGraphics.GetCurrentContext ()) {

				context.SetLineWidth (1);
				UIColor.Black.SetFill ();
				UIColor.Black.SetStroke ();
				var currentPath = new CGPath ();
	
				currentPath.AddLines (new PointF[] {
					new PointF (10, 20),
					new PointF (16, 10), 
					new PointF (22, 20)
				});
				currentPath.CloseSubpath ();
				context.AddPath (currentPath);    
				context.DrawPath (CGPathDrawingMode.FillStroke);   
			}
		}



	}
}

