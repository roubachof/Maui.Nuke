# Release Notes v12.8.0

## 🚀 Major Update - November 2025

This is a major version update bringing .NET 9 support and the latest Nuke library version.

## ✨ New Features & Updates

### .NET 9 Support
- **Updated to .NET 9**: Full compatibility with the latest .NET MAUI 9 framework
- **Target frameworks**: `net9.0-ios` and `net9.0-maccatalyst`
- **MAUI Controls**: Updated to Microsoft.Maui.Controls 9.0.82

### Nuke 12.8 Integration
- **Updated ImageCaching.Nuke**: Now using version 4.1.1 (Nuke 12.8)
- **Performance improvements**: Latest Nuke optimizations for faster image loading
- **Better memory management**: Enhanced caching algorithms from Nuke 12.8

## 📦 Installation

```bash
dotnet add package Sharpnado.Maui.Nuke --version 12.8.0
```

Or update your `.csproj`:

```xml
<PackageReference Include="Sharpnado.Maui.Nuke" Version="12.8.0" />
```

## 🔄 Migration from 10.11.2

### Breaking Changes
- Requires .NET 9 and MAUI 9
- If you're still on .NET 8, please stay on version 10.11.2

### Update Steps
1. Update your project to .NET 9
2. Update Sharpnado.Maui.Nuke to 12.8.0
3. No code changes required - API remains compatible

## 🌐 Platform Support

| Platform | Supported |
|----------|-----------|
| iOS 15.0+ | ✅ |
| Mac Catalyst 15.0+ | ✅ |

**Note**: This library is iOS/MacCatalyst only. Android has native Glide support through MAUI.

## 🎯 What is Maui.Nuke?

Maui.Nuke brings the power of iOS's most popular native image caching library (Nuke) to .NET MAUI. While Android has Glide built-in, iOS lacks native caching - Maui.Nuke solves this by:

- **Native performance**: Uses Nuke 12.8 under the hood
- **Zero configuration**: Works out of the box with MAUI's Image control
- **Automatic caching**: Memory and disk caching handled automatically
- **Fast loading**: Preheating, progressive loading, and more

## 📚 Documentation

- **GitHub Repository**: https://github.com/roubachof/Maui.Nuke
- **NuGet Package**: https://www.nuget.org/packages/Sharpnado.Maui.Nuke

## 🔗 Dependencies

- Microsoft.Maui.Controls 9.0.82
- ImageCaching.Nuke 4.1.1 (Nuke 12.8)

## 📝 License

MIT License - see LICENSE file in the repository

## 🙏 Credits

Special thanks to the [Nuke](https://github.com/kean/Nuke) team for creating and maintaining the excellent iOS image caching library that powers this package.
