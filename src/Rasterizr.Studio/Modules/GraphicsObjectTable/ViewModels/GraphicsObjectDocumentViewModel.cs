using Gemini.Framework;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Resources;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class GraphicsObjectDocumentViewModel : Document
    {
        private readonly GraphicsObjectViewModel _objectViewModel;

        public object GraphicsObject
        {
            get
            {
                if (_objectViewModel.DeviceChild is BlendState)
                    return new BlendStateViewModel((BlendState) _objectViewModel.DeviceChild);
                if (_objectViewModel.DeviceChild is InputLayout)
                    return new InputLayoutViewModel((InputLayout) _objectViewModel.DeviceChild);
                if (_objectViewModel.DeviceChild is Buffer)
                    return new BufferViewModel((Buffer) _objectViewModel.DeviceChild);
                if (_objectViewModel.DeviceChild is ShaderBase)
                    return new ShaderViewModel((ShaderBase) _objectViewModel.DeviceChild);
                if (_objectViewModel.DeviceChild is Texture2D)
                    return new Texture2DViewModel((Texture2D) _objectViewModel.DeviceChild);
                return null;
            }
        }

        public GraphicsObjectDocumentViewModel(GraphicsObjectViewModel objectViewModel)
        {
            DisplayName = objectViewModel.Type + " (" + objectViewModel.Identifier + ")";
            _objectViewModel = objectViewModel;
        }
    }
}