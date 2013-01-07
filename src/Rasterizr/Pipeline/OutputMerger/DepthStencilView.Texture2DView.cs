using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView
	{
		private class Texture2DView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource _subresource;

			public Texture2DView(Texture2D resource, DepthStencilViewDescription.Texture2DResource description)
			{
				_subresource = resource.GetSubresource(0, description.MipSlice);
			}

			public override DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _subresource.Data,
					Offset = _subresource.CalculateByteOffset(x, y)
				};
			}

			public override void Clear(float depth)
			{
				_subresource.Clear(ref depth);
			}
		}
	}
}