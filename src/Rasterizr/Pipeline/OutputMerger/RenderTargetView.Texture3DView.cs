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

            public override Color4F GetData(int arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresource.GetData(x, y, _firstDepthSlice + arrayIndex);
            }

            public override void SetData(int arrayIndex, int x, int y, int sampleIndex, ref Color4F value)
            {
                _subresource.SetData(x, y, _firstDepthSlice + arrayIndex, ref value);
            }

			public override void Clear(ref Color4F color)
			{
				// TODO
				//for (int i = _firstDepthSlice; i < _firstDepthSlice + _depthSliceCount; i++)
				//	FormatHelper.Clear(subresource, format, ref color);
			}
		}
	}
}