using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private class Texture3DView : InnerResourceView
		{
			private readonly Texture3D.Texture3DSubresource _subresource;
			private readonly int _firstDepthSlice;
			private readonly int _depthSliceCount;

			public Texture3DView(Texture3D resource, RenderTargetViewDescription.Texture3DResource description)
			{
				_subresource = resource.GetSubresource(description.MipSlice);
				_firstDepthSlice = description.FirstDepthSlice;
				_depthSliceCount = description.DepthSliceCount;
			}

			public override DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _subresource.Data,
					Offset = _subresource.CalculateByteOffset(x, y, _firstDepthSlice + arrayIndex)
				};
			}

			public override void Clear(Format format, ref Color4F color)
			{
				// TODO
				//for (int i = _firstDepthSlice; i < _firstDepthSlice + _depthSliceCount; i++)
				//	FormatHelper.Clear(subresource, format, ref color);
			}
		}
	}
}