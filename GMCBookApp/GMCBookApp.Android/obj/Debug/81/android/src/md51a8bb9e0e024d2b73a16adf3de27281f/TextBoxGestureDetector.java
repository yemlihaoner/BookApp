package md51a8bb9e0e024d2b73a16adf3de27281f;


public class TextBoxGestureDetector
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSingleTapConfirmed:(Landroid/view/MotionEvent;)Z:GetOnSingleTapConfirmed_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfPdfViewer.XForms.Droid.TextBoxGestureDetector, Syncfusion.SfPdfViewer.XForms.Android", TextBoxGestureDetector.class, __md_methods);
	}


	public TextBoxGestureDetector ()
	{
		super ();
		if (getClass () == TextBoxGestureDetector.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfPdfViewer.XForms.Droid.TextBoxGestureDetector, Syncfusion.SfPdfViewer.XForms.Android", "", this, new java.lang.Object[] {  });
	}

	public TextBoxGestureDetector (md51a8bb9e0e024d2b73a16adf3de27281f.TextBoxEx p0)
	{
		super ();
		if (getClass () == TextBoxGestureDetector.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfPdfViewer.XForms.Droid.TextBoxGestureDetector, Syncfusion.SfPdfViewer.XForms.Android", "Syncfusion.SfPdfViewer.XForms.Droid.TextBoxEx, Syncfusion.SfPdfViewer.XForms.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean onSingleTapConfirmed (android.view.MotionEvent p0)
	{
		return n_onSingleTapConfirmed (p0);
	}

	private native boolean n_onSingleTapConfirmed (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
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
