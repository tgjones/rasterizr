using Rasterizr.Resources;
using SlimShader;

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

            public override Number4 GetData(uint arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresource.GetData(x, y);
            }

            public override void SetData(uint arrayIndex, int x, int y, int sampleIndex, ref Number4 value)
            {
                _subresource.SetData(x, y, ref value);
            }

            public override void Clear(ref Number4 color)
			{
			    _subresource.Clear(ref color);
			}
		}
	}
}