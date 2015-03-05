package judodotnetxamarinsdk.activies;


public class BaseActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onPostCreate:(Landroid/os/Bundle;)V:GetOnPostCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("JudoDotNetXamarinSDK.Activies.BaseActivity, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BaseActivity.class, __md_methods);
	}


	public BaseActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BaseActivity.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Activies.BaseActivity, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onPostCreate (android.os.Bundle p0)
	{
		n_onPostCreate (p0);
	}

	private native void n_onPostCreate (android.os.Bundle p0);

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
