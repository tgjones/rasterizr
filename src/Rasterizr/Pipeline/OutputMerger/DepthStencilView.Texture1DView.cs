using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView
	{
		private class Texture1DView : InnerResourceView
		{
			private readonly Texture1D.Texture1DSubresource _subresource;

			public Texture1DView(Texture1D resource, DepthStencilViewDescription.Texture1DResource description)
			{
				_subresource = resource.GetSubresource(0, description.MipSlice);
			}

			public override DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _subresource.Data,
					Offset = _subresource.CalculateByteOffset(x)
				};
			}

			public override void Clear(float depth)
			{
				_subresource.Clear(ref depth);
			}
		}
	}
}