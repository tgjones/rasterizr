using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView
	{
		private class Texture2DArrayView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource[] _subresources;

			public Texture2DArrayView(Texture2D resource, DepthStencilViewDescription.Texture2DArrayResource description)
			{
				_subresources = new Texture2D.Texture2DSubresource[description.ArraySize];
				for (int i = description.FirstArraySlice; i < description.FirstArraySlice + description.ArraySize; i++)
					_subresources[i] = resource.GetSubresource(i, description.MipSlice);
			}

            public override float GetData(int arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresources[arrayIndex].GetData(x, y).R;
            }

            public override void SetData(int arrayIndex, int x, int y, int sampleIndex, float value)
            {
                var color = new Color4F(value, 0, 0, 0);
                _subresources[arrayIndex].SetData(x, y, ref color);
            }

            public override void Clear(float depth)
            {
                var color = new Color4F(depth, 0, 0, 0);
                foreach (var subresource in _subresources)
                    subresource.Clear(ref color);
            }
		}
	}
}