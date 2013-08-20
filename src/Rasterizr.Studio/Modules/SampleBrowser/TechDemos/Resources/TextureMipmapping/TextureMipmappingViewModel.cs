using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gemini.Framework.Services;
using Rasterizr.Platform.Wpf;

namespace Rasterizr.Studio.Modules.SampleBrowser.TechDemos.Resources.TextureMipmapping
{
	[Export(typeof(TechDemoViewModel))]
	[ExportMetadata("SortOrder", 0)]
	public class TextureMipmappingViewModel : TechDemoViewModel
	{
		public override string Category
		{
			get { return "Resources"; }
		}

		private readonly IEnumerable<MipMapViewModel> _mipMaps;
		public IEnumerable<MipMapViewModel> MipMaps
		{
			get { return _mipMaps; }
		}

		[ImportingConstructor]
		public TextureMipmappingViewModel(IResourceManager resourceLoader)
		{
		    DisplayName = "Texture Mipmapping";

			var textureStream = resourceLoader.GetStream(
                "Modules/SampleBrowser/TechDemos/Resources/TextureMipmapping/window_28.jpg",
                GetType().Assembly.FullName);
			var texture = TextureLoader.CreateTextureFromStream(new Device(), textureStream);

			_mipMaps = Enumerable.Range(0, texture.Description.MipLevels)
				.Select((x, i) => new MipMapViewModel(TextureLoader.CreateBitmapFromTexture(texture, x), i));
		}
	}
}