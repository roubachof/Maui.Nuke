using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

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
		
		// Register custom ImageHandler that fixes layout invalidation for custom ImageSourceServices
		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddHandler<Image, NukeImageHandler>();
		});
		
		// Override the MapSource to use our custom implementation
		NukeImageHandler.Mapper.ModifyMapping(nameof(Microsoft.Maui.IImage.Source), (handler, view, _) =>
		{
			NukeImageHandler.MapSource(handler, view);
		});
		
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
