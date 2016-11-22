package mono;

import java.io.*;
import java.lang.String;
import java.util.Locale;
import java.util.HashSet;
import java.util.zip.*;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.util.Log;
import mono.android.Runtime;

public class MonoPackageManager {

	static Object lock = new Object ();
	static boolean initialized;

	static android.content.Context Context;

	public static void LoadApplication (Context context, ApplicationInfo runtimePackage, String[] apks)
	{
		synchronized (lock) {
			if (context instanceof android.app.Application) {
				Context = context;
			}
			if (!initialized) {
				android.content.IntentFilter timezoneChangedFilter  = new android.content.IntentFilter (
						android.content.Intent.ACTION_TIMEZONE_CHANGED
				);
				context.registerReceiver (new mono.android.app.NotifyTimeZoneChanges (), timezoneChangedFilter);
				
				System.loadLibrary("monodroid");
				Locale locale       = Locale.getDefault ();
				String language     = locale.getLanguage () + "-" + locale.getCountry ();
				String filesDir     = context.getFilesDir ().getAbsolutePath ();
				String cacheDir     = context.getCacheDir ().getAbsolutePath ();
				String dataDir      = getNativeLibraryPath (context);
				ClassLoader loader  = context.getClassLoader ();

				Runtime.init (
						language,
						apks,
						getNativeLibraryPath (runtimePackage),
						new String[]{
							filesDir,
							cacheDir,
							dataDir,
						},
						loader,
						new java.io.File (
							android.os.Environment.getExternalStorageDirectory (),
							"Android/data/" + context.getPackageName () + "/files/.__override__").getAbsolutePath (),
						MonoPackageManager_Resources.Assemblies,
						context.getPackageName ());
				
				mono.android.app.ApplicationRegistration.registerApplications ();
				
				initialized = true;
			}
		}
	}

	public static void setContext (Context context)
	{
		// Ignore; vestigial
	}

	static String getNativeLibraryPath (Context context)
	{
	    return getNativeLibraryPath (context.getApplicationInfo ());
	}

	static String getNativeLibraryPath (ApplicationInfo ainfo)
	{
		if (android.os.Build.VERSION.SDK_INT >= 9)
			return ainfo.nativeLibraryDir;
		return ainfo.dataDir + "/lib";
	}

	public static String[] getAssemblies ()
	{
		return MonoPackageManager_Resources.Assemblies;
	}

	public static String[] getDependencies ()
	{
		return MonoPackageManager_Resources.Dependencies;
	}

	public static String getApiPackageName ()
	{
		return MonoPackageManager_Resources.ApiPackageName;
	}
}

class MonoPackageManager_Resources {
	public static final String[] Assemblies = new String[]{
		/* We need to ensure that "SensorTagMvvm.dll" comes first in this list. */
		"SensorTagMvvm.dll",
		"DataProcessor.dll",
		"Java.Interop.dll",
		"Microsoft.Data.Sqlite.dll",
		"Microsoft.EntityFrameworkCore.dll",
		"Microsoft.EntityFrameworkCore.Relational.dll",
		"Microsoft.EntityFrameworkCore.Sqlite.dll",
		"Microsoft.Extensions.Caching.Abstractions.dll",
		"Microsoft.Extensions.Caching.Memory.dll",
		"Microsoft.Extensions.DependencyInjection.Abstractions.dll",
		"Microsoft.Extensions.DependencyInjection.dll",
		"Microsoft.Extensions.Logging.Abstractions.dll",
		"Microsoft.Extensions.Logging.dll",
		"Microsoft.Extensions.Options.dll",
		"Microsoft.Extensions.Primitives.dll",
		"MvvmCross.Binding.dll",
		"MvvmCross.Binding.Droid.dll",
		"MvvmCross.Core.dll",
		"MvvmCross.Droid.dll",
		"MvvmCross.Droid.Shared.dll",
		"MvvmCross.Localization.dll",
		"MvvmCross.Platform.dll",
		"MvvmCross.Platform.Droid.dll",
		"MvvmCross.Plugins.BLE.dll",
		"MvvmCross.Plugins.BLE.Droid.dll",
		"MvvmCross.Plugins.Json.dll",
		"Newtonsoft.Json.dll",
		"Plugin.BLE.Abstractions.dll",
		"Plugin.BLE.dll",
		"Plugin.Connectivity.Abstractions.dll",
		"Plugin.Connectivity.dll",
		"Plugin.CurrentActivity.dll",
		"Plugin.Permissions.Abstractions.dll",
		"Plugin.Permissions.dll",
		"Remotion.Linq.dll",
		"System.Collections.Immutable.dll",
		"System.Diagnostics.DiagnosticSource.dll",
		"System.Interactive.Async.dll",
		"System.Runtime.CompilerServices.Unsafe.dll",
		"Xamarin.Android.Support.v4.dll",
		"SensorTagMvvm.DAL.dll",
		"SensorTagMvvm.Domain.dll",
		"System.Threading.dll",
		"System.Runtime.dll",
		"System.Collections.dll",
		"System.Collections.Concurrent.dll",
		"System.Diagnostics.Debug.dll",
		"System.Reflection.dll",
		"System.Linq.dll",
		"System.Runtime.InteropServices.dll",
		"System.Runtime.Extensions.dll",
		"System.Reflection.Extensions.dll",
		"System.ServiceModel.Internals.dll",
		"System.Resources.ResourceManager.dll",
		"System.Data.Common.dll",
		"System.Threading.Tasks.dll",
		"System.Runtime.Handles.dll",
		"System.AppContext.dll",
		"System.Globalization.dll",
		"System.Text.Encoding.dll",
		"System.ComponentModel.dll",
		"System.Linq.Expressions.dll",
		"System.ObjectModel.dll",
		"System.ComponentModel.Annotations.dll",
		"System.IO.dll",
		"System.Linq.Queryable.dll",
		"System.Dynamic.Runtime.dll",
		"System.Text.RegularExpressions.dll",
		"System.IO.FileSystem.dll",
		"System.Xml.XDocument.dll",
		"System.Runtime.Serialization.Primitives.dll",
		"System.Xml.ReaderWriter.dll",
		"System.Text.Encoding.Extensions.dll",
		"System.Diagnostics.Tracing.dll",
	};
	public static final String[] Dependencies = new String[]{
	};
	public static final String ApiPackageName = "Mono.Android.Platform.ApiLevel_23";
}
