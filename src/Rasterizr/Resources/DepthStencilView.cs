namespace Rasterizr.Resources
{
	public class DepthStencilView : ResourceView
	{
		private readonly float[] _depths;

		public float this[int x, int y, int sampleIndex]
		{
			get
			{
				// TODO: Get rid of all these conversions.
				var texture = (Texture2D)Resource;
				return _depths[(y * texture.Description.Width) + x];
			}
			set
			{
				var texture = (Texture2D)Resource;
				_depths[(y * texture.Description.Width) + x] = value;
			}
		}

		public DepthStencilView(Device device, Texture2D resource)
			: base(device, resource)
		{
			_depths = new float[resource.Description.Width * resource.Description.Height];
		}

		internal void Invalidate()
		{
			Resource.SetData(_depths);
		}
	}
}