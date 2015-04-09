using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Helpers
{
    class AddressHelper : Address
    {
        (NSDictionary JUDO_dictionaryRepresentation
        {
            Dictionary<string, string> cardAddress = new Dictionary<string, string>{};

            if(line1) cardAddress.Add("Line1", Line1);
            if(line2) cardAddress.Add("Line2", Line2);
            if(line3) cardAddress.Add("Line3", Line3);
            if(town) cardAddress.Add("Town", Town);
            if(postcode) cardAddress.Add("PostCode", PostCode);

            return cardAddress;
        }
    }
}