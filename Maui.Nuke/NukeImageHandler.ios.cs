#if IOS || MACCATALYST
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Maui.Nuke;

/// <summary>
/// Custom ImageHandler that ensures InvalidateMeasure is called for all image sources,
/// including custom ImageSourceService implementations like Nuke.
/// This fixes the issue where images from custom services don't trigger layout updates.
/// </summary>
public class NukeImageHandler : ImageHandler
{
	private UIImage? _lastImage;

	public static new void MapSource(IImageHandler handler, Microsoft.Maui.IImage image)
	{
		_ = MapSourceAsync(handler, image);
	}

	public static new async Task MapSourceAsync(IImageHandler handler, Microsoft.Maui.IImage image)
	{
		await ImageHandler.MapSourceAsync(handler, image);

		// After the source has been loaded, ensure InvalidateMeasure is called
		if (handler is NukeImageHandler nukeHandler && nukeHandler.PlatformView is { } imageView)
		{
			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				if (imageView.Image != null && !ReferenceEquals(imageView.Image, nukeHandler._lastImage))
				{
					nukeHandler._lastImage = imageView.Image;
					// Call the extension method with both UIView and IView
					if (image.Source is not IStreamImageSource)
					{
						// If it's a StreamImageSource invalidate measure is already called by the maui image handler
						imageView.InvalidateMeasure(image);
					}
				}
			});
		}
	}
}
#endif
