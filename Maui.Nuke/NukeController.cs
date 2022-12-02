using Foundation;
using UIKit;

using Xamarin.Nuke;

namespace Maui.Nuke;

public static class NukeController
{
    public static Task<UIImage?> LoadImageAsync(NSUrl url, Action<string>? onFail)
    {
        var tcs = new TaskCompletionSource<UIImage?>();

        var pipeline = new ImagePipeline();

        // ImagePipeline.Shared.LoadImageWithUrl(

        pipeline.LoadImageWithUrl(
            url,
            (image, errorMessage) =>
                {
                    if (image == null)
                    {
                        onFail?.Invoke(errorMessage);
                    }

                    tcs.SetResult(image);
                });

        tcs.SetResult(null);

        return tcs.Task;
    }

    public static void ClearCache()
    {
        DataLoader.Shared.RemoveAllCachedResponses();
        ImageCache.Shared.RemoveAll();
    }
}