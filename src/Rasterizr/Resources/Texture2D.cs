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

		public Texture2D(Device device, Texture2DDescription description)
			: base(device, description.Width * description.Height, description.Format)
		{
			_description = description;
		}
	}
}