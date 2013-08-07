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

            public override Color4F GetData(int arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresource.GetData(x, y);
            }

            public override void SetData(int arrayIndex, int x, int y, int sampleIndex, ref Color4F value)
            {
                _subresource.SetData(x, y, ref value);
            }

			public override void Clear(ref Color4F color)
			{
			    _subresource.Clear(ref color);
			}
		}
	}
}