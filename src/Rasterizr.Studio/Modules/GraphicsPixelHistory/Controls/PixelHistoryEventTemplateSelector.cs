using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Controls
{
    public class PixelHistoryEventTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SimpleEventTemplate { get; set; }
        public DataTemplate DrawEventTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var @event = (PixelHistoryEventViewModel) item;
            var subEvent = @event.PixelHistoryEvents.First();

            if (subEvent is SimplePixelHistoryEventViewModel)
                return SimpleEventTemplate;
            if (subEvent is DrawPixelHistoryEventViewModel)
                return DrawEventTemplate;
            throw new ArgumentException();
        }
    }
}