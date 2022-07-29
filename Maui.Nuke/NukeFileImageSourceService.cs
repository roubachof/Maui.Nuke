using Foundation;
using Microsoft.Extensions.Logging;
using UIKit;

namespace Maui.Nuke;

internal class NukeFileImageSourceService: ImageSourceService, IImageSourceService<IFileImageSource>
{
    private static readonly Dictionary<float, string> ScaleToDensitySuffix = new()
    {
            { 1, string.Empty }, 
            { 2, "@2x" }, 
            { 3, "@3x" },
            { 4, "@4x" },
            { 5, "@5x" },
            { 6, "@6x" },
        };

    private readonly LoggerWrapper _logger;

    public NukeFileImageSourceService(ILogger? logger = null) : base(logger)
    {
        _logger = new LoggerWrapper(logger);
    }

    public override async Task<IImageSourceServiceResult<UIImage>?> GetImageAsync(
        IImageSource imageSource, 
        float scale = 1, 
        CancellationToken cancellationToken = default)
    {
        IFileImageSource fileImageSource = (IFileImageSource)imageSource;

        var fileName = fileImageSource.File;
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _logger.Debug(() => "A null or empty filename has been specified for the FileImageSource, returning...");
            return null;
        }

        string name = Path.GetFileNameWithoutExtension(fileName);
        if (string.IsNullOrWhiteSpace(name))
        {
            _logger.Debug(() => $"An extension without a name ({fileName}) has been specified for the FileImageSource, returning...");
            return null;
        }

        string nameWithSuffix = $"{name}{ScaleToDensitySuffix[scale]}";
        string filenameWithDensity = fileName.Replace(name, nameWithSuffix);

        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        NSUrl fileUrl;
        if (File.Exists(filenameWithDensity))
        {
            _logger.Debug(() => $"Loading \"{filenameWithDensity}\" as a file URI");
            fileUrl = NSUrl.FromFilename(filenameWithDensity);;
        }
        else if (File.Exists(fileName))
        {
            _logger.Debug(() => $"Loading \"{fileName}\" as a file URI");
            fileUrl = NSUrl.FromFilename(fileName);;
        }
        else
        {
            _logger.Debug(() => $"Couldn't retrieve the image URI: loading \"{fileName}\" from Bundle");

            var imageFromBundle = UIImage.FromBundle(fileName);
            if (imageFromBundle == null)
            {
                return null;
            }

            return new ImageSourceServiceResult(imageFromBundle!, () => imageFromBundle.Dispose());
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        var image = await NukeController.LoadImageAsync(
            fileUrl, 
            (errorMessage) => _logger.Warn($"Fail to load image: {fileUrl.AbsoluteString}, innerError: {errorMessage}"));

        if (image == null)
        {
            return null;
        }

        return new ImageSourceServiceResult(image!, () => image.Dispose());
    }
}
