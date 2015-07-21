using System.Collections.Generic;

namespace JudoDotNetXamariniOSSDK.Controllers
{
    class CardHelper : Card
    {
    //    Dictionary JUDO_dictionaryRepresentation
    //    {
    //        Dictionary<string, string> cardAddress = new Dictionary<string, string>{};
    
    //        if (cardNumber) cardAddress.Add("cardNumber", CardNumber);
    //        if (expiryDate) cardAddress.Add("expiryDate", ExpiryDate);
    //        if (CV2) cardAddress.Add("CV2", CV2);
    ////      if (self.cardType == MAESTRO) {
    //            if (startDate) cardAddress.Add("startDate", StartDate);
    //            if (issueNumber) cardAddress.Add("issueNumber", IssueNumber);
    ////      }
    //        if (postCode && countryCode) {
    //            cardAddress.Add("postCode", PostCode);
    //            cardAddress.Add("countryCode", CountryCode);
    //        }
    
    //        return cardAddress;
    //    }
        public CardHelper(string cardNumber, string expiryDate, string cv2, string startDate = null, string issueNumber = null) : base(cardNumber, expiryDate, cv2, startDate, issueNumber)
        {
        }

        public CardHelper(string cardToken, string expiryDate, string lastFour, string cardType, string startDate = null, string issueNumber = null) : base(cardToken, expiryDate, lastFour, cardType, startDate, issueNumber)
        {
        }

        public CardHelper(Dictionary<string, object> details) : base(details)
        {
        }
    }
}