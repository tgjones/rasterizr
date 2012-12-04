namespace Rasterizr.Resources
{
	public class Texture3D : TextureBase
	{
		private readonly Texture3DDescription _description;

		public Texture3DDescription Description
		{
			get { return _description; }
		}

		internal Texture3D(Device device, Texture3DDescription description)
			: base(device, description.Width * description.Height * description.Depth, description.Format)
		{
			_description = description;
		}
	}
}