using System;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public delegate void CreditCardSaved(Card card);

	public class CreditCardController : UIViewController, IUIScrollViewDelegate
	{
		public int CreditCardControllerType {get; set;}
		public Card judoCard {get; set;}
		public event Action<bool, Card> CompletionBlock;

		private UITableView TableView { get; set; }
		private UITableViewCell CardDetailsCell {get; set;}
		private UITableViewCell ReassuringTextCell { get; set;}
		private UITableViewCell AVSCell { get; set; }
		private UITableViewCell MaestroCell { get; set;}
		private UITableViewCell PayCell {get; set;}
		private UITableViewCell SpacerCell {get; set;}

		private UILabel PostCodeLabel { get; set;}
		private UIView PostCodeBackgroundView {get; set;}
		private UITextField PostCodeTextField {get; set;}
		private UIButton CountryButton {get; set;}
		private UILabel CountryLabel {get; set;}
		private UIButton HomeButton {get; set;}
		private UILabel CountryWarningLabel {get; set;}
		private UIView PostCodeContainerView {get; set;}

		private UITextField StartDateTextField {get; set;}
		private UILabel StartDateLabel { get; set;}
		private UIView StartDateContainerView {get; set;}
		private UILabel StartDatePlaceholder {get; set;}
		private UILabel StartDateWarningLabel {get; set;}
		private UITextField IssueNumberContainerTextView {get; set;}
		private UILabel IssueNumberLabel {get; set;}
		private UILabel IssueNumberContainerView {get; set;}

		private UIView PickerViewContainer {get; set;}
		private UIPickerView PickerView {get; set;}
		private UIButton PickerDoneCoverButton {get; set;}

		private UILabel TransactionInfoLabel {get; set;}
		private UIButton SubmitButton {get; set;}
		private UIButton CancelButton {get; set;}

		private BSKeyboardControls KeyboardControls {get; set;}
		private UIButton NumberFieldClearButton {get; set;}
		private UIButton ExpiryInfoButton {get; set;}

		private UILabel StatusHelpLabel { get; set;}
		private UILabel PleaseRecheckNumberLabel {get; set;}
		private NSMutableArray CellsToShow {get; set;}

		private NSLayoutConstraint PickBottomConstraint {get; set;}

		private UIImageView creaditCardImage;
		private UIView containerView;

		private UIScrollView textScroller;
		private UIView warningView;
		private PlaceHolderTextView placeView;
	}
}

