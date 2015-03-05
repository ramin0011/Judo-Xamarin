package judodotnetxamarinsdk.ui;


public class NoCursorMovingEditText
	extends android.widget.EditText
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onSelectionChanged:(II)V:GetOnSelectionChanged_IIHandler\n" +
			"n_onCreateInputConnection:(Landroid/view/inputmethod/EditorInfo;)Landroid/view/inputmethod/InputConnection;:GetOnCreateInputConnection_Landroid_view_inputmethod_EditorInfo_Handler\n" +
			"";
		mono.android.Runtime.register ("JudoDotNetXamarinSDK.Ui.NoCursorMovingEditText, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NoCursorMovingEditText.class, __md_methods);
	}


	public NoCursorMovingEditText (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == NoCursorMovingEditText.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Ui.NoCursorMovingEditText, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public NoCursorMovingEditText (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == NoCursorMovingEditText.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Ui.NoCursorMovingEditText, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public NoCursorMovingEditText (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == NoCursorMovingEditText.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Ui.NoCursorMovingEditText, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onSelectionChanged (int p0, int p1)
	{
		n_onSelectionChanged (p0, p1);
	}

	private native void n_onSelectionChanged (int p0, int p1);


	public android.view.inputmethod.InputConnection onCreateInputConnection (android.view.inputmethod.EditorInfo p0)
	{
		return n_onCreateInputConnection (p0);
	}

	private native android.view.inputmethod.InputConnection n_onCreateInputConnection (android.view.inputmethod.EditorInfo p0);

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
