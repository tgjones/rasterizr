using System.IO;

namespace Rasterizr.SampleBrowser.Framework.Services
{
	public interface IResourceLoader
	{
		Stream OpenResource(string path);
	}
}