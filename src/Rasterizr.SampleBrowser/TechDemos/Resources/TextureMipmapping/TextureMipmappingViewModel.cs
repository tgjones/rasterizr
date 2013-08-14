using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Rasterizr.Platform.Wpf;
using Rasterizr.SampleBrowser.Framework.Services;

namespace Rasterizr.SampleBrowser.TechDemos.Resources.TextureMipmapping
{
	[Export(typeof(TechDemoViewModel))]
	[ExportMetadata("SortOrder", 0)]
	public class TextureMipmappingViewModel : TechDemoViewModel
	{
		public override string DisplayName
		{
			get { return "Texture Mipmapping"; }
			set { base.DisplayName = value; }
		}

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
		public TextureMipmappingViewModel(IResourceLoader resourceLoader)
		{
			var textureStream = resourceLoader.OpenResource("TechDemos/Resources/TextureMipmapping/window_28.jpg");
			var texture = TextureLoader.CreateTextureFromStream(new Device(), textureStream);

			_mipMaps = Enumerable.Range(0, texture.Description.MipLevels)
				.Select((x, i) => new MipMapViewModel(TextureLoader.CreateBitmapFromTexture(texture, x), i));
		}
	}
}