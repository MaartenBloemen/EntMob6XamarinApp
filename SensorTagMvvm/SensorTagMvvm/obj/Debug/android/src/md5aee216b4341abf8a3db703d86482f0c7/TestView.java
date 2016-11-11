package md5aee216b4341abf8a3db703d86482f0c7;


public class TestView
	extends mvvmcross.droid.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SensorTagMvvm.View.TestView, SensorTagMvvm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TestView.class, __md_methods);
	}


	public TestView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TestView.class)
			mono.android.TypeManager.Activate ("SensorTagMvvm.View.TestView, SensorTagMvvm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
