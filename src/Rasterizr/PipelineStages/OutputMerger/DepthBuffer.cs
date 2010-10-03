using System;

namespace Rasterizr.PipelineStages.OutputMerger
{
	public class DepthBuffer
	{
		private readonly int _width;
		private readonly int _height;
		private float[,] _values;

		public float this[int x, int y]
		{
			get { return _values[x, y]; }
			set { _values[x, y] = value; }
		}

		public DepthBuffer(int width, int height)
		{
			_width = width;
			_height = height;
			_values = new float[width,height];
		}

		public void Clear(float value)
		{
			for (int y = 0; y < _height; ++y)
				for (int x = 0; x < _width; ++x)
					_values[x, y] = value;
		}
	}
}