using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private class Texture2DView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource _subresource;

			public Texture2DView(Texture2D resource, RenderTargetViewDescription.Texture2DResource description)
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

			public override void Clear(Format format, ref Color4F color)
			{
				FormatHelper.Clear(_subresource, format, ref color);
			}
		}
	}
}