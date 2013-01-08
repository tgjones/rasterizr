using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Rasterizr.Platform.Wpf;

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

		public TextureMipmappingViewModel()
		{
			var textureStream = Application.GetResourceStream(new Uri("pack://application:,,,/TechDemos/Resources/TextureMipmapping/window_28.jpg")).Stream;
			var texture = TextureHelper.CreateTextureFromFile(new Device(), textureStream);

			_mipMaps = Enumerable.Range(0, texture.Description.MipLevels)
				.Select((x, i) => new MipMapViewModel(TextureHelper.CreateBitmapFromTexture(texture, x), i));
		}
	}
}