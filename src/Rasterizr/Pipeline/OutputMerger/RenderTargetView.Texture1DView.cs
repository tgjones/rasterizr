using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private class Texture1DView : InnerResourceView
		{
			private readonly Texture1D.Texture1DSubresource _subresource;

			public Texture1DView(Texture1D resource, RenderTargetViewDescription.Texture1DResource description)
			{
				_subresource = resource.GetSubresource(0, description.MipSlice);
			}

            public override Color4F GetData(int arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresource.GetData(x);
            }

            public override void SetData(int arrayIndex, int x, int y, int sampleIndex, ref Color4F value)
            {
                _subresource.SetData(x, ref value);
            }

			public override void Clear(ref Color4F color)
			{
			    _subresource.Clear(ref color);
			}
		}
	}
}