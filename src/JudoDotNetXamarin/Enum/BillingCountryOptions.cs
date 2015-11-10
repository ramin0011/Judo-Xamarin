using System;
using System.ComponentModel;
using JudoPayDotNet.Enums;

namespace JudoDotNetXamarin
{
	public enum BillingCountryOptions
	{
		[Description("UK")]
		BillingCountryOptionUK,
		[Description("USA")]
		BillingCountryOptionUSA,
		[Description("Can")]
		BillingCountryOptionCanada,
		[Description("Other")]
		BillingCountryOptionOther
	}
}

