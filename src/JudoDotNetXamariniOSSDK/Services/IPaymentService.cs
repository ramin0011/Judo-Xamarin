using System;
using System.Threading.Tasks;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public interface IPaymentService
	{
		Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel payment);
	}
}

