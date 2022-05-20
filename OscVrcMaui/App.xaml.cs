using OscVrcMaui.Services;

namespace OscVrcMaui;

public partial class App : Application
{ 
	public App()
	{
		InitializeComponent();
		DependencyService.Register<ConfigService>();
		DependencyService.Register<OSCService>();
		DependencyService.Register<MiBandService>();
		DependencyService.Register<ProfileDataStore>();
		DependencyService.Register<DeviceSensorsService>();
		DependencyService.Register<BLEService>();
		DependencyService.Register<OSCMainService>();
		MainPage = new AppShell();
	}
}
