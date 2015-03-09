using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Android.OS;
using Java.Interop;
using Java.IO;
using Java.Lang;
using Exception = System.Exception;

namespace JudoDotNetXamarinSDK.Models
{
    public class ExceptionSerializable : Object, IParcelable
    {
        public Exception Exception { get; set; }

        public ExceptionSerializable(Parcel parcel)
        {
            var exceptionByteLength = parcel.ReadInt();
            byte[] exceptionBytes = new byte[exceptionByteLength];
            parcel.ReadByteArray(exceptionBytes);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(exceptionBytes))
            {
                Exception = binaryFormatter.Deserialize(stream) as Exception;
            }
        }

        [ExportField("CREATOR")]
        public static ExceptionCreator InitializeCreator()
        {
            return new ExceptionCreator();
        }

        public ExceptionSerializable(Exception exception)
        {
            Exception = exception;
        }


        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, Exception);

                var exceptionArray = stream.ToArray();
                var exceptionByteLength = exceptionArray.Length;

                dest.WriteInt(exceptionByteLength);
                dest.WriteByteArray(exceptionArray);
            }
        }

        public int DescribeContents()
        {
            return 0;
        }

    }

    public class ExceptionCreator : Java.Lang.Object, IParcelableCreator
    {
        public Object CreateFromParcel(Parcel source)
        {
            return new ExceptionSerializable(source);
        }

        public Object[] NewArray(int size)
        {
            return new ExceptionSerializable[size];
        }
    }
}