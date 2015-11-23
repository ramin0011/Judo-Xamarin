using Android.OS;
using Java.Interop;
using Java.Lang;
using JudoPayDotNet.Models;
using Newtonsoft.Json;
using Type = System.Type;

namespace JudoDotNetXamarinAndroidSDK.Models
{
    public class SReceipt : Object, IParcelable, ITransactionResult
    {
        private ITransactionResult receipt;

        public SReceipt (Parcel parcelable)
        {
            Type receipType = JsonConvert.DeserializeObject<Type> (parcelable.ReadString ());

            var receiptString = parcelable.ReadString ();
            receipt = JsonConvert.DeserializeObject (receiptString, receipType) as ITransactionResult;
        }

        public SReceipt (ITransactionResult receipt)
        {
            this.receipt = receipt;
        }

        [ExportField ("CREATOR")]
        public static ReceiptCreator InitializeCreator ()
        {
            return new ReceiptCreator ();
        }


        public void WriteToParcel (Parcel dest, ParcelableWriteFlags flags)
        {
            var receiptType = receipt.GetType ();
            dest.WriteString (JsonConvert.SerializeObject (receiptType));
            dest.WriteString (JsonConvert.SerializeObject (receipt));
        }

        public string ReceiptId {
            get { return receipt.ReceiptId; }
            set { receipt.ReceiptId = value; }
        }

        public string Result {
            get { return receipt.Result; }
            set { receipt.Result = value; }
        }

        public string Message {
            get { return receipt.Message; }
            set { receipt.Message = value; }
        }

        public ITransactionResult FullReceipt {
            get { return receipt; }
        }

        public int DescribeContents ()
        {
            return 0;
        }

    }

    public class ReceiptCreator : Java.Lang.Object, IParcelableCreator
    {
        public Object CreateFromParcel (Parcel source)
        {
            return new SReceipt (source);
        }

        public Object[] NewArray (int size)
        {
            return new SReceipt[size];
        }
    }
}