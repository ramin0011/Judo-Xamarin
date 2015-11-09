﻿using System;
using System.ComponentModel;
using JudoPayDotNet;
using System.Reflection;

namespace JudoDotNetXamariniOSSDK
{
	internal static class EnumExtensions
	{
		public static string ToDescriptionString(this Enum val)
		{
			DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attributes.Length > 0 ? attributes[0].Description : string.Empty;
		}
	}
}

