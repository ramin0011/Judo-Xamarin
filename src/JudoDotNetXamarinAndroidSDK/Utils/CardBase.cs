namespace JudoDotNetXamarinAndroidSDK.Utils
{
    public abstract class CardBase
    {

        public enum CardType
        {
            UNKNOWN = 0,
            VISA = 1,
            MASTERCARD = 2,
            VISA_ELECTRON = 3,
            SWITCH = 4,
            SOLO = 5,
            LASER = 6,
            CHINA_UNION_PAY = 7,
            AMEX = 8,
            JCB = 9,
            MASTRO = 10,
            VISA_DEBIT = 11
        };


        public abstract bool IsValidCard();
    }
}