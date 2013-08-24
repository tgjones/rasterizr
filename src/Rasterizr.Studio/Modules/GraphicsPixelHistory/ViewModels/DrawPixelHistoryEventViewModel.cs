using Rasterizr.Diagnostics;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
    public class DrawPixelHistoryEventViewModel : PixelHistoryEventViewModelBase
    {
        private readonly DrawEvent _event;

        public string Name
        {
            get { return string.Format("Triangle {0}", _event.PrimitiveID); }
        }

        public PixelExclusionReason ExclusionReason
        {
            get { return _event.ExclusionReason; }
        }

        public ColorViewModel Previous
        {
            get { return new ColorViewModel(_event.Previous); }
        }

        public ColorViewModel PixelShader
        {
            get { return new ColorViewModel(_event.PixelShader); }
        }

        public override ColorViewModel Result
        {
            get { return new ColorViewModel(_event.Result); }
        }

        public DrawPixelHistoryEventViewModel(DrawEvent @event)
        {
            _event = @event;
        }
    }
}