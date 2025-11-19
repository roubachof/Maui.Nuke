# Maui.Nuke

Since MAUI, the `Android` platform get a native caching library: `Glide`.
Unfortunately on, `iOS`, there is no native caching...

`Maui.Nuke` is here to repair this injustice by implementing image caching with the fastest and most popular ios native caching library: [Nuke](https://github.com/kean/Nuke/).

Moreover, once installed, it is completly transparent to the user, you use your `Image` views just like before, all the work is done under the hood.

This project is using the [NukeProxy](https://github.com/roubachof/NukeProxy) library, which is a Swift proxy to the nuke native library. The new binding and the packaging has been done by the great @cheesebaron. Hail to the Cheese!

Current version of the Nuke library is **12.8**.

## Installation

### Basic Setup

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseSharpnadoNuke(loggerEnable: false);
    
    return builder.Build();
}
```

### Advanced Configuration

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseSharpnadoNuke(
            loggerEnable: true,                    // Enable logging
            cacheOnlyRemoteImages: true);          // Cache only HTTP/HTTPS images (v12.8.1+)
    
    return builder.Build();
}
```

### Configuration Options

- **`loggerEnable`** (default: `false`): Enable debug logging for troubleshooting
- **`cacheOnlyRemoteImages`** (default: `false`, since v12.8.1): When `true`, only caches remote images (HTTP/HTTPS), excludes local `file://` URIs


### BOOM

You just achieved **90%+** memory reduction when manipulating ```Image``` views on iOS and MacCatalyst.

## Platform Support

Since version 12.8.1, `Maui.Nuke` compiles for all MAUI platforms for easier integration:

| Platform | Nuke Caching | Behavior |
|----------|--------------|----------|
| **iOS 15.0+** | ✅ Active | Full Nuke 12.8 native caching |
| **MacCatalyst 15.0+** | ✅ Active | Full Nuke 12.8 native caching |
| **Android API 21+** | ❌ No-op | Uses MAUI's built-in Glide (already excellent) |
| **Windows 10.0.17763.0+** | ❌ No-op | Uses MAUI's built-in caching |

**Why multi-platform compilation?**
- Single package for all platforms (no conditional `PackageReference`)
- Simplified project setup
- Future-ready for potential enhancements

## Use Cases for `cacheOnlyRemoteImages`

The `cacheOnlyRemoteImages: true` option (v12.8.1+) is useful when:

✅ **You want to cache only network images** (HTTP/HTTPS)
- Saves disk space by not caching bundled/local images
- Focuses cache on images that are slow to load

❌ **Skip caching for local images** (`file://` URIs)
- Local images from Resources folder
- Bundled images
- Embedded images

Example:
```csharp
.UseSharpnadoNuke(
    loggerEnable: false,
    cacheOnlyRemoteImages: true)  // Only cache HTTP/HTTPS images
```

## Known Issues

`Maui.Nuke` cannot cache images coming from the Asset Catalog:

https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/images-icons/displaying-an-image

This is due to the fact that the `Asset Catalogue` is packed in the ipa, and you cannot get an image URI from it.
Since version 8.4.1, it will however cache correctly images respecting the density convention (@2x, @3x) locating in your `Resources` folder (see [Issue #13](https://github.com/roubachof/Xamarin.Forms.Nuke/issues/13)).

