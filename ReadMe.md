# Maui.Nuke

<p align="left"><img src="__Docs__/nuke_big.png" height="180"/>

[Nuke](https://github.com/kean/Nuke/) image caching library for dotnet MAUI.

Get it from NuGet:

[![Nuget](https://img.shields.io/nuget/v/Sharpnado.Maui.Nuke.svg)](https://www.nuget.org/packages/sharpnado.maui.nuke)

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


## Benchmark (old one)

I changed a bit the ```glidex``` benchmark samples to have a more fair comparison. I switched from a random distribution of the images to a deterministic one to be sure we are comparing the same data set.

I used ```System.Diagnostics.Process.GetCurrentProcess().WorkingSet64``` to have the memory workload of the process. The value given in the results are the **consumed bytes** between the ```MainPage``` and the complete loading of the target page.

The tests have been made on an iPhone 7 (real device, not a simulator).

For each test:

1. Launch iPhone 7
2. Wait 4-5 seconds on ```MainPage```
3. Launch a Page
4. Scroll till the end of page
5. Get consumed bytes in the output window
6. Empty caches
7. Kill app


<table>
	<thead>
		<tr>
      		<th>Page</th>
      		<th>Data Type</th>
			<th>Xamarin.Forms 4.5.0.356</th>
      		<th>Xamarin.Forms.Nuke 8.4.0</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td>GridOnlyRemotePage</td>
			<td>Remote only</td>
			<td align="right">248 905 728</td>
			<td align="right">15 073 280 <b><font color="greev">(-94%)</font></b></td>
		</tr>
		<tr>
			<td>GridPage</td>
			<td>Remote and local mix</td>
			<td align="right">195 035 136</td>
			<td align="right">15 040 512 <b><font color="greev">(-92%)</font></b></td>
		</tr>
		<tr>
			<td>ViewCellPage</td>
			<td>Remote and local mix</td>
			<td align="right">41 418 752</td>
			<td align="right">20 758 528 <b><font color="greev">(-50%)</font></b></td>
		</tr>
		<tr>
			<td>ImageCellPage</td>
			<td>Remote and local mix</td>
			<td align="right">27 000 832</td>
			<td align="right">20 611 072 <b><font color="greev">(-24%)</font></b></td>
		</tr>
		<tr>
			<td>HugeImagePage</td>
			<td>Local only</td>
			<td align="right">128 516 096</td>
			<td align="right">8 634 368 <b><font color="greev">(-93%)</font></b></td>
		</tr>
	</tbody>
</table>

### Comparison with FFImageLoading

Before I could successfully bind the `Nuke` swift library, I tried to use `FFImageLoading` as image source handler. You can find the older repository here: 

https://github.com/roubachof/Xamarin.Forms.ImageSourceHandlers

As expected the native `Nuke` library outperforms `FFImageLoading` on every test.

<table>
	<thead>
		<tr>
      		<th>Page</th>
      		<th>Data Type</th>
			<th>FFImageLoading 2.4.11.982</th>
      		<th>Xamarin.Forms.Nuke 8.4.0</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td>GridOnlyRemotePage</td>
			<td>Remote only</td>
			<td align="right">25 722 880</td>
			<td align="right">15 073 280 <b><font color="greev">(-41%)</font></b></td>
		</tr>
		<tr>
			<td>GridPage</td>
			<td>Remote and local mix</td>
			<td align="right">24 674 304</td>
			<td align="right">15 040 512 <b><font color="greev">(-39%)</font></b></td>
		</tr>
		<tr>
			<td>ViewCellPage</td>
			<td>Remote and local mix</td>
			<td align="right">28 852 224 <i>(1)</i></td>
			<td align="right">20 758 528 <b><font color="greev">(-28%)</font></b></td>
		</tr>
		<tr>
			<td>ImageCellPage</td>
			<td>Remote and local mix</td>
			<td align="right">28 868 608 <i>(2)</i></td>
			<td align="right">20 611 072 <b><font color="greev">(-28%)</font></b></td>
		</tr>
		<tr>
			<td>HugeImagePage</td>
			<td>Local only</td>
			<td align="right">10 059 776</td>
			<td align="right">8 634 368 <b><font color="greev">(-14%)</font></b></td>
		</tr>
	</tbody>
</table>

* *(1)* often fails to load first images (failed 7 times on 10)
* *(2)* often fails to load some images (failed 6 times on 10)


And more importantly, it loads way faster the cells images:

#### View Cells test

<table>
	<thead>
		<tr>
			<th>FFImageLoading</th>
			<th>Nuke</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td><img src="__Docs__/ffil_view_cells.gif" width="300" /></td>
			<td><img src="__Docs__/nukex_view_cells.gif" width="300" /></td>
		</tr>
  </tbody>
</table>

#### Image Cells test

<table>
	<thead>
		<tr>
			<th>FFImageLoading</th>
			<th>Nuke</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td><img src="__Docs__/ffil_image_cells.gif" width="300" /></td>
			<td><img src="__Docs__/nukex_image_cells.gif" width="300" /></td>
		</tr>
  </tbody>
</table>
