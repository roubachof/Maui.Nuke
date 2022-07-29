using Benchmark.Profiler;
using Benchmark.Views;
using Maui.Nuke;

namespace Benchmark;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage (new MainPage ());
	}
}
