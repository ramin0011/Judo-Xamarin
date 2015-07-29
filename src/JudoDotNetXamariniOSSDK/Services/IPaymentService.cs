using System;
using System.Threading.Tasks;

namespace JudoDotNetXamariniOSSDK
{
	public interface IPaymentService
	{
		Task MakePayment (PaymentViewModel payment);
	}
}

