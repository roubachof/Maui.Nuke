# Release Notes v12.8.3

## 🐛 Image Scaling Fix - November 2025

This release fixes an important issue with image scaling on retina displays (iOS/MacCatalyst).

**Fixes**: [Issue #8](https://github.com/roubachof/Maui.Nuke/issues/8)

## 🔧 Bug Fixes

### UIImage Scaling Information
- **Fixed**: UIImage scale factor not being returned correctly
- **Issue**: Images weren't respecting device pixel density (@2x, @3x)
- **Impact**: Images now render properly on retina displays
- **GitHub Issue**: [#8](https://github.com/roubachof/Maui.Nuke/issues/8)
- **Commit**: `4d301b9`

## ✨ What This Fixes

### The Problem

iOS devices use different pixel densities for their screens:
- **@1x**: Standard displays (older devices)
- **@2x**: Retina displays (most iPhones)
- **@3x**: Super Retina displays (iPhone Pro models)

When the scale factor wasn't properly returned, images could appear:
- Blurry or pixelated
- Too large or too small
- With incorrect resolution

### The Solution

The `NukeFileImageSourceService` and `NukeUriImageSourceService` now correctly return the UIImage with its original scale information, ensuring:
- ✅ Crisp, sharp images on all displays
- ✅ Correct sizing based on device pixel density
- ✅ Proper @2x/@3x image selection

## 🎯 Impact

This fix affects:
- **All iOS/MacCatalyst apps** using Maui.Nuke
- **Retina display rendering** - Images now look sharp and crisp
- **Image sizing** - Correct dimensions on all devices
- **Resource selection** - Proper @2x/@3x image variants

## 📦 Installation

```bash
dotnet add package Sharpnado.Maui.Nuke --version 12.8.3
```

Or update your `.csproj`:

```xml
<PackageReference Include="Sharpnado.Maui.Nuke" Version="12.8.3" />
```

## 🔄 Migration from 12.8.2

### Breaking Changes
None! This is a backward-compatible bug fix update.

### Update Steps
1. Update package to 12.8.3
2. No code changes required
3. Images will automatically render with correct scaling

## 🌐 Platform Support

| Platform | Nuke Caching | Scaling Fix | Behavior |
|----------|--------------|-------------|----------|
| **iOS 15.0+** | ✅ Active | ✅ Yes | Full Nuke 12.8 + Correct scaling |
| **MacCatalyst 15.0+** | ✅ Active | ✅ Yes | Full Nuke 12.8 + Correct scaling |
| **Android API 21+** | ❌ No-op | N/A | Uses MAUI's built-in Glide |
| **Windows 10.0.17763.0+** | ❌ No-op | N/A | Uses MAUI's built-in caching |

## 🔗 Dependencies

- Microsoft.Maui.Controls 9.0.82
- ImageCaching.Nuke 4.1.1 (iOS/MacCatalyst only)

## 📚 Documentation

- **GitHub Repository**: https://github.com/roubachof/Maui.Nuke
- **NuGet Package**: https://www.nuget.org/packages/Sharpnado.Maui.Nuke
- **Related Issue**: [#8 - Image scaling on retina displays](https://github.com/roubachof/Maui.Nuke/issues/8)

## 💡 Technical Details

### The Scaling Bug

When loading images through Nuke, the scale factor information embedded in the UIImage wasn't being preserved. This caused iOS to treat all images as @1x (standard density), leading to:
- Blurry images on retina displays
- Incorrect image dimensions
- Poor visual quality

### The Fix

The `NukeFileImageSourceService.cs` and `NukeUriImageSourceService.cs` now properly extract and return the scale information:

```csharp
// Before (incorrect)
return new UIImage(...);  // ❌ Scale info lost

// After (correct)  
return new UIImage(..., scale: originalScale);  // ✅ Scale preserved
```

This ensures that UIImage maintains its original scale factor, allowing iOS to render images correctly for the device's pixel density.

## 📝 License

MIT License - see LICENSE file in the repository

## 🙏 Credits

Special thanks to:
- The [Nuke](https://github.com/kean/Nuke) team for the excellent iOS image caching library
- Community members who reported and helped identify issue #8
