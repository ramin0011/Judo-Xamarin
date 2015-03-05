package judodotnetxamarinsdk.ui;


public class BackgroundHintTextView_Filter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.text.InputFilter
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_filter:(Ljava/lang/CharSequence;IILandroid/text/Spanned;II)Ljava/lang/CharSequence;:GetFilter_Ljava_lang_CharSequence_IILandroid_text_Spanned_IIHandler:Android.Text.IInputFilterInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("JudoDotNetXamarinSDK.Ui.BackgroundHintTextView/Filter, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BackgroundHintTextView_Filter.class, __md_methods);
	}


	public BackgroundHintTextView_Filter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BackgroundHintTextView_Filter.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Ui.BackgroundHintTextView/Filter, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public BackgroundHintTextView_Filter (judodotnetxamarinsdk.ui.BackgroundHintTextView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == BackgroundHintTextView_Filter.class)
			mono.android.TypeManager.Activate ("JudoDotNetXamarinSDK.Ui.BackgroundHintTextView/Filter, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "JudoDotNetXamarinSDK.Ui.BackgroundHintTextView, JudoDotNetXamarinSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public java.lang.CharSequence filter (java.lang.CharSequence p0, int p1, int p2, android.text.Spanned p3, int p4, int p5)
	{
		return n_filter (p0, p1, p2, p3, p4, p5);
	}

	private native java.lang.CharSequence n_filter (java.lang.CharSequence p0, int p1, int p2, android.text.Spanned p3, int p4, int p5);

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
