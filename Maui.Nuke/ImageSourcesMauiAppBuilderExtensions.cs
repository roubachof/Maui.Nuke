﻿using Microsoft.Extensions.Logging;

namespace Maui.Nuke;

public static class ImageSourcesMauiAppBuilderExtensions
{
    public static MauiAppBuilder ConfigureNuke(this MauiAppBuilder builder)
	{
#if __IOS__
		builder.ConfigureImageSources(services =>
		{
			services.AddService(svcs => new NukeFileImageSourceService(svcs.GetService<ILogger<FileImageSourceService>>()));
			services.AddService(svcs => new NukeUriImageSourceService(svcs.GetService<ILogger<UriImageSourceService>>()));
		});
#endif
		return builder;
	}
}