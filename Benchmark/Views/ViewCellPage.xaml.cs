namespace Benchmark.Views;

public partial class ViewCellPage
{
	public ViewCellPage ()
	{
		InitializeComponent ();

		BindingContext = Images.Sources().ToArray ();
	}
}