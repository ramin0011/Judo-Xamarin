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
				IResult<ITransactionResult> response = await task;
				if (!response.HasError)
				{
					PaymentReceiptModel paymentreceipt = response.Response as PaymentReceiptModel;
					PaymentReceiptViewModel receipt = new PaymentReceiptViewModel()
					{
						CreatedAt = paymentreceipt.CreatedAt.DateTime,
						Currency = paymentreceipt.Currency,
						OriginalAmount = paymentreceipt.Amount,
						ReceiptId = paymentreceipt.ReceiptId
					};
					JudoSDKManager.ShowReceipt(receipt);
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
