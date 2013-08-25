using System.Windows.Media.Imaging;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
	public abstract class SampleBase
	{
		public abstract string Name { get; }

		/// <summary>
		/// In a derived class, implements logic to initialize the sample.
		/// </summary>
		public abstract WriteableBitmap Initialize(Device device);

		/// <summary>
		/// In a derived class, implements logic to render the sample.
		/// </summary>
        public virtual void Draw(float time)
		{
		}
	} 
}