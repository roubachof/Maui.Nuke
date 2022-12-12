using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Maui.Nuke;

public static class ImageSourcesMauiAppBuilderExtensions
{
    public static MauiAppBuilder UseNuke(this MauiAppBuilder builder, bool showDebugLogs = false)
	{
#if __IOS__
		NukeController.ShowDebugLogs = showDebugLogs;
		builder.ConfigureImageSources(services =>
		{
			services.AddService(
				svcs => new NukeFileImageSourceService(
					svcs.GetService<ILogger<FileImageSourceService>>()));

			services.AddService(
				svcs => new NukeUriImageSourceService(
					svcs.GetService<ILogger<UriImageSourceService>>()));

			services.AddService<FileImageSource>(
				svcs => new NukeFileImageSourceService(
					svcs.GetService<ILogger<FileImageSourceService>>()));

			services.AddService<UriImageSource>(
				svcs => new NukeUriImageSourceService(
					svcs.GetService<ILogger<UriImageSourceService>>()));
		});
#endif
		return builder;
	}
}
