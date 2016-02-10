using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace JudoDotNetXamarin
{
    public class BasePaymentViewModel
    {
        public JObject ClientDetails { get; set; }

        /// <summary>
        /// your JudoID, can be used to ovverride the value set within JudoConfiguration on a transactional bases
        /// JudoConfiguration MUST still be set as failover
        /// </summary>
        public string  JudoID { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// your consumer reference 
        /// </summary>
        public string ConsumerReference { get; set; }

        /// <summary>
        /// your meta data 
        /// </summary>
        public 
        IDictionary<string, string> YourPaymentMetaData { get; set; }
    }
}

