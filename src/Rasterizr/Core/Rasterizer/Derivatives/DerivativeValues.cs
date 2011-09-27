namespace Rasterizr.Core.Rasterizer.Derivatives
{
	public abstract class DerivativeValues<TValue, TDerivative>
	{
		private readonly TValue[] _values;

		public TValue this[FragmentQuadLocation location]
		{
			get { return _values[(int) location]; }
			set { _values[(int) location] = value; }
		}

		protected DerivativeValues()
		{
			_values = new TValue[4];
		}

		public TDerivative Ddx(FragmentQuadLocation currentLocation)
		{
			int y = (currentLocation == FragmentQuadLocation.TopLeft || currentLocation == FragmentQuadLocation.TopRight) ? 0 : 1;
			return Subtract(_values[(y * 2) + 1], _values[(y * 2) + 0]);
		}

		public TDerivative Ddy(FragmentQuadLocation currentLocation)
		{
			int x = (currentLocation == FragmentQuadLocation.TopLeft || currentLocation == FragmentQuadLocation.BottomLeft) ? 0 : 1;
			return Subtract(_values[x + 2], _values[x + 0]);
		}

		protected abstract TDerivative Subtract(TValue v1, TValue v2);
	}
}