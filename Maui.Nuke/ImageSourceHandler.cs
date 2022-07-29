﻿using Foundation;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;

using Xamarin.Forms.Nuke;

[assembly: ExportImageSourceHandler(
    typeof(FileImageSource), typeof(ImageSourceHandler))]
[assembly: ExportImageSourceHandler(
    typeof(UriImageSource), typeof(ImageSourceHandler))]

namespace Xamarin.Forms.Nuke
{
    [Preserve(AllMembers = true)]
    public class ImageSourceHandler : IImageSourceHandler, IAnimationSourceHandler
    {
        internal static readonly FileImageSourceHandler DefaultFileImageSourceHandler = new FileImageSourceHandler();

        private static readonly ImageLoaderSourceHandler DefaultUriImageSourceHandler = new ImageLoaderSourceHandler();

        public Task<UIImage> LoadImageAsync(
            ImageSource imageSource,
            CancellationToken cancellationToken = new CancellationToken(),
            float scale = 1) =>
            NukeHelper.LoadViaNuke(imageSource, cancellationToken, scale);

        public Task<FormsCAKeyFrameAnimation> LoadImageAnimationAsync(
            ImageSource imageSource,
            CancellationToken cancellationToken = new CancellationToken(),
            float scale = 1)
        {
            FormsHandler.Debug(() => $"Delegating animation of {imageSource} to default Xamarin.Forms handler");

            if (imageSource is UriImageSource)
            {
                return DefaultUriImageSourceHandler.LoadImageAnimationAsync(imageSource, cancellationToken, scale);
            }

            return DefaultFileImageSourceHandler.LoadImageAnimationAsync(imageSource, cancellationToken, scale);
        }
    }
}
