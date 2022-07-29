using Foundation;
using Microsoft.Extensions.Logging;
using UIKit;

namespace Maui.Nuke;

public class NukeUriImageSourceService : ImageSourceService, IImageSourceService<IUriImageSource>
{
    private readonly LoggerWrapper _logger;

    public NukeUriImageSourceService(ILogger? logger = null) : base(logger)
    {
        _logger = new LoggerWrapper(logger);
    }

    public override async Task<IImageSourceServiceResult<UIImage>?> GetImageAsync(
        IImageSource imageSource, 
        float scale = 1, 
        CancellationToken cancellationToken = default)
    {
        IUriImageSource uriImageSource = (IUriImageSource)imageSource;

        var urlString = uriImageSource.Uri.AbsoluteUri;
        if (string.IsNullOrWhiteSpace(urlString))
        {
            _logger.Debug(() => "A null or empty url has been specified for the UriImageSource, returning...");
            return null;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        var nsUrl = NSUrl.FromString(urlString);
        if (nsUrl == null)
        {
            _logger.Debug(() => $"NSUrl returned a null url from: {urlString}");
            return null;
        }

        _logger.Debug(() => $"Loading \"{urlString}\" as a web URL");
        var image = await NukeController.LoadImageAsync(
            nsUrl, 
            (errorMessage) => _logger.Warn($"Fail to load image: {nsUrl.AbsoluteString}, innerError: {errorMessage}"));

        if (image == null)
        {
            return null;
        }

        return new ImageSourceServiceResult(image!, () => image.Dispose());
    }
}
