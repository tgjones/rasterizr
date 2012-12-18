namespace Rasterizr.Resources
{
	public class Texture3D : TextureBase
	{
		private readonly Texture3DDescription _description;

		public Texture3DDescription Description
		{
			get { return _description; }
		}

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Texture3D; }
		}

		internal Texture3D(Device device, Texture3DDescription description)
			: base(device, description.Width * description.Height * description.Depth, description.Format)
		{
			_description = description;
		}

		protected override int CalculateIndex(int x, int y, int z)
		{
			return (z * _description.Width * _description.Height) + (y * _description.Width) + x;
		}
	}
}