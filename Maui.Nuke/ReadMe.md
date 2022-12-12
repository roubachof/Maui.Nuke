# Maui.Nuke

Since MAUI, the `Android` platform get a native caching library: `Glide`.
Unfortunately on, `iOS`, there is no native caching...

`Maui.Nuke` is here to repair this injustice by implementing image caching with the fastest and most popular ios native caching library: [Nuke](https://github.com/kean/Nuke/).

Moreover, once installed, it is completly transparent to the user, you use your `Image` views just like before, all the work is done under the hood.

This project is using the [NukeProxy](https://github.com/roubachof/NukeProxy) library, which is a Swift .net6 proxy to the nuke native library. The new binding and the packaging has been done by the great @cheesebaron. Hail to the Cheese!

Current version of the Nuke library is 10.3.1.

## Installation

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseNuke(showDebugLogs: false);
}
```


### BOOM

You just achieved **90%+** memory reduction when manipulating ```Image``` views.


## Known Issues

`Maui.Nuke` cannot cache images coming from the Asset Catalog:

https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/images-icons/displaying-an-image

This is due to the fact that the `Asset Catalogue` is packed in the ipa, and you cannot get an image URI from it.
Since version 8.4.1, it will however cache correctly images respecting the density convention (@2x, @3x) locating in your `Resources` folder (see [Issue #13](https://github.com/roubachof/Xamarin.Forms.Nuke/issues/13)).

