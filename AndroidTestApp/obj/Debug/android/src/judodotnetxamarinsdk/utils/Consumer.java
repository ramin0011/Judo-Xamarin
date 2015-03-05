package judodotnetxamarinsdk.utils;


public class Consumer
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.os.Parcelable
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_InitializeCreator:()Ljudodotnetxamarinsdk/utils/ConsumerCreator;:__export__\n" +
			"n_toString:()Ljava/lang/String;:GetToStringHandler\n" +
			"n_describeContents:()I:GetDescribeContentsHandler:Android.OS.IParcelableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_writeToParcel:(Landroid/os/Parcel;I)V:GetWriteToParcel_Landroid_os_Parcel_IHandler:Android.OS.IParcelableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("JudoDotNetXamarinSDK.Utils.Consumer, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Consumer.class, __md_methods);
	}


	public Consumer () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Consumer.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Utils.Consumer, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public Consumer (java.lang.String p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == Consumer.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Utils.Consumer, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}

	public Consumer (android.os.Parcel p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == Consumer.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Utils.Consumer, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Parcel, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public static judodotnetxamarinsdk.utils.ConsumerCreator CREATOR = InitializeCreator ();

	public static judodotnetxamarinsdk.utils.ConsumerCreator InitializeCreator ()
	{
		return n_InitializeCreator ();
	}

	private static native judodotnetxamarinsdk.utils.ConsumerCreator n_InitializeCreator ();


	public java.lang.String toString ()
	{
		return n_toString ();
	}

	private native java.lang.String n_toString ();


	public int describeContents ()
	{
		return n_describeContents ();
	}

	private native int n_describeContents ();


	public void writeToParcel (android.os.Parcel p0, int p1)
	{
		n_writeToParcel (p0, p1);
	}

	private native void n_writeToParcel (android.os.Parcel p0, int p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
