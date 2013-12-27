using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView
	{
		private class Texture1DArrayView : InnerResourceView
		{
			private readonly Texture1D.Texture1DSubresource[] _subresources;

			public Texture1DArrayView(Texture1D resource, DepthStencilViewDescription.Texture1DArrayResource description)
			{
				_subresources = new Texture1D.Texture1DSubresource[description.ArraySize];
				for (int i = description.FirstArraySlice; i < description.FirstArraySlice + description.ArraySize; i++)
					_subresources[i] = resource.GetSubresource(i, description.MipSlice);
			}

            public override float GetData(uint arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresources[arrayIndex].GetData(x).R;
            }

            public override void SetData(uint arrayIndex, int x, int y, int sampleIndex, float value)
            {
                var color = new Number4(value, 0, 0, 0);
                _subresources[arrayIndex].SetData(x, ref color);
            }

            public override void Clear(float depth)
            {
                var color = new Number4(depth, 0, 0, 0);
                foreach (var subresource in _subresources)
                    subresource.Clear(ref color);
            }
		}
	}
}