using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;

namespace Rasterizr.SampleBrowser.Framework.Services
{
	[Export(typeof(IResourceLoader))]
	public class ResourceLoader : IResourceLoader
	{
		public Stream OpenResource(string path)
		{
			var streamResourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/" + path));
			if (streamResourceInfo == null)
				throw new ArgumentException("Could not find resource at this path: " + path);
			return streamResourceInfo.Stream;
		}
	}
}