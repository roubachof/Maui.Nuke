namespace Benchmark.Views;

public static class Images
{
	static readonly Random random = new Random ();
	static readonly string tempDir = Path.Combine (Path.GetTempPath (), "glide.forms.sample");
	static readonly string tempPath = Path.Combine (tempDir, "temp.jpg");
	const int MaxImages = 7;

    public static IEnumerable<ImageSource> Sources (int imageCount = 100)
    {
        var random = new Random ();
        for (int i = 0; i < imageCount; i++) {
            yield return SourceById(i);
        }
    }

	public static ImageSource SourceById (int x)
    {
        int modulo = x % MaxImages;
		switch (modulo) {
			//Urls
			case 0:
				return ImageSource.FromUri (new Uri ("https://s3.amazonaws.com/app.growtix.com/media/big/34/28/23/5a9f03c2-3734-4486-bd04-2d45ac1c102e.png"));
			case 1:
				return ImageSource.FromUri (new Uri ("https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Steven_Seagal_November_2016.jpg/230px-Steven_Seagal_November_2016.jpg"));
			case 2:
				return ImageSource.FromUri (new Uri ("https://www.biography.com/.image/ar_1:1%2Cc_fill%2Ccs_srgb%2Cg_face%2Cq_80%2Cw_300/MTIwNjA4NjM0MDQyNzQ2Mzgw/hulk-hogan-9542305-1-402.jpg"));
			//Resources
			case 3:
			case 4:
			case 5:
			case 6:
				return ImageSource.FromFile ("patch" + (modulo - 2) + ".jpg");
			//File path
			//case 7:
			//	if (!File.Exists(tempPath)) {
			//		Directory.CreateDirectory (tempDir);
			//		using (var assetStream = Android.App.Application.Context.Assets.Open("assetpatch1.jpg"))
			//		using (var fileStream = File.Create (tempPath)) {
			//			assetStream.CopyTo (fileStream);
			//		}
			//	}
			//	return ImageSource.FromFile (tempPath);
			////Stream
			//case 8:
			//	return ImageSource.FromStream (() => Android.App.Application.Context.Assets.Open ("assetpatch2.jpg"));
			////Embedded Resource
			//case 9:
			//	return ImageSource.FromResource ("Android.Glide.Sample.embeddedpatch1.jpg", typeof (App));
			default:
				throw new NotImplementedException ($"Whoops {x} not implemented!");
		}
	}
}