using Caliburn.Micro;
using Rasterizr.Resources;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class GraphicsObjectViewModel : PropertyChangedBase
    {
        private readonly DeviceChild _deviceChild;

        internal DeviceChild DeviceChild
        {
            get { return _deviceChild; }
        }

        public string Identifier
        {
            get { return "obj:" + _deviceChild.ID; }
        }

        public string Type
        {
            get { return _deviceChild.GetType().Name; }
        }

        public bool IsActive
        {
            get { return true; }
        }

        public int Size
        {
            get
            {
                var resource = _deviceChild as Resource;
                if (resource == null)
                    return 0;
                return resource.Size;
            }
        }

        public int Mips
        {
            get
            {
                if (_deviceChild is Texture1D)
                    return ((Texture1D) _deviceChild).Description.MipLevels;
                if (_deviceChild is Texture2D)
                    return ((Texture2D) _deviceChild).Description.MipLevels;
                if (_deviceChild is Texture3D)
                    return ((Texture3D) _deviceChild).Description.MipLevels;
                return 0;
            }
        }

        public int Width
        {
            get
            {
                if (_deviceChild is Texture1D)
                    return ((Texture1D) _deviceChild).Description.Width;
                if (_deviceChild is Texture2D)
                    return ((Texture2D) _deviceChild).Description.Width;
                if (_deviceChild is Texture3D)
                    return ((Texture3D) _deviceChild).Description.Width;
                return 0;
            }
        }

        public int Height
        {
            get
            {
                if (_deviceChild is Texture2D)
                    return ((Texture2D) _deviceChild).Description.Height;
                if (_deviceChild is Texture3D)
                    return ((Texture3D) _deviceChild).Description.Height;
                return 0;
            }
        }

        public int Depth
        {
            get
            {
                if (_deviceChild is Texture3D)
                    return ((Texture3D) _deviceChild).Description.Depth;
                return 0;
            }
        }

        public int ArraySize
        {
            get
            {
                if (_deviceChild is Texture1D)
                    return ((Texture1D) _deviceChild).Description.ArraySize;
                if (_deviceChild is Texture2D)
                    return ((Texture2D) _deviceChild).Description.ArraySize;
                return 0;
            }
        }

        public GraphicsObjectViewModel(DeviceChild deviceChild)
        {
            _deviceChild = deviceChild;
        }
    }
}