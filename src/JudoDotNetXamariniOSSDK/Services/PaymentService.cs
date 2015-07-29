using System;

using JudoPayDotNet;
using JudoPayDotNet.Models;
using System.Threading.Tasks;

namespace JudoDotNetXamariniOSSDK
{
	public class PaymentService : IPaymentService
	{
		private	JudoPayApi _judoAPI;

		public PaymentService (JudoPayApi judoAPI)
		{
			_judoAPI = judoAPI;
		}

		public async Task MakePayment (PaymentViewModel paymentViewModel)
		{
			CardPaymentModel payment = new CardPaymentModel {
				JudoId = AppConfig.JudoID,
				YourPaymentReference = AppConfig.PaymentReference,
				YourConsumerReference = AppConfig.ConsumerRef,
				Amount = decimal.Parse(paymentViewModel.Amount),
				CardNumber = paymentViewModel.Card.CardNumber,
				CV2 = paymentViewModel.Card.CV2.ToString(),
				ExpiryDate = paymentViewModel.Card.ExpireDate
			};
			try
			{
				
				Task<IResult<ITransactionResult>> task =  _judoAPI.Payments.Create(payment);
				var response = await task;
				if (!response.HasError)
				{
					Console.WriteLine(response.Response);    
				}
				else
				{
					Console.WriteLine(response.Error);
				}
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
			}





		}
	}
}
