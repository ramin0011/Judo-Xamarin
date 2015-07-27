﻿using System;
using System.Text.RegularExpressions;

namespace JudoDotNetXamariniOSSDK
{
	public class CreditCard
	{
		// See: http://www.regular-expressions.info/creditcard.html
		private const string VISA = @"^4[0-9]{15}?";						// VISA 16
		private const string  MC= @"^5[1-5][0-9]{14}$";				// MC 16
		private const string AMEX_REG=			@"^3[47][0-9]{13}$";					// AMEX 15
		private const string DISCOVER=			@"^6(?:011|5[0-9]{2})[0-9]{12}$";	// Discover 16
		private const string DINERS_CLUB=			@"^3(?:0[0-5]|[68][0-9])[0-9]{11}$";	// DinersClub 14
		private const string MAESTRO =            @"[0-9]{16}?";

		private const string AMEX_REG_TYPE	=	@"^3[47][0-9]{2}$";					// AMEX 15
		private const string DINERS_CLUB_TYPE=	@"^3(?:0[0-5]|[68][0-9])[0-9]$";		// DinersClub 14 // 38812345678901
		private const string VISA_TYPE=			@"^4[0-9]{3}?";						// VISA 16
		private const string MC_TYPE=				@"^5[1-5][0-9]{2}$";					// MC 16
		private const string DISCOVER_TYPE=		@"^6(?:011|5[0-9]{2})$";				// Discover 16
		private const string MAESTRO_TYPE =       @"^(5018|5020|5038|6304|6759|6761|6763|6334|6767|4903|4905|4911|4936|564182|633110|6333|6759|5600|5602|5603|5610|5611|5656|6700|6706|6773|6775|6709|6771|6773|6775)";

		static Regex visaReg;
		static Regex mcReg;
		static Regex amexReg;
		static Regex discoverReg;
		static Regex dinersClubReg;
		static Regex maestroReg;

		static Regex visaTypeReg;
		static Regex mcTypeReg;
		static Regex amexTypeReg;
		static Regex discoverTypeReg;
		static Regex dinersClubTypeReg;
		static Regex maestroTypeReg;

		private int CC_LEN_FOR_TYPE =12; //TODO find the proper dict reference 
	
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

			/*
	        discoverReg			= [NSRegularExpression regularExpressionWithPattern:DISCOVER options:0 error:&error];
			dinersClubReg		= [NSRegularExpression regularExpressionWithPattern:DINERS_CLUB options:0 error:&error];
			*/

			/*
			discoverTypeReg		= [NSRegularExpression regularExpressionWithPattern:DISCOVER_TYPE options:0 error:&error];
			dinersClubTypeReg	= [NSRegularExpression regularExpressionWithPattern:DINERS_CLUB_TYPE options:0 error:&error];	
			}
			*/
		}

		public int LengthOfFormattedStringTilLastGroupForType (CreditCardType type)
		{
			int idx =0;

			switch(type) {
			case CreditCardType.Visa:
				break;
			case CreditCardType.MasterCard:
				break;
			case CreditCardType.Discover:
				break;		// { 4-4-4-4}
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
	public CreditCardType GetCCType(string proposedNumber)
	{
			Regex reg = new Regex (".*");

		if(proposedNumber.Length < CC_LEN_FOR_TYPE) return CreditCardType.InvalidCard;

		for(int idx = 0; idx < (int)CreditCardType.InvalidCard; ++idx) {
			switch(idx) {
			case  (int)CreditCardType.Visa:
				reg = visaTypeReg;
				break;
			case  (int)CreditCardType.MasterCard:
				reg = mcTypeReg;
				break;
			case  (int)CreditCardType.AMEX:
				reg = amexTypeReg;
				break;
			case  (int)CreditCardType.Discover:
				reg = discoverTypeReg;
				break;
			case  (int)CreditCardType.DinersClub:
				reg = dinersClubTypeReg;
				break;
			case (int)CreditCardType.Maestro:
				reg = maestroTypeReg;
				break;
			}
			CSRange range = new CSRange(0,CC_LEN_FOR_TYPE);

			int matches = reg.Matches(proposedNumber).Count;
			if(matches == 1) return (CreditCardType) idx;
		}
		return CreditCardType.InvalidCard;
	}

	string CleanNumber(string str)
	{
			return str.Replace (@" ", @"");
	}

	public string FormatForViewing(string enteredNumber)
	{
		string cleaned = CleanNumber(enteredNumber);
		int len = cleaned.Length;

		if(len <= CC_LEN_FOR_TYPE) return cleaned;

		CSRange r2 = new CSRange(); r2.Location = 0;
		CSRange r3 = new CSRange(); r3.Location = 0;
		CSRange r4 = new CSRange(); r4.Location = 0;
		string[] gaps= new string[]{@"", @"", @""};

		int[] segmentLengths = new int[3] { 0, 0, 0 };

		switch(GetCCType(enteredNumber)) {
		case CreditCardType.Visa:
		case CreditCardType.MasterCard:
		case CreditCardType.Discover:		// { 4-4-4-4}
		case CreditCardType.Maestro:
			segmentLengths[0] = 4;
			segmentLengths[1] = 4;
			segmentLengths[2] = 4;
			break;
		case CreditCardType.AMEX:			// {4-6-5}
			segmentLengths[0] = 6;
			segmentLengths[1] = 5;
			break;
		case CreditCardType.DinersClub:	// {4-6-4}
			segmentLengths[0] = 6;
			segmentLengths[1] = 4;
			break;
		default:
			return enteredNumber;
		}

		len -= CC_LEN_FOR_TYPE;
		CSRange[] r = new CSRange[3]{ r2, r3, r4 };
		int totalLen = CC_LEN_FOR_TYPE;
		for(int idx=0; idx<3; ++idx) {
			int segLen = segmentLengths[idx];
			if(segLen==null) break;

			r[idx].Location = totalLen;
			r[idx].Length = len >= segLen ? segLen : len;
			totalLen += segLen;
			len -= segLen;
			gaps[idx] = @" ";


			if(len <= 0) break;
		}
			
		string segment1 = enteredNumber.Substring(0,CC_LEN_FOR_TYPE);// [enteredNumber substringWithRange:NSMakeRange(0, CC_LEN_FOR_TYPE)];
		string segment2 =  r2.Location == null ? @"" :enteredNumber.Substring(r2.Location,r2.Length);// [enteredNumber substringWithRange:r2];
		string segment3 = r3.Location == null ? @"" : enteredNumber.Substring(r3.Location,r3.Length);;
		string segment4 = r4.Location == null ? @"" : enteredNumber.Substring(r4.Location,r4.Length);;

		string ret = string.Format (@"%@%@%@%@%@%@%@", 
			segment1, gaps[0],
			segment2, gaps[1],
			segment3, gaps[2],
			segment4);

		return ret;
	}

	public int LengthOfStringForType(CreditCardType type)
	{
		int idx=0;

		switch(type) {
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
	public int LengthOfFormattedStringForType(CreditCardType type)
	{
		int idx=0;

		switch(type) {
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


	public string ccvFormat(CreditCardType type)
	{
		return type == CreditCardType.AMEX ? @"%04.4u" : @"%03.3u";
	}

	public string promptStringForType(CreditCardType type, bool justNumber)
	{
		string number="";
		string additions="";

		switch(type) {
		case CreditCardType.Visa:
		case CreditCardType.MasterCard:
		case CreditCardType.Discover:		// { 4-4-4-4}
		case CreditCardType.Maestro:
			number = @"XXXX XXXX XXXX XXXX";
			number = @"0000 0000 0000 0000";
			additions = @" MM/YY CV2";
			break;
		case CreditCardType.AMEX:			// {4-6-5}
			number = @"XXXX XXXXXX XXXXX";
			additions = @" MM/YY CIDV";
			break;
		case CreditCardType.DinersClub:	// {4-6-4}
			number = @"XXXX XXXXXX XXXX";
			additions = @" MM/YY CV2";
			break;
		default:
			break;
		}
			return justNumber ? number : number + additions; // [number stringByAppendingString:additions];
	}




	}
}

