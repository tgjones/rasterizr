using Rasterizr.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView
	{
		private class Texture2DView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource[] _subresources;

			public Texture2DView(Texture2D resource, ShaderResourceViewDescription.Texture2DResource description, Format format)
				: base(format)
			{
				_subresources = new Texture2D.Texture2DSubresource[description.MipLevels];
				for (int i = description.MostDetailedMip; i < description.MostDetailedMip + description.MipLevels; i++)
					_subresources[i] = resource.GetSubresource(0, i);
			}

			public override DataIndex GetDataIndex(int arrayIndex, int mipSlice, int x, int y, int z, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _subresources[mipSlice].Data,
					Offset = _subresources[mipSlice].CalculateByteOffset(x, y)
				};
			}
		}
	}
}