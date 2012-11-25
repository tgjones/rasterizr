namespace Rasterizr.Resources
{
	public class Texture1D : TextureBase
	{
		private readonly Texture1DDescription _description;

		public Texture1DDescription Description
		{
			get { return _description; }
		}

		public Texture1D(Device device, Texture1DDescription description)
			: base(device, description.Width, description.Format)
		{
			_description = description;
		}
	}
}