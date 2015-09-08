using System;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public interface IErrorPresenter
	{
		void DisplayError (IResult<ITransactionResult> result,string failHeader);
	}
}

