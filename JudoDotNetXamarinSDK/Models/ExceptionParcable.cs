using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Android.OS;
using Java.Interop;
using Java.IO;
using Java.Lang;
using Exception = System.Exception;

public class ExceptionSerializable : Object, ISerializable
{
    public Exception Exception { get; set; }

    public ExceptionSerializable()
    {
        
    }

    public ExceptionSerializable(Exception exception)
    {
        Exception = exception;
    }


    [Export("writeObject", Throws = new[] {
        typeof (Java.IO.IOException),
        typeof (Java.Lang.ClassNotFoundException)})]
    public void Write(ObjectOutputStream destination)
    {

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            binaryFormatter.Serialize(stream, Exception);

            var exceptionArray = stream.ToArray();
            var exceptionByteLength = exceptionArray.Length;

            destination.WriteInt(exceptionByteLength);
            destination.Write(exceptionArray);
        }
    }


    [Export("readObject", Throws = new[] {
        typeof (Java.IO.IOException),
        typeof (Java.Lang.ClassNotFoundException)})]
    public void Read(ObjectInputStream source)
    {
        var exceptionByteLength = source.ReadInt();
        byte[] exceptionBytes = new byte[exceptionByteLength];
        source.Read(exceptionBytes);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(exceptionBytes))
        {
            Exception = binaryFormatter.Deserialize(stream) as Exception;
        }
    }

}
