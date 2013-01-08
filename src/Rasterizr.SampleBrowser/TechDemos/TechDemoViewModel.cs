using Caliburn.Micro;

namespace Rasterizr.SampleBrowser.TechDemos
{
	public abstract class TechDemoViewModel : Screen
	{
		public abstract string Category { get; }
	}
}