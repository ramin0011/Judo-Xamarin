﻿using System.Collections.Generic;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarin
{
    public class TokenPaymentViewModel :BasePaymentViewModel
    {
        /// <summary>
        /// Card Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Card Token
        /// </summary>
        public string CV2 { get; set; }

        /// <summary>
        /// your consumer token
        /// </summary>
        public string ConsumerToken { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Last Four digits of card 
        /// </summary>
        public string LastFour { get; set; }

        /// <summary>
        /// your consumer reference 
        /// </summary>
        public string ConsumerReference { get; set; }

        /// <summary>
        /// your meta data 
        /// </summary>
        public IDictionary<string, string> YourPaymentMetaData { get; set; }

        /// <summary>
        /// must pass the card token type to dispay card image 
        /// </summary>
        public CardType CardType { get; set; }

        public TokenPaymentViewModel Clone ()
        {
            return new TokenPaymentViewModel {
                Token = this.Token,
                CV2 = this.CV2,
                JudoID = this.JudoID,
                CardType = this.CardType,
                ConsumerToken = this.ConsumerToken,
                Amount = this.Amount,
                Currency = this.Currency,
                LastFour = this.LastFour,
                ConsumerReference = this.ConsumerReference,
                YourPaymentMetaData = this.YourPaymentMetaData,
            }; 
        }
    }
}

