using System;
using System.ComponentModel;
using JudoPayDotNet.Enums;

namespace JudoDotNetXamariniOSSDK
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

