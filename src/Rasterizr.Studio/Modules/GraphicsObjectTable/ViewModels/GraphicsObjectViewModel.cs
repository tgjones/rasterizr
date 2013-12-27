using Caliburn.Micro;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class GraphicsObjectViewModel : PropertyChangedBase
    {
        public string Identifier
        {
            get { return "1234"; }
        }

        public string Type
        {
            get { return "Depth-Stencil State"; }
        }

        public bool IsActive
        {
            get { return true; }
        }

        public int Size
        {
            get { return 100; }
        }

        public int Mips
        {
            get { return 5; }
        }

        public int Width
        {
            get { return 800; }
        }

        public int Height
        {
            get { return 600; }
        }

        public int Depth
        {
            get { return 1; }
        }

        public GraphicsObjectViewModel(DeviceChild deviceChild)
        {
            
        }
    }
}