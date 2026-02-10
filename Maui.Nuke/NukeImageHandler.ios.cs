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

	/// <summary>
	/// Safely checks if the handler's native platform view is still connected.
	/// 
	/// IMPORTANT: ViewHandler&lt;T&gt;.PlatformView throws InvalidOperationException
	/// when the native view is null (disconnected handler). The base IElementHandler.PlatformView
	/// property returns null safely. We must use this safe path to avoid crashes during
	/// async operations that complete after view disconnection.
	/// 
	/// See: https://github.com/dotnet/maui/issues/17165
	///      https://github.com/dotnet/maui/issues/17569
	///      https://github.com/dotnet/maui/issues/27194
	/// </summary>
	private static bool HasPlatformView(IElementHandler handler)
	{
		try
		{
			return ((IElementHandler)handler).PlatformView is not null;
		}
		catch (ObjectDisposedException)
		{
			return false;
		}
	}

	public static new void MapSource(IImageHandler handler, Microsoft.Maui.IImage image)
	{
		_ = MapSourceAsync(handler, image);
	}

	public static new async Task MapSourceAsync(IImageHandler handler, Microsoft.Maui.IImage image)
	{
		// Guard: ensure PlatformView is still connected before starting async work
		if (!HasPlatformView(handler))
			return;

		try
		{
			await ImageHandler.MapSourceAsync(handler, image);
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("PlatformView cannot be null"))
		{
			// Race condition: handler was disconnected during async image load.
			// This is expected during fast scrolling, navigation, or tab switching.
			System.Diagnostics.Debug.WriteLine(
				$"[Maui.Nuke] PlatformView disconnected during image source mapping (expected during recycling): {ex.Message}");
			return;
		}
		catch (ObjectDisposedException)
		{
			// Native view was disposed during async operation
			System.Diagnostics.Debug.WriteLine(
				"[Maui.Nuke] PlatformView disposed during image source mapping");
			return;
		}

		// After the source has been loaded, ensure InvalidateMeasure is called.
		// Re-check PlatformView since async gap above could have allowed disconnection.
		if (handler is NukeImageHandler nukeHandler && HasPlatformView(handler))
		{
			try
			{
				var imageView = nukeHandler.PlatformView;
				await MainThread.InvokeOnMainThreadAsync(() =>
				{
					if (imageView.Image != null && !ReferenceEquals(imageView.Image, nukeHandler._lastImage))
					{
						nukeHandler._lastImage = imageView.Image;
						// If it's a StreamImageSource, InvalidateMeasure is already called by the MAUI image handler
						if (image.Source is not IStreamImageSource)
						{
							imageView.InvalidateMeasure(image);
						}
					}
				});
			}
			catch (InvalidOperationException ex) when (ex.Message.Contains("PlatformView cannot be null"))
			{
				// Handler disconnected between our check and access - this is fine
				System.Diagnostics.Debug.WriteLine(
					$"[Maui.Nuke] PlatformView disconnected during measure invalidation: {ex.Message}");
			}
			catch (ObjectDisposedException)
			{
				System.Diagnostics.Debug.WriteLine(
					"[Maui.Nuke] PlatformView disposed during measure invalidation");
			}
		}
	}
}
#endif
