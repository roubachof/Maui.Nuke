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
		
		// Override the MapSource to use our custom implementation with PlatformView null safety.
		// 
		// IMPORTANT: ViewHandler<T>.PlatformView throws InvalidOperationException when the
		// native view is disconnected (e.g., during fast scrolling, navigation, tab switching).
		// We guard against this at the mapper level as a defense-in-depth measure.
		//
		// See: https://github.com/dotnet/maui/issues/17165
		//      https://github.com/dotnet/maui/issues/17569
		//      https://github.com/roubachof/Maui.Nuke/issues/11
		NukeImageHandler.Mapper.ModifyMapping(nameof(Microsoft.Maui.IImage.Source), (handler, view, _) =>
		{
			// Safe check: use base IElementHandler.PlatformView which returns null
			// instead of ViewHandler<T>.PlatformView which throws
			if (((IElementHandler)handler).PlatformView is null)
			{
				System.Diagnostics.Debug.WriteLine(
					"[Maui.Nuke] Skipping MapSource - PlatformView is null (handler disconnected)");
				return;
			}

			try
			{
				NukeImageHandler.MapSource(handler, view);
			}
			catch (InvalidOperationException ex) when (ex.Message.Contains("PlatformView cannot be null"))
			{
				// Race condition: PlatformView became null between our check and the actual access.
				// This is expected during fast scrolling in CollectionViews, navigation transitions,
				// tab switching in Shell, and CarPlay/background transitions.
				System.Diagnostics.Debug.WriteLine(
					$"[Maui.Nuke] PlatformView disconnected during MapSource (race condition, expected): {ex.Message}");
			}
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
