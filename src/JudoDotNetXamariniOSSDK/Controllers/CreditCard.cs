﻿using System;
using System.Text.RegularExpressions;
using System.Linq;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public class CreditCard
	{
		// See: http://www.regular-expressions.info/creditcard.html
		private const string VISA = @"^4[0-9]{15}?";
		// VISA 16
		private const string MC = @"^5[1-5][0-9]{14}$";
		// MC 16
		private const string AMEX_REG = @"^3[47][0-9]{13}$";
		// AMEX 15
		private const string DISCOVER = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
		// Discover 16
		private const string DINERS_CLUB = @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$";
		// DinersClub 14
		private const string MAESTRO = @"[0-9]{16}?";

		private const string AMEX_REG_TYPE	=	@"^3[47][0-9]{2}$";
		// AMEX 15
		private const string DINERS_CLUB_TYPE =	@"^3(?:0[0-5]|[68][0-9])[0-9]$";
		// DinersClub 14 // 38812345678901
		private const string VISA_TYPE = @"^4[0-9]{3}?";
		// VISA 16
		private const string MC_TYPE = @"^5[1-5][0-9]{2}$";
		// MC 16
		private const string DISCOVER_TYPE = @"^6(?:011|5[0-9]{2})$";
		// Discover 16
		private const string MAESTRO_TYPE = @"^(5018|5020|5038|6304|6759|6761|6763|6334|6767|4903|4905|4911|4936|564182|633110|6333|6759|5600|5602|5603|5610|5611|5656|6700|6706|6773|6775|6709|6771|6773|6775)";

		static Regex visaReg;
		static Regex mcReg;
		static Regex amexReg;

		static Regex maestroReg;

		static Regex visaTypeReg;
		static Regex mcTypeReg;
		static Regex amexTypeReg;

		static Regex maestroTypeReg;

	
	
		public CreditCard ()
		{
			visaReg = new Regex (VISA,	RegexOptions.None);
			mcReg	= new Regex (MC,	RegexOptions.None);
			maestroReg = new Regex (MAESTRO,	RegexOptions.None);      
			amexReg	= new Regex (AMEX_REG,	RegexOptions.None); 		


			visaTypeReg	= new Regex (VISA_TYPE,	RegexOptions.None); 	
			mcTypeReg = new Regex (MC_TYPE,	RegexOptions.None); 
			maestroTypeReg = new Regex (MAESTRO_TYPE,	RegexOptions.None); 

			amexTypeReg = new Regex (AMEX_REG_TYPE,	RegexOptions.None);

		}

		public int LengthOfFormattedStringTilLastGroupForType (CreditCardType type)
		{
			int idx = 0;

			switch (type) {
			case CreditCardType.Visa:
				
			case CreditCardType.MasterCard:
				
			case CreditCardType.Discover:
						// { 4-4-4-4}
			case CreditCardType.Maestro:
				idx = 16 + 3 - 4;
				break;
			case CreditCardType.AMEX:			// {4-6-5}
				idx = 15 + 2 - 5;
				break;
			case CreditCardType.DinersClub:	// {4-6-4}
				idx = 14 + 2 - 4;
				break;
			default:
				idx = 0;
				break;
			}
			return idx;
		}

		// http://www.regular-expressions.info/creditcard.html
		public CreditCardType GetCCType (string proposedNumber)
		{
			Regex reg = new Regex ("");

			if (proposedNumber.Length < Card.CC_LEN_FOR_TYPE)
				return CreditCardType.InvalidCard;

			for (int idx = 0; idx < (int)CreditCardType.InvalidCard; ++idx) {
				switch (idx) {
				case  (int)CreditCardType.Visa:
					reg = visaTypeReg;
					break;
				case  (int)CreditCardType.MasterCard:
					reg = mcTypeReg;
					break;
				case  (int)CreditCardType.AMEX:
					reg = amexTypeReg;
					break;
				case (int)CreditCardType.Maestro:
					reg = maestroTypeReg;
					break;
				}


				CSRange range = new CSRange (0, Card.CC_LEN_FOR_TYPE);


				var matches = reg.Matches (proposedNumber.Substring (range.Location, range.Length));
				if (matches != null) {
					if (matches.Count == 1) {
						return (CreditCardType)idx;
					}
				}

			}
			return CreditCardType.InvalidCard;
		}

		string CleanNumber (string str)
		{
			return str.Replace (@" ", @"");
		}

		public string FormatForViewing (string enteredNumber)
		{
			string cleaned = CleanNumber (enteredNumber);
			int len = cleaned.Length;

			if (len <= Card.CC_LEN_FOR_TYPE)
				return cleaned;

			CSRange r2 = new CSRange ();
			r2.Location = 0;
			CSRange r3 = new CSRange ();
			r3.Location = 0;
			CSRange r4 = new CSRange ();
			r4.Location = 0;
			string[] gaps = new string[]{ @"", @"", @"" };

			int[] segmentLengths = new int[3] { 0, 0, 0 };

			switch (GetCCType (enteredNumber)) {
			case CreditCardType.Visa:
			case CreditCardType.MasterCard:
			case CreditCardType.Discover:		// { 4-4-4-4}
			case CreditCardType.Maestro:
				segmentLengths [0] = 4;
				segmentLengths [1] = 4;
				segmentLengths [2] = 4;
				break;
			case CreditCardType.AMEX:			// {4-6-5}
				segmentLengths [0] = 6;
				segmentLengths [1] = 5;
				break;
			case CreditCardType.DinersClub:	// {4-6-4}
				segmentLengths [0] = 6;
				segmentLengths [1] = 4;
				break;
			default:
				return enteredNumber;
			}

			len -= Card.CC_LEN_FOR_TYPE;
			CSRange[] r = new CSRange[3]{ r2, r3, r4 };
			int totalLen = Card.CC_LEN_FOR_TYPE;
			for (int idx = 0; idx < 3; ++idx) {
				int segLen = segmentLengths [idx];
				if (segLen == null)
					break;

				r [idx].Location = totalLen;
				r [idx].Length = len >= segLen ? segLen : len;
				totalLen += segLen;
				len -= segLen;
				gaps [idx] = @" ";


				if (len <= 0)
					break;
			}
			
			string segment1 = enteredNumber.Substring (0, Card.CC_LEN_FOR_TYPE);
			string segment2 = r2.Location == 0 ? @"" : enteredNumber.Substring (r2.Location, r2.Length);
			string segment3 = r3.Location == 0 ? @"" : enteredNumber.Substring (r3.Location, r3.Length);
			;
			string segment4 = r4.Location == 0 ? @"" : enteredNumber.Substring (r4.Location, r4.Length);
			;

			string ret = string.Format (@"{0}{1}{2}{3}{4}{5}{6}", 
				             segment1, gaps [0],
				             segment2, gaps [1],
				             segment3, gaps [2],
				             segment4);

			return ret;
		}


		public int LengthOfStringForType (CreditCardType type)
		{
			int idx = 0;

			switch (type) {
			case CreditCardType.Visa:
			case CreditCardType.MasterCard:
			case CreditCardType.Discover:		// { 4-4-4-4}
			case CreditCardType.Maestro:
				idx = 16;
				break;
			case CreditCardType.AMEX:			// {4-6-5}
				idx = 15;
				break;
			case CreditCardType.DinersClub:	// {4-6-4}
				idx = 14;
				break;
			default:
				idx = 0;
				break;
			}
			return idx;
		}

		public int LengthOfFormattedStringForType (CreditCardType type)
		{
			int idx = 0;

			switch (type) {
			case CreditCardType.Visa:
			case CreditCardType.MasterCard:
			case CreditCardType.Discover:		// { 4-4-4-4}
			case CreditCardType.Maestro:
				idx = 16 + 3;
				break;
			case CreditCardType.AMEX:			// {4-6-5}
				idx = 15 + 2;
				break;
			case CreditCardType.DinersClub:	// {4-6-4}
				idx = 14 + 2;
				break;
			default:
				idx = 0;
				break;
			}
			return idx;
		}

		public bool IsLuhnValid (string number)
		{

			var cardArray = number.Select (c => c - '0').ToArray ();

			return (cardArray.Select ((d, i) => i % 2 == cardArray.Length % 2 ? ((2 * d) % 10) + d / 5 : d).Sum () % 10) == 0;
		}

		//////////////////////////////////////////////////////////////////////
		// http://www.regular-expressions.info/creditcard.html
		public bool isValidNumber (string number)
		{
			Regex reg = null;
			bool ret = false;

			switch (GetCCType (number)) {
			case CreditCardType.Visa:
				reg = visaReg;
				break;
			case CreditCardType.MasterCard:
				reg = mcReg;
				break;
			case CreditCardType.AMEX:
				reg = amexReg;
				break;
			case CreditCardType.Maestro:
				reg = maestroReg;
				break;

			default:
				break;
			}
			if (reg != null) {
				int matches = reg.Matches (number).Count;
				ret = matches == 1 ? true : false;

			}

			return ret;
		}

		public bool IsStartDateValid (string proposedDate)
		{
			if (proposedDate.Length != 5) {
				return false;
			}

			Regex regex = new Regex (@"([0-9]{2})/([0-9]{2})", RegexOptions.None);
			MatchCollection results = regex.Matches (proposedDate.Substring (0, proposedDate.Length));
			if (results.Count == 0) {
				return false;
			}

			var result = results [results.Count - 1];
			if (result.Groups.Count != 3) {
				return false;

			}

			string monthString = proposedDate.Substring (result.Groups [1].Index, result.Groups [1].Length);
			string yearString = proposedDate.Substring (result.Groups [2].Index, result.Groups [2].Length);

			int proposedMonth = Int32.Parse (monthString);
			int proposedYear = Int32.Parse (yearString) + 2000;



			DateTime today = DateTime.Now;
			int currentMonth = today.Month;
			int currentYear = today.Year;

			if (currentYear < proposedYear) {
				return false;
			}
			if (currentYear == proposedYear) {
				if (currentMonth < proposedMonth) {
					return false;
				}
			}
			// no more than 10 years in the past
			if (currentYear - 10 > proposedYear || (currentYear - 10 == proposedYear && currentMonth > proposedMonth)) {
				return false;
			}

			return true;

		}


		public string ccvFormat (CreditCardType type)
		{
			return type == CreditCardType.AMEX ? @"%04.4u" : @"%03.3u";
		}

		public string promptStringForType (CreditCardType type, bool justNumber)
		{
			string number = "0000 0000 0000 0000";
			string additions = @"";

			switch (type) {
			case CreditCardType.Visa:
			case CreditCardType.MasterCard:
			case CreditCardType.Discover:		// { 4-4-4-4}
			case CreditCardType.Maestro:
				
				number = @"0000 0000 0000 0000";
				additions = @" MM/YY CV2";
				break;
			case CreditCardType.AMEX:			// {4-6-5}
				number = @"0000 000000 00000";
				additions = @" MM/YY CIDV";
				break;
			case CreditCardType.DinersClub:	// {4-6-4}
				number = @"XXXX XXXXXX XXXX";
				additions = @" MM/YY CV2";
				break;
			default:
				break;
			}
			return justNumber ? number : number + additions;
		}


		public UIImage CreditCardImage (CreditCardType type)
		{
			string name;

			switch (type) {
			case CreditCardType.Visa:
				name = @"ic_card_large_visa";
				break;
			case CreditCardType.MasterCard:
				name = @"ic_card_large_mastercard";
				break;
			case CreditCardType.Maestro:
				name = @"ic_card_large_maestro";
				break;
			case CreditCardType.AMEX:
				name = @"ic_card_large_amex";
				break;
			case CreditCardType.Discover:
				name = @"ic_card_large_unknown";
				break;
			case CreditCardType.DinersClub:
				name = @"ic_card_large_unknown";
				break;
			default:
				name = @"ic_card_large_unknown";
				break;
			}
			return ThemeBundleReplacement.BundledOrReplacementImage (name, BundledOrReplacementOptions.BundledOrReplacement);
		}


		public UIImage CreditCardBackImage (CreditCardType type)
		{
			string backName;

			switch (type) {
			case CreditCardType.AMEX:
				backName = @"ic_card_large_cv2_amex";
				break;
			default:
				backName = @"ic_card_large_cv2";
				break;
			}	
			return ThemeBundleReplacement.BundledOrReplacementImage (backName, BundledOrReplacementOptions.BundledOrReplacement);
		}





	}
}

