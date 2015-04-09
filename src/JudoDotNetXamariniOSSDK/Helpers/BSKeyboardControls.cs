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
        public UISegmentedControl SegmentedControl { get; set; }
        public IBSkeyboardControlsEvents VisibleControls { get; set; }
        private UIToolbar Toolbar { get; set; }
        private UIBarButtonItem DoneButton { get; set; }
        private UIBarButtonItem SegmentedControlItem { get; set; }

        public BSKeyboardControls(NSArray fields)
            : base(new CGRect(0.0f, 0.0f, 320.0f, 44.0f))
        {
            Toolbar = new UIToolbar(Frame);
            Toolbar.TintColor = UIColor.FromWhiteAlpha(0.961f, 1);
            AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
            AddSubview(Toolbar);

            SegmentedControl.AddTarget(this, new Selector(segmentedControlValueChanged), UIControlEvent.ValueChanged);
            SegmentedControl.Momentary = true;
            SegmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
            SegmentedControl.SetEnabled(false, (int)BSKeyboardControlsDirection.DirectionPrevious);
            SegmentedControl.SetEnabled(false, (int)BSKeyboardControlsDirection.DirectionNext);
            SegmentedControlItem = new UIBarButtonItem(SegmentedControl);

            UIImage buttonImage = new UIImage("BTN_done_normal");

            UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Bordered, this, new Selector(doneButtonPressed));
            doneButton.SetBackgroundImage(buttonImage, UIControlState.Normal, UIBarMetrics.Default);
            doneButton.AccessibilityLabel = "Done Button";

            var titleTextAttributes = new UITextAttributes();
            titleTextAttributes.TextColor = UIColor.FromWhiteAlpha(0.369f, 1);
            titleTextAttributes.TextShadowColor = UIColor.FromRGBA(0, 0, 0, 0.8);
            titleTextAttributes.TextShadowOffset = new UIOffset(0, 0);
            titleTextAttributes.Font = UIFont.FromName("Proxima Nova Regular", 10);


            doneButton.SetTitleTextAttributes(titleTextAttributes, UIControlState.Normal);
            this.DoneButton = doneButton;
            this.VisibleControls = BSKeyboardControl.PreviousNext | BSKeyboardControl.ControlDone;
            this.Fields = fields;
        }

        private void segmentedControlValueChanged(object sender)
        {

        }

        private void doneButtonPressed(object sender)
        {
        }

        private UIView _activeField;
        public UIView ActiveField
        {
            get
            { return _activeField; }
            set
            {
                UIView activeField = value
                if (activeField != this.ActiveField)
                {
                    if (Fields.conteainobject(activeField)) // conteainobject needs to be implement 
                    {
                        _activeField = activeField;
        
                        if (!ActiveField.IsFirstResponder)
                        {
                            activeField.BecomeFirstResponder();
                        }
        
                        this.SegmentedControl.Enabled = true;
                    }
                }
            }
        }

        private NSArray _fields;
        public NSArray Fields
        {
            get
            { return _fields; }
            set
            {
                NSArray fields = value;
                if (fields != this.Fields)
                {
                    foreach (UIView field in fields)
                    {
                        if (field.IsKindOfClass(UITextField))
                        {
                            ((UITextField)field).InputAccessoryView = this;
                        }
                        else if (field.IsKindOfClass(UITextView))
                        {
                            ((UITextView)field).InputAccessoryView = this;
                        }
                    }

                    this.Fields = fields;
                }
            }
        }

        private UIBarStyle _barstyle;
        public UIBarStyle BarStyle 
        { 
            get
            {
                return _barstyle;
            }
            set
            {
                Toolbar.BarStyle = value;
                _barstyle = value;
            }
        }

        private UIColor _bartintcolor;
        public UIColor BarTintColor 
        {
            get { return _bartintcolor; }
            set
            {
                Toolbar.TintColor = value;
                _bartintcolor = value;
            }
        }

        private UIColor _segmentctrlcolor;
        public UIColor SegmentedControlTintControl 
        {
            get { return _segmentctrlcolor; }
            set
            {
                SegmentedControl.TintColor = value;
                _segmentctrlcolor = value;
            }
        }

        private string _previoustitle;
        public string PreviousTitle
        {
            get { return _previoustitle; }
            set
            {
                SegmentedControl.SetTitle(value, BSKeyboardControlsDirection.DirectionPrevious);
                _previoustitle = value;
            }
        }

        private string _nexttitle;
        public string NextTitle
        {
            get { return _nexttitle; }
            set
            {
                SegmentedControl.SetTitle(value, BSKeyboardControlsDirection.DirectionNext);
                _nexttitle = value;
            }
        }

        public string DoneTitle
        {
            set { DoneButton.Title = value; }
        }

        public UIColor DoneTintColor
        {
            set { DoneButton.TintColor = value; }
        }

        private void segmentedControlValueChanged (object sender)
        {
            switch (SegmentedControl.SelectedSegment)
            {
                case BSKeyboardControlsDirection.DirectionPrevious:
                    selectPreviousField();
                    break;
                case BSKeyboardControlsDirection.DirectionNext:
                    selectNextField();
                    break;
                default:
                    break;
            }
        }

        private void doneButtonPressed (object sender)
        {
            // pass the event to done button
            //if ([self.delegate respondsToSelector:@selector(keyboardControlsDonePressed:)])
            //{
            //    [self.delegate keyboardControlsDonePressed:self];
            //}
        }

        private void updateSegmentedControlEnabledStates()
        {
            int index = Fields.getIndexOfObject(activeField); //getindexOfObject to be implemented
            if (index != -1)
            {
                SegmentedControl.SetEnabled((index > 0), BSKeyboardControlsDirection.DirectionPrevious);
                SegmentedControl.SetEnabled((index > Fields.Count - 1), BSKeyboardControlsDirection.DirectionNext);
            }
        }

        private void selectNextField()
        {
            int index = Fields.getIndexOfObject(ActiveField); //getindexOfObject to be implemented
            if (index < Fields.Count - 1)
            {
                index += 1;
                UIView field = Fields.ObjectAtIndex(index); // it is ActiveField
                setActiveField(field);
        
                // to be implement
                //if ([self.delegate respondsToSelector:@selector(keyboardControls:selectedField:inDirection:)])
                //{
                //    [self.delegate keyboardControls:self selectedField:field inDirection:BSKeyboardControlsDirectionNext];
                //}
            }

        }

        private NSArray toolbarItems()
        {
            NSMutableArray items = new NSMutableArray[3];

    
            if (VisibleControls & BSKeyboardControl.PreviousNext)
            {
                items.Add(SegmentedControlItem);
            }
    
            if (VisibleControls & BSKeyboardControl.ControlDone)
            {
                items.Add(UIBarButtonItem.Alloc(UIBarButtonSystemItem);
                items.Add(DoneButton);
            }
    
            return items;
        }


    }
}

