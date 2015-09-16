using System;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public class CardViewModel
	{
		public string CardNumber { get; set; }

		public string ExpireDate { get; set; }

		public string CV2 { get; set; }

		public CardType CardType { get; set; }

		public string PostCode { get; set; }

        public ISO3166CountryCodes CountryCode { get; set; }

		public string StartDate {get;set;}

		public string IssueNumber {get;set;}
	}
}

