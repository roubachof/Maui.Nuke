# Release Notes v12.8.2

## 🐛 Image Invalidation Fix - November 2025

This release fixes a critical MAUI bug where images don't properly invalidate/refresh when their source changes.

**Fixes**: [Issue #9](https://github.com/roubachof/Maui.Nuke/issues/9)

## 🔧 Bug Fixes

### Custom NukeImageHandler
- **Fixed**: MAUI's lack of image invalidation on iOS/MacCatalyst
- **Issue**: When an `Image.Source` property changed, the displayed image wouldn't update
- **Solution**: Implemented custom `NukeImageHandler` for iOS/MacCatalyst platforms
- **Impact**: Images now properly refresh when source changes dynamically
- **GitHub Issue**: [#9](https://github.com/roubachof/Maui.Nuke/issues/9)
- **Commit**: `e95a595`

### Better Image Lifecycle Management
- Improved image loading/unloading cycle
- Proper cleanup when images are disposed
- Better integration with MAUI's image pipeline

## ✨ What This Fixes

**Before v12.8.2:**
```csharp
// Changing image source didn't update the display
myImage.Source = "new_image.jpg";  // ❌ Image stayed the same
```

**After v12.8.2:**
```csharp
// Image properly refreshes
myImage.Source = "new_image.jpg";  // ✅ Image updates correctly
```

## 🎯 Use Cases

This fix is particularly important for:
- **Profile pictures** that update when user changes avatar
- **Dynamic content** that refreshes based on user actions
- **Image galleries** where images change frequently
- **Cached images** that need to be invalidated and reloaded

## 📦 Installation

```bash
dotnet add package Sharpnado.Maui.Nuke --version 12.8.2
```

Or update your `.csproj`:

```xml
<PackageReference Include="Sharpnado.Maui.Nuke" Version="12.8.2" />
```

## 🔄 Migration from 12.8.1

### Breaking Changes
None! This is a backward-compatible bug fix update.

### Update Steps
1. Update package to 12.8.2
2. No code changes required
3. Images will now properly invalidate automatically

## 🌐 Platform Support

| Platform | Nuke Caching | Custom Handler | Behavior |
|----------|--------------|----------------|----------|
| **iOS 15.0+** | ✅ Active | ✅ Yes | Full Nuke 12.8 + Image invalidation fix |
| **MacCatalyst 15.0+** | ✅ Active | ✅ Yes | Full Nuke 12.8 + Image invalidation fix |
| **Android API 21+** | ❌ No-op | ❌ No | Uses MAUI's built-in Glide |
| **Windows 10.0.17763.0+** | ❌ No-op | ❌ No | Uses MAUI's built-in caching |

## 📝 Sample Updates

The test applications have been upgraded:
- **Benchmark**: Updated to .NET 10
- **TestApp**: Updated to .NET 10
- Both projects now use the latest MAUI features

## 🔗 Dependencies

- Microsoft.Maui.Controls 9.0.82
- ImageCaching.Nuke 4.1.1 (iOS/MacCatalyst only)

## 📚 Documentation

- **GitHub Repository**: https://github.com/roubachof/Maui.Nuke
- **NuGet Package**: https://www.nuget.org/packages/Sharpnado.Maui.Nuke
- **Related Issue**: [#9 - Image not updating when source changes](https://github.com/roubachof/Maui.Nuke/issues/9)

## 💡 Technical Details

### The MAUI Bug

MAUI's default image handling doesn't properly invalidate images when the source changes on iOS/MacCatalyst. This is a known issue that affects apps using dynamic image sources (reported in issue #9).

### The Solution

`Maui.Nuke` v12.8.2 introduces `NukeImageHandler.ios.cs`, a custom image handler that:
1. Properly tracks image source changes
2. Invalidates cached images when needed
3. Triggers image reload with the new source
4. Maintains all Nuke caching benefits

This handler is automatically registered when you call `.UseSharpnadoNuke()` in your `MauiProgram.cs`.

## 📝 License

MIT License - see LICENSE file in the repository

## 🙏 Credits

Special thanks to:
- The [Nuke](https://github.com/kean/Nuke) team for the excellent iOS image caching library
- Community members who reported and helped identify issue #9
