using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Controllers
{
    class CardHelper : Card
    {
        Dictionary JUDO_dictionaryRepresentation
        {
            Dictionary<string, string> cardAddress = new Dictionary<string, string>{};
    
            if (cardNumber) cardAddress.Add("cardNumber", CardNumber);
            if (expiryDate) cardAddress.Add("expiryDate", ExpiryDate);
            if (CV2) cardAddress.Add("CV2", CV2);
    //      if (self.cardType == MAESTRO) {
                if (startDate) cardAddress.Add("startDate", StartDate);
                if (issueNumber) cardAddress.Add("issueNumber", IssueNumber);
    //      }
            if (postCode && countryCode) {
                cardAddress.Add("postCode", PostCode);
                cardAddress.Add("countryCode", CountryCode);
            }
    
            return cardAddress;
        }
    }
}