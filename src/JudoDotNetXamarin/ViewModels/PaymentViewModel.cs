using System.Collections.Generic;

namespace JudoDotNetXamarin
{
    public class PaymentViewModel :BasePaymentViewModel
    {
        /// <summary>
        /// Card Detail
        /// </summary>
        public CardViewModel Card { get; set; }


        public PaymentViewModel Clone ()
        {
            return new PaymentViewModel {
                Card = this.Card.Clone (),
                Amount = this.Amount,
                Currency = this.Currency,
                ConsumerReference = this.ConsumerReference,
                YourPaymentMetaData = this.YourPaymentMetaData,
                JudoID = this.JudoID
                    
            }; 
        }
    }
}

