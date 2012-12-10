using Caliburn.Micro;
using Rasterizr.Math;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels
{
	public class ColorViewModel : PropertyChangedBase
	{
		private readonly Color4F _color;

		public Color4F Color
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

		public ColorViewModel(Color4F color)
		{
			_color = color;
		}
	}
}