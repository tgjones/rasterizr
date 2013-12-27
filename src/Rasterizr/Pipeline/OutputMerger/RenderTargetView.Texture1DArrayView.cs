using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private class Texture1DArrayView : InnerResourceView
		{
			private readonly Texture1D.Texture1DSubresource[] _subresources;

			public Texture1DArrayView(Texture1D resource, RenderTargetViewDescription.Texture1DArrayResource description)
			{
				_subresources = new Texture1D.Texture1DSubresource[description.ArraySize];
				for (int i = description.FirstArraySlice; i < description.FirstArraySlice + description.ArraySize; i++)
					_subresources[i] = resource.GetSubresource(i, description.MipSlice);
			}

            public override Number4 GetData(uint arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresources[arrayIndex].GetData(x);
            }

            public override void SetData(uint arrayIndex, int x, int y, int sampleIndex, ref Number4 value)
            {
                _subresources[arrayIndex].SetData(x, ref value);
            }

            public override void Clear(ref Number4 color)
			{
			    foreach (var subresource in _subresources)
			        subresource.Clear(ref color);
			}
		}
	}
}