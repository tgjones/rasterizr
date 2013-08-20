using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.SampleBrowser.TechDemos
{
	public abstract class TechDemoViewModel : Screen, ISample
	{
		public abstract string Category { get; }
	}
}