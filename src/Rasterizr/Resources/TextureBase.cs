namespace Rasterizr.Resources
{
	public abstract class TextureBase : Resource
 	{
		protected TextureBase(Device device, int numElements, Format format)
			: base(device, numElements * FormatHelper.SizeOfInBytes(format))
		{
		}
 	}
}