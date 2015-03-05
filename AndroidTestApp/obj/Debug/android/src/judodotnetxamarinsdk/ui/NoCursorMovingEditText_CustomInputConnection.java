package judodotnetxamarinsdk.ui;


public class NoCursorMovingEditText_CustomInputConnection
	extends android.view.inputmethod.InputConnectionWrapper
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_sendKeyEvent:(Landroid/view/KeyEvent;)Z:GetSendKeyEvent_Landroid_view_KeyEvent_Handler\n" +
			"n_deleteSurroundingText:(II)Z:GetDeleteSurroundingText_IIHandler\n" +
			"";
		mono.android.Runtime.register ("JudoDotNetXamarinSDK.Ui.NoCursorMovingEditText/CustomInputConnection, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NoCursorMovingEditText_CustomInputConnection.class, __md_methods);
	}


	public NoCursorMovingEditText_CustomInputConnection (android.view.inputmethod.InputConnection p0, boolean p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == NoCursorMovingEditText_CustomInputConnection.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Ui.NoCursorMovingEditText/CustomInputConnection, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.InputMethods.IInputConnection, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Boolean, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
	}


	public boolean sendKeyEvent (android.view.KeyEvent p0)
	{
		return n_sendKeyEvent (p0);
	}

	private native boolean n_sendKeyEvent (android.view.KeyEvent p0);


	public boolean deleteSurroundingText (int p0, int p1)
	{
		return n_deleteSurroundingText (p0, p1);
	}

	private native boolean n_deleteSurroundingText (int p0, int p1);

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
