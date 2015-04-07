using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
    class Consumer
    {
        Consumer(string consumerReference) 
        {
            yourConsumerReference = consumerReference;
        }

        public string consumerToken { get; set; }
        public string yourConsumerReference { get; set; }
        public string mobileNumber { get; set; }
        public string emailAddress { get; set; }

    }
}