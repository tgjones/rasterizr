using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class Texture2DViewModel : PropertyChangedBase
    {
        private readonly Texture2D _texture;

        public int Width
        {
            get { return _texture.Description.Width; }
        }

        public int Height
        {
            get { return _texture.Description.Height; }
        }

        public int MipLevels
        {
            get { return _texture.Description.MipLevels; }
        }

        public int ArraySize
        {
            get { return _texture.Description.ArraySize; }
        }

        public string BindFlags
        {
            get { return _texture.Description.BindFlags.ToString(); }
        }

        public IEnumerable<int> ArraySlices
        {
            get { return Enumerable.Range(0, _texture.Description.ArraySize); }
        }

        private int _selectedArraySlice;
        public int SelectedArraySlice
        {
            get { return _selectedArraySlice; }
            set
            {
                _selectedArraySlice = value;
                NotifyOfPropertyChange(() => SelectedArraySlice);
                NotifyOfPropertyChange(() => MipMaps);
            }
        }

        public IEnumerable<TextureMipMapViewModel> MipMaps
        {
            get
            {
                return Enumerable.Range(0, _texture.Description.MipLevels)
                    .Select(x => new TextureMipMapViewModel(TextureLoader.CreateBitmapFromTexture(_texture, _selectedArraySlice, x), x));
            }
        }

        public Texture2DViewModel(Texture2D texture)
        {
            _texture = texture;
        }
    }

    public class TextureMipMapViewModel
    {
        private readonly WriteableBitmap _bitmap;
        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
        }

        private readonly string _description;
        public string Description
        {
            get { return _description; }
        }

        public TextureMipMapViewModel(WriteableBitmap bitmap, int level)
        {
            _bitmap = bitmap;
            _description = string.Format("Mip level {0}: {1} x {2}", level, bitmap.PixelWidth, bitmap.PixelHeight);
        }
    }
}