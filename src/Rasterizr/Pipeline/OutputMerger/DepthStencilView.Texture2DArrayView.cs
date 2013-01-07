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

			public override DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _subresources[arrayIndex].Data,
					Offset = _subresources[arrayIndex].CalculateByteOffset(x, y)
				};
			}

			public override void Clear(float depth)
			{
				foreach (var subresource in _subresources)
					subresource.Clear(ref depth);
			}
		}
	}
}