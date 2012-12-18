namespace Rasterizr.Resources
{
	public class Texture2D : TextureBase
	{
		private readonly Texture2DDescription _description;

		public static T FromSwapChain<T>(SwapChain swapChain, int index)
			where T : Resource
		{
			return swapChain.GetBuffer<T>(index);
		}

		public Texture2DDescription Description
		{
			get { return _description; }
		}

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Texture2D; }
		}

		internal Texture2D(Device device, Texture2DDescription description)
			: base(device, description.Width * description.Height, description.Format)
		{
			_description = description;
		}

		protected override int CalculateIndex(int x, int y, int z)
		{
			return (y * _description.Width) + x;
		}
	}
}