using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Object = Java.Lang.Object;

namespace JudoDotNetXamarinSDK.Utils
{
    public class Consumer : Object, IParcelable
    {
        public string ConsumerToken { get; set; }
        public string YourConsumerReference { get; set; }

        [ExportField("CREATOR")]
        public static ConsumerCreator InitializeCreator()
        {
            return new ConsumerCreator();
        }

        public Consumer() { }

        public Consumer(string consumerReference)
        {
            YourConsumerReference = consumerReference;
        }

        public Consumer(Parcel inParcel)
        {
            ConsumerToken = inParcel.ReadString();
            YourConsumerReference = inParcel.ReadString();
        }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteString(ConsumerToken);
            dest.WriteString(YourConsumerReference);
        }

        public override string ToString()
        {
            return String.Format(@"Consumer{{consumerToken='{0}', yourConsumerReference='{1}'}}", ConsumerToken,
                YourConsumerReference);
        }
    }

    public class ConsumerCreator : Java.Lang.Object, IParcelableCreator
    {
        public Object CreateFromParcel(Parcel source)
        {
            return new Consumer(source);
        }

        public Object[] NewArray(int size)
        {
            return new Consumer[size];
        }
    }
}