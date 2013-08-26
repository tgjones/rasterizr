using Rasterizr.Diagnostics;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class VertexShaderDrawPixelHistoryEventPartViewModel : DrawPixelHistoryEventPartViewModel
    {
        private readonly DrawEvent _event;

        public override string Name
        {
            get { return "Vertex Shader"; }
        }

        public DrawEventVertex[] Vertices
        {
            get { return _event.Vertices; }
        }

        public VertexShaderDrawPixelHistoryEventPartViewModel(DrawEvent @event)
        {
            _event = @event;
        }
    }

    public class InputAssemblerDrawPixelHistoryEventPartViewModel : DrawPixelHistoryEventPartViewModel
    {
        private readonly DrawEvent _event;

        public override string Name
        {
            get { return "Input Assembler"; }
        }

        public DrawEventVertex[] Vertices
        {
            get { return _event.Vertices; }
        }

        public InputAssemblerDrawPixelHistoryEventPartViewModel(DrawEvent @event)
        {
            _event = @event;
        }
    }
}