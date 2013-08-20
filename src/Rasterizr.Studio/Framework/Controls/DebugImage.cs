using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Rasterizr.Studio.Framework.Controls
{
    public class DebugImage : Control
    {
        public static DependencyProperty SourceProperty = Image.SourceProperty.AddOwner(typeof(DebugImage));

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static readonly DependencyPropertyKey MousePositionPropertyKey = DependencyProperty.RegisterReadOnly(
            "MousePosition", typeof(Point), typeof(DebugImage), new PropertyMetadata(new Point()));

        public static DependencyProperty MousePositionProperty = MousePositionPropertyKey.DependencyProperty;

        public Point MousePosition
        {
            get { return (Point) GetValue(MousePositionProperty); }
            private set { SetValue(MousePositionPropertyKey, value); }
        }

        static DebugImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DebugImage),
                new FrameworkPropertyMetadata(typeof(DebugImage)));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var p = e.GetPosition(this);
            MousePosition = new Point((int) p.X, (int) p.Y);
            base.OnMouseMove(e);
        }
    }
}