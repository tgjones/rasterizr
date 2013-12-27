using Rasterizr.Resources;
using SlimShader;

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

            public override float GetData(uint arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresource.GetData(x, y).R;
            }

            public override void SetData(uint arrayIndex, int x, int y, int sampleIndex, float value)
            {
                var color = new Number4(value, 0, 0, 0);
                _subresource.SetData(x, y, ref color);
            }

			public override void Clear(float depth)
			{
                var color = new Number4(depth, 0, 0, 0);
				_subresource.Clear(ref color);
			}
		}
	}
}