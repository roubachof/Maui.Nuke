using Microsoft.Extensions.Logging;

namespace Maui.Nuke;

public static class ImageSourcesMauiAppBuilderExtensions
{
    public static MauiAppBuilder UseNuke(
	    this MauiAppBuilder builder,
	    bool cacheOnlyRemoteImages = false,
	    bool showDebugLogs = false)
	{
#if IOS || MACCATALYST
		NukeController.ShowDebugLogs = showDebugLogs;
		builder.ConfigureImageSources(services =>
		{
			if (!cacheOnlyRemoteImages)
			{
				services.AddService(svcs =>
					new NukeFileImageSourceService(svcs.GetService<ILogger<FileImageSourceService>>()));
				services.AddService<FileImageSource>(svcs =>
					new NukeFileImageSourceService(svcs.GetService<ILogger<FileImageSourceService>>()));
			}

			services.AddService(svcs =>
				new NukeUriImageSourceService(svcs.GetService<ILogger<UriImageSourceService>>()));
			services.AddService<UriImageSource>(svcs =>
				new NukeUriImageSourceService(svcs.GetService<ILogger<UriImageSourceService>>()));
		});
#endif
		return builder;
	}
}
