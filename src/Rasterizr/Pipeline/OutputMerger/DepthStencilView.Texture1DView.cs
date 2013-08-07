using Rasterizr.Math;
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

            public override float GetData(int arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresource.GetData(x).R;
            }

            public override void SetData(int arrayIndex, int x, int y, int sampleIndex, float value)
            {
                var color = new Color4F(value, 0, 0, 0);
                _subresource.SetData(x, ref color);
            }

            public override void Clear(float depth)
            {
                var color = new Color4F(depth, 0, 0, 0);
                _subresource.Clear(ref color);
            }
		}
	}
}