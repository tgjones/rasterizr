using Caliburn.Micro;
using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	public class ColorViewModel : PropertyChangedBase
	{
        private readonly Number4 _color;

        public Number4 Color
		{
			get { return _color; }
		}

		public float Red
		{
			get { return _color.R; }
		}

		public float Blue
		{
			get { return _color.B; }
		}

		public float Green
		{
			get { return _color.G; }
		}

		public float Alpha
		{
			get { return _color.A; }
		}

        public ColorViewModel(Number4 color)
		{
			_color = color;
		}
	}
}