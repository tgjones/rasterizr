namespace Rasterizr.Resources
{
	public class Texture1D : TextureBase
	{
		private readonly Texture1DDescription _description;

		public Texture1DDescription Description
		{
			get { return _description; }
		}

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Texture1D; }
		}

		internal Texture1D(Device device, Texture1DDescription description)
			: base(device, description.Width, description.Format)
		{
			_description = description;
		}

		protected override int CalculateIndex(int x, int y, int z)
		{
			return x;
		}
	}
}