
public class ExceptionSerializable
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.io.Serializable
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_Write:(Ljava/io/ObjectOutputStream;)V:__export__\n" +
			"n_Read:(Ljava/io/ObjectInputStream;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("ExceptionSerializable, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ExceptionSerializable.class, __md_methods);
	}


	public ExceptionSerializable () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ExceptionSerializable.class)
			mono.android.TypeManager.Activate ("ExceptionSerializable, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void writeObject (java.io.ObjectOutputStream p0) throws java.io.IOException, java.lang.ClassNotFoundException
	{
		n_Write (p0);
	}

	private native void n_Write (java.io.ObjectOutputStream p0);


	public void readObject (java.io.ObjectInputStream p0) throws java.io.IOException, java.lang.ClassNotFoundException
	{
		n_Read (p0);
	}

	private native void n_Read (java.io.ObjectInputStream p0);

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
