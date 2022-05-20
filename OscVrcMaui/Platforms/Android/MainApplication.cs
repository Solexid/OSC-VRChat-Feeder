

using Android;
using Android.App;
using Android.Runtime;
using AndroidX.Core.App;
using Shiny;

namespace OscVrcMaui;

[Application]

public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{	
	}
    public override void OnCreate()
    {
		this.ShinyOnCreate(new OscVrcMaui.MauiShinyStartup());

		DependencyService.Register<ForegroundWorker>();
		base.OnCreate();
    }

    protected override MauiApp CreateMauiApp() {


	
		return MauiProgram.CreateMauiApp(); 
	
	
	
	}
}
