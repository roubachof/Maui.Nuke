using Benchmark.Profiler;
using Maui.Nuke;

using Microsoft.Extensions.Logging;

namespace Benchmark;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureNuke(showDebugLogs: true);

		builder.Logging.AddDebug();

		// Uncomment for XF only test
        // MemoryProfiler.Instance = new MemoryProfiler("Maui", null);

        // Uncomment for Nuke test
        MemoryProfiler.Instance = new MemoryProfiler("Nuke", NukeController.ClearCache);

		return builder.Build();
	}
}
