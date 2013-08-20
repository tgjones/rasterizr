using Caliburn.Micro;
using Rasterizr.SampleBrowser.Framework;

namespace Rasterizr.SampleBrowser.TechDemos
{
	public abstract class TechDemoViewModel : Screen, ISample
	{
		public abstract string Category { get; }
	}
}