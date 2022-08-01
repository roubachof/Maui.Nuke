using Foundation;
using UIKit;

namespace Maui.Nuke;

public static class NukeController
{
    public static Task<UIImage?> LoadImageAsync(NSUrl url, Action<string>? onFail)
    {
        var tcs = new TaskCompletionSource<UIImage?>();

        //ImagePipeline.Shared.LoadImageWithUrl(
        //    url,
        //    (image, errorMessage) =>
        //        {
        //            if (image == null)
        //            {
        //                onFail?.Invoke(errorMessage);
        //            }

        //            tcs.SetResult(image);
        //        });

        tcs.SetResult(null);

        return tcs.Task;
    }

    public static void ClearCache()
    {
        //DataLoader.Shared.RemoveAllCachedResponses();
        //ImageCache.Shared.RemoveAll();
    }
}