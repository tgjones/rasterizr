using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private class Texture2DArrayView : InnerResourceView
		{
			private readonly Texture2D.Texture2DSubresource[] _subresources;

			public Texture2DArrayView(Texture2D resource, RenderTargetViewDescription.Texture2DArrayResource description)
			{
				_subresources = new Texture2D.Texture2DSubresource[description.ArraySize];
				for (int i = description.FirstArraySlice; i < description.FirstArraySlice + description.ArraySize; i++)
					_subresources[i] = resource.GetSubresource(i, description.MipSlice);
			}

            public override Number4 GetData(uint arrayIndex, int x, int y, int sampleIndex)
            {
                return _subresources[arrayIndex].GetData(x, y);
            }

            public override void SetData(uint arrayIndex, int x, int y, int sampleIndex, ref Number4 value)
            {
                _subresources[arrayIndex].SetData(x, y, ref value);
            }

            public override void Clear(ref Number4 color)
			{
			    foreach (var subresource in _subresources)
			        subresource.Clear(ref color);
			}
		}
	}
}