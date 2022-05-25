
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using OscVrcMaui.Services;
using Shiny;

namespace OscVrcMaui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public partial class MainActivity : MauiAppCompatActivity
{
	
	SleepStatusReceiver sleeprecv;
	protected override void OnCreate(Bundle savedInstanceState)
    {
		ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.BluetoothConnect, Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothScan }, 0);
	
		sleeprecv = new SleepStatusReceiver();
		RegisterReceiver(sleeprecv, new IntentFilter("com.mc.miband.fellAsleep"));
		RegisterReceiver(sleeprecv, new IntentFilter("com.mc.miband.wokeUp"));
		
		base.OnCreate(savedInstanceState);	
    }
    protected override void OnNewIntent(Intent intent)
	{
		base.OnNewIntent(intent);
		this.ShinyOnNewIntent(intent);
	}

	protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
	{
		base.OnActivityResult(requestCode, resultCode, data);
		this.ShinyOnActivityResult(requestCode, resultCode, data);
	}

	public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
	{
		base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		this.ShinyOnRequestPermissionsResult(requestCode, permissions, grantResults);
		//global::Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
	}
}

[BroadcastReceiver(Enabled = true)]
[IntentFilter(new[] { "com.mc.miband.tasker.fellAsleep", "com.mc.miband.tasker.wokeUp", "com.mc.miband1.tasker.fellAsleep", "com.mc.miband1.tasker.wokeUp" })]

class SleepStatusReceiver : BroadcastReceiver
{
	public MiBandService band => DependencyService.Get<MiBandService>();
	public override void OnReceive(Context context, Intent intent)
	{
		// Do stuff here.

		if (intent.Action.Contains("fellAsleep"))
			band.SetSleepStatus(true);
		else
			band.SetSleepStatus(false);
	}
}