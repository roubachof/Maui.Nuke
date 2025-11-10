# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

Maui.Nuke is a .NET MAUI library that brings native iOS image caching to MAUI applications using the [Nuke](https://github.com/kean/Nuke/) Swift library (v10.3.1). It wraps the native Nuke library via [NukeProxy](https://github.com/roubachof/NukeProxy), providing transparent image caching without requiring code changes in consuming applications.

**Key characteristics:**
- iOS/macCatalyst only (net6.0-ios, net6.0-maccatalyst targets)
- Transparent integration - replaces standard MAUI image handlers
- Achieves 90%+ memory reduction compared to standard MAUI Image views
- Published as `Sharpnado.Maui.Nuke` NuGet package

## Build Commands

### Build the library
```bash
dotnet build Maui.Nuke/Maui.Nuke.sln
```

### Build the benchmark app
```bash
dotnet build Benchmark/Benchmark.sln
```

### Clean build artifacts
```pwsh
pwsh Clean-BinObj.ps1
```

### Create NuGet package
The project is configured with `<GeneratePackageOnBuild>true</GeneratePackageOnBuild>`, so building in Release mode automatically generates the NuGet package:
```bash
dotnet build Maui.Nuke/Maui.Nuke.csproj -c Release
```

### Run benchmark app on iOS Simulator
```bash
dotnet build Benchmark/Benchmark.csproj -f net6.0-ios -t:Run
```

## Architecture

### Core Components

**Library Structure (Maui.Nuke/):**
- `ImageSourcesMauiAppBuilderExtensions.cs` - Entry point for integration via `.UseNuke()` extension method
- `NukeController.cs` - Central interface to native Nuke library, handles async image loading and cache management
- `NukeFileImageSourceService.cs` - Custom MAUI image source handler for file-based images
- `NukeUriImageSourceService.cs` - Custom MAUI image source handler for URI-based images
- `LoggerWrapper.cs` - Conditional logging wrapper respecting `ShowDebugLogs` flag

**Integration Pattern:**
The library uses MAUI's image source service extensibility to intercept image loading. When `.UseNuke()` is called during app initialization:
1. Registers custom `NukeFileImageSourceService` and `NukeUriImageSourceService` handlers
2. These replace the default MAUI image handlers with Nuke-backed implementations
3. All `Image` views automatically use Nuke caching without code changes

**Density-Aware Image Loading:**
`NukeFileImageSourceService` implements iOS density suffix convention (@2x, @3x) to load appropriate resolution images from the Resources folder. Falls back to UIImage.FromBundle for Asset Catalog images (though these cannot be cached).

**Cache Management:**
`NukeController.ClearCache()` provides programmatic cache clearing via `DataLoader.Shared.RemoveAllCachedResponses()` and `ImageCache.Shared.RemoveAll()`.

### Benchmark App Structure (Benchmark/)

The benchmark app is a test harness for memory profiling:
- **Profiler/** - Memory monitoring system (`MemoryProfiler.cs` tracks WorkingSet64)
- **Views/** - Test pages with different image loading scenarios (Grid, ViewCell, ImageCell, HugeImage, etc.)
- Uses `System.Diagnostics.Process.GetCurrentProcess().WorkingSet64` for memory measurements
- Configurable to test with/without Nuke by commenting lines in `MauiProgram.cs`

## Key Implementation Details

### iOS-Only Target
All Nuke integration code is wrapped in `#if __IOS__` preprocessor directives. The library only targets iOS and macCatalyst platforms.

### Async Image Loading
Images are loaded via `NukeController.LoadImageAsync()`, which wraps Nuke's callback-based API in a `TaskCompletionSource<UIImage?>` pattern.

### Error Handling
Both image source services implement graceful degradation:
- Return `null` on cancellation or failure
- Log warnings/errors through `LoggerWrapper`
- File sources fall back to `UIImage.FromBundle` if file paths fail

### Debug Logging
Enable verbose logging via `.UseNuke(showDebugLogs: true)` - this changes LogLevel from Debug to Information for easier visibility.

## Known Limitations

1. **Asset Catalog images cannot be cached** - iOS Asset Catalogs are packed in the IPA without accessible URIs
2. **Density-suffixed images** (@2x, @3x) must be in the Resources folder to be cached properly (not in Asset Catalog)
3. **iOS/macCatalyst only** - No Android/Windows support (Android already has Glide natively in MAUI)

## Code Style

- StyleCop rules enabled via `StyleCopRules.ruleset`
- XAML styling configuration in `Settings.XamlStyler`
- Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- Nullable reference types enabled (`<Nullable>enable</Nullable>`)
