namespace Rasterizr.Resources
{
	public abstract class TextureBase : Resource
	{
		private readonly int _numElements;
		private readonly Format _format;

		internal override int NumElements
		{
			get { return _numElements; }
		}

		internal Format Format
		{
			get { return _format; }
		}

		protected TextureBase(Device device, int numElements, Format format)
			: base(device, numElements * FormatHelper.SizeOfInBytes(format), FormatHelper.SizeOfInBytes(format))
		{
			_numElements = numElements;
			_format = format;
		}

		internal override sealed int CalculateByteOffset(int x, int y, int z)
		{
			return CalculateIndex(x, y, z) * FormatHelper.SizeOfInBytes(_format);
		}

		protected abstract int CalculateIndex(int x, int y, int z);
	}
}