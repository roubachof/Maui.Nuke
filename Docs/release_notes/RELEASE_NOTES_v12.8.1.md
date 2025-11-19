# Release Notes v12.8.1

## ✨ Multi-Platform Support - November 2025

This release adds multi-platform compilation support and a new option to cache only remote images.

## 🎯 New Features

### Multi-Platform Compilation
- **Now compiles for all MAUI platforms**: Android, Windows, iOS, MacCatalyst
- **Target frameworks**:
  - `net9.0-android`
  - `net9.0-ios`
  - `net9.0-maccatalyst`
  - `net9.0-windows10.0.19041.0`

### Remote-Only Caching Option
- **New feature**: Option to cache only remote images (HTTP/HTTPS)
- **Excludes**: Local `file://` URIs from caching
- **Use case**: Avoid unnecessary caching of bundled or local images

### Improved Logging
- Better platform detection in logs
- Enhanced debugging information

## 🔧 Platform Behavior

### iOS & MacCatalyst ✅
- **Full Nuke 12.8 caching** (native Swift implementation)
- All features active:
  - Memory cache
  - Disk cache
  - Progressive loading
  - Prefetching

### Android 📱
- **Compiles successfully** for compatibility
- **Delegates to MAUI's built-in Glide** (no-op)
- Reason: Android already has excellent native caching via Glide

### Windows 🪟
- **Compiles successfully** for compatibility
- **Delegates to MAUI's built-in caching** (no-op)
- Reason: Windows has adequate built-in caching

## 💡 Why Multi-Platform?

Even though Nuke caching is iOS/MacCatalyst specific, multi-platform compilation provides:

✅ **Simplified project setup**: Single package for all platforms
✅ **No conditional PackageReference**: Add once, works everywhere
✅ **Code compatibility**: No need for platform-specific code blocks
✅ **Future-proofing**: Ready for potential Android/Windows enhancements

## 📦 Installation

```bash
dotnet add package Sharpnado.Maui.Nuke --version 12.8.1
```

Or update your `.csproj`:

```xml
<PackageReference Include="Sharpnado.Maui.Nuke" Version="12.8.1" />
```

## 🔄 Migration from 12.8.0

### Breaking Changes
None! This is a backward-compatible update.

### Update Steps
1. Update package to 12.8.1
2. (Optional) Configure remote-only caching if needed
3. No other code changes required

## 🌐 Platform Support

| Platform | Nuke Caching | Built-in MAUI Caching |
|----------|--------------|----------------------|
| iOS 15.0+ | ✅ Active | ➖ |
| Mac Catalyst 15.0+ | ✅ Active | ➖ |
| Android API 21+ | ➖ | ✅ (Glide) |
| Windows 10.0.17763.0+ | ➖ | ✅ (Default) |

## 📝 Usage

### Basic Setup (All Platforms)

```csharp
// In MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSharpnadoNuke(); // Works on all platforms
            
        return builder.Build();
    }
}
```

### Remote-Only Caching (New)

```csharp
builder.UseSharpnadoNuke(cacheOnlyRemoteImages: true);
```

This will:
- ✅ Cache images from HTTP/HTTPS URLs
- ❌ Skip caching for `file://` URIs (local images)

## 🔗 Dependencies

- Microsoft.Maui.Controls 9.0.82
- ImageCaching.Nuke 4.1.1 (iOS/MacCatalyst only)

## 📚 Documentation

- **GitHub Repository**: https://github.com/roubachof/Maui.Nuke
- **NuGet Package**: https://www.nuget.org/packages/Sharpnado.Maui.Nuke

## 🐛 Bug Fixes

- Improved platform detection in logger
- Better handling of platform-specific features

## 📝 License

MIT License - see LICENSE file in the repository

## 🙏 Credits

Special thanks to the [Nuke](https://github.com/kean/Nuke) team for creating and maintaining the excellent iOS image caching library.
