#if IOS || MACCATALYST
using Foundation;
using ImageCaching.Nuke;
using UIKit;

namespace Maui.Nuke;

public static class NukeController
{
    public static bool ShowDebugLogs { get; set; }

    public static Task<UIImage?> LoadImageAsync(NSUrl url, Action<string>? onFail)
    {
        var tcs = new TaskCompletionSource<UIImage?>();

        ImagePipeline.Shared.LoadImageWithUrl(
            url,
            (image, errorMessage) =>
                {
                    if (image == null)
                    {
                        onFail?.Invoke(errorMessage);
                    }

                    tcs.SetResult(image);
                });

        return tcs.Task;
    }

    public static void ClearCache()
    {
        ImageCache.Shared.RemoveAll();
    }
}
#endif
