package md5aee216b4341abf8a3db703d86482f0c7;


public class NoInternetView
	extends mvvmcross.droid.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SensorTagMvvm.View.NoInternetView, SensorTagMvvm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NoInternetView.class, __md_methods);
	}


	public NoInternetView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NoInternetView.class)
			mono.android.TypeManager.Activate ("SensorTagMvvm.View.NoInternetView, SensorTagMvvm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
