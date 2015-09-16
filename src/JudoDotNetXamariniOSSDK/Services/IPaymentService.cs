using System;
using System.Threading.Tasks;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public interface IPaymentService
	{
		Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel payment);
		Task<IResult<ITransactionResult>> PreAuthoriseCard (PaymentViewModel payment);
		Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel payment);
		Task<IResult<ITransactionResult>>  MakeTokenPreAuthorisation (TokenPaymentViewModel payment);
	    Task<IResult<ITransactionResult>> RegisterCard(PaymentViewModel payment);
	}
}

