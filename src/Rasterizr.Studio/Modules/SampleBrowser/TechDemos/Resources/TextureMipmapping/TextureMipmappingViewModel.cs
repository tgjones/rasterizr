using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Microsoft.Win32;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;

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

		private IEnumerable<MipMapViewModel> _mipMaps;
		public IEnumerable<MipMapViewModel> MipMaps
		{
			get { return _mipMaps; }
		    set
		    {
		        _mipMaps = value;
                NotifyOfPropertyChange(() => MipMaps);
		    }
		}

		public TextureMipmappingViewModel()
		{
		    DisplayName = "Texture Mipmapping";
            LoadImage("Modules/SampleBrowser/TechDemos/Resources/TextureMipmapping/window_28.jpg");
		}

	    public IEnumerable<IResult> LoadImage()
	    {
	        var openDialog = new OpenFileDialog
	        {
                Filter = "Image files (*.png, *.jpg)|*.png;*.jpg"
	        };
	        yield return Show.Dialog(openDialog);
	        LoadImage(openDialog.FileName);
	    }

	    private void LoadImage(string fileName)
	    {
	        Texture2D texture;
	        using (var fileStream = File.OpenRead(fileName))
                texture = TextureLoader.CreateTextureFromStream(new Device(), fileStream);

            MipMaps = Enumerable.Range(0, texture.Description.MipLevels)
                .Select((x, i) => new MipMapViewModel(TextureLoader.CreateBitmapFromTexture(texture, 0, x), i));
	    }
	}
}