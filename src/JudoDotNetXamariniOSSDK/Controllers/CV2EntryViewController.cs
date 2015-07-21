using System;
using System.Drawing;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using UIKit;
using Foundation;

namespace JudoDotNetXamariniOSSDK.Controllers
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("CV2EntryViewController")]
    public class CV2EntryViewController : UIViewController
    {

        //public Transaction transaction;
        public NSDictionary cardInfo;
        public Card judoCard;

        public void completionBlock(bool success, Card card);

        public UILabel lastFourLabel { get; set; }
        public UILabel transactionInfoLabel { get; set; }
        public UIButton submitButton { get; set; }
        public UIButton cancelButton { get; set; }

        public BSKeyboardControls keyboardControls;
        public UIButton numberFieldClearButton { get; set; }
        public UIButton expiryInfoButton { get; set; }

        // CreditCard Info
        CreditCardType type;		// brand
        int numberLength;	    // length of formatted number only
        int ccv;				// three or 4 digits
        // we may need to change ccv datatype as string

        // States
        bool completelyDone;

        public CV2EntryViewController()
        {
            base.Init();
        }

        public virtual void Init() 
        {
            // need to update parameters and may be move this to constructor
            NSBundle bundle = NSBundle.GetUrlForResource ("JudoPay", "bundle", "", ""); 
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
            UIImage patternImage = ThemeBundleReplacement.BundledOrReplacementImage("bg_light_iPhone5", BundledOrReplacementOptions.BundledOrReplacement);
            this.View.BackgroundColor = UIColor.FromPatternImage(patternImage);
    
	        NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
            defaultCenter.AddObserver(this.Select());
            //[defaultCenter addObserver:self selector:@selector(keyboardMoving:) name:UIKeyboardWillShowNotification object:nil];	// dummyTV
            //[defaultCenter addObserver:self selector:@selector(keyboardMoving:) name:UIKeyboardDidShowNotification object:nil];		// dummyTV
            //[defaultCenter addObserver:self selector:@selector(keyboardMoving:) name:UIKeyboardWillHideNotification object:nil];	// passwordTextField
    
            UIEdgeInsets insets = UIEdgeInsetsMake(0.0, 20.0, 0.0, 20.0);
            UIImage activeImage = ThemeBundleReplacement.BundledOrReplacementImage("btn_pay_normal_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
            UIImage inactiveImage = ThemeBundleReplacement.BundledOrReplacementImage("btn_pay_inactive_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
            UIImage resizableActiveImage = activeImage.CreateResizableImage(insets);
            UIImage resizableInactiveImage = inactiveImage.CreateResizableImage(insets);
    
            this.submitButton.SetBackgroundImage(resizableActiveImage, UIControlState.Normal);
            this.submitButton.SetBackgroundImage(resizableInactiveImage, UIControlState.Disabled);

            this.submitButton.SetTitleColor(ThemeBundleReplacement.BundledOrReplacementColor("GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement), UIControlState.Normal);
            this.cancelButton.SetTitleColor(ThemeBundleReplacement.BundledOrReplacementColor("GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement), UIControlState.Normal);

	        this.editCard;

	        // something to do with this - HG
            //{
            //    CGRect r = ccText.frame;
            //    r.size.height = 36;	// 32 in NIB for Courier
            //    ccText.frame = r;
            //}
    
            creditCardImage.Tag = InvalidCard;
    
            creditCardImage.Layer.CornerRadius = 4.0f;
            creditCardImage.Layer.MasksToBounds = true;
    
            UIImage image = ThemeBundleReplacement.BundledOrReplacementImage("card_unknown", BundledOrReplacementOptions.BundledOrReplacement);
    
            creditCardImage.image = image;
    
	        CALayer layer = containerView.layer;
	        layer.cornerRadius = 4.0;
	        layer.masksToBounds = true;
	        layer.borderColor = ThemeBundleReplacement.BundledOrReplacementColor("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement);
	        layer.borderWidth = 1;
    
	        layer = textScroller.layer;
	        layer.cornerRadius = 4.0;
	        layer.masksToBounds = YES;
	        layer.borderWidth = 0;
    
            placeholderTextView.text = "CV2";
            ccText.text = "";
    
            ccText.font = FIXED_WIDTH_FONT_SIZE_20;
            self.lastFourLabel.font = FIXED_WIDTH_FONT_SIZE_20;
            placeholderTextView.font = FIXED_WIDTH_FONT_SIZE_20;
    
    
            ccText.becomeFirstResponder();
    
    
            NSArray fields = [ccText, placeholderTextView];
            BSKeyboardControls bd = new BSKeyboardControls (fields);
            bd.SegmentedControl.Hidden = true;
            this.SetKeyboardControls(bd);
            this.KeyboardControls(this);
    
            UITapGestureRecognizer tap = UITapGestureRecognizer.Alloc (this, selector(dismissKeyboardAction));
    
            this.View.AddGestureRecognizer(tap);
        }

        public bool PrefersStatusBarHidden()
        {
            return this.NavigationController == null;
        }

        void override ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.judoCard = new Card(init);
            this.judoCard.SetDetails(this.cardInfo);
    
            type= 1;
            this.updateCCimage;
    
            this.lastFourLabel.text = String.Format("{0:#####}", this.judoCard.lastFour);
        }

        void override DismissKeyboardAction(object sender)
        {
            ccText.resignFirstResponder();
        }

        void override ViewDidUnload
        {
	        // BEWARE: may not be complete or correct!
	        placeholderTextView = null;
	        creditCardImage = null;
	        containerView = null;
	        textScroller = null;
	        warningView = null;
	        ccText = null;
	        updateCard = null;
    
            NSNotificationCenter.DefaultCenter = this;
        }

        void override ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
    
            // I had to add this odd line to resign right before becoming first responder to make make the
            // cursor appear and flash in the textview, RP
            ccText.resignFirstResponder();
            ccText.becomeFirstResponder();
        }

        void override textViewDidChange(UITextView textView)
        {
            int maxLength = type == CreditCardType.AMEX ? 4 : 3;
    
            int lengthAfterChange = (int)textView.text.length;
    
            if (lengthAfterChange == maxLength)
            {
                this.KeyboardControlsDonePressed(null);
            }
        }

        bool textView(UITextView textView, NSRange range, NSString text)
        {
	        int maxLength = type == CreditCardType.AMEX ? 4 : 3;
    
            ulong lengthAfterChange = textView.text.length + text.length - range.length;
    
            if (lengthAfterChange <= maxLength) {
                const ushort spaces[] = {' ',' ',' ',' '};
                //placeholderTextView.text = ["CV2 " stringByReplacingCharactersInRange:NSMakeRange(0, lengthAfterChange) withString:[NSString stringWithCharacters:&spaces[0] length:lengthAfterChange]];
                placeholderTextView.text ="";

                completelyDone = lengthAfterChange == maxLength;
                this.submitButton.Enabled = completelyDone;
        
                return true;
            } else {
                this.KeyboardControlsDonePressed(null);
                return false;
            }
        }

        void updateCCimage()
        {
            creditCardImage.image = [CreditCard creditCardImage:self.judoCard.cardType];
        }

        void updateUI()
        {
            this.submitButton.Enabled = completelyDone;
        }

        IBAction SaveAction(object sender)
        {
            ccText.ResignFirstResponder();
        
            if (this.completionBlock) {
        
                this.judoCard.cv2 = ccText.text;
                this.completionBlock(true, this.judoCard);
            } else {
                this.DismissViewController(true, null);
            }
        }

        IBAction CancelAction(object sender)
        {
            if (this.CompletionBlock) {
                this.CompletionBlock(false, null);
            } else {
                this.DismissViewController(true, null);
            }
        }

        void editCard
        {
	        dispatch_async(dispatch_get_main_queue(), { [self updateUI]; });
        }

        void KeyboardControlsDonePressed(BSKeyboardControls keyboardControls)
        {
            this.updateUI();
            this.DismissKeyboardAction(null);
        }
    }
}