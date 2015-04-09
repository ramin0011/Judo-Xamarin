using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
    class Receipt
    {
        public int receiptId { get; set; }
        public int originalReceiptId { get; set; }
        public string transactionType { get; set; }
        public string result { get; set; }
        public string message { get; set; }
        public string currency { get; set; }
        public string judoID { get; set; }
        public string marchantName { get; set; }
        public string appearsOnStatementsAs { get; set; }
        public string consumerRef { get; set; }
        public string paymentRef { get; set; }
        public DateTime createdAt { get; set; }
        public Dictionary metaData;
        public double originalAmount { get; set; }
        public double refunds { get; set; }
        public double netAmount { get; set; }
        public double amount { get; set; }
        public double partnerServiceFee { get; set; }
        public Card card { get; set; }

        Receipt(Dictionary<string, string> dict) 
        { 
            //need to check logic how this controcutor is going to be used
        }

        public string Description()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", receiptId, createdAt, merchantName, netAmount);
        }

        public string GetFormattedDate()
        {
            return createdAt.ToString("d MMMM · h:mm f");
        }

        UIImage receiptStatusImage()
        { 
            UIImage image = ThemeBundleReplacement.BundledOrReplacementImage("ic_successful", null);
            if([transactionType.ToLower() == "refund") // need to grab @refund string from resource
            {
                image = ThemeBundleReplacement.BundledOrReplacementImage("ic_refund", null);
            }
            return image;
        }
    }
}