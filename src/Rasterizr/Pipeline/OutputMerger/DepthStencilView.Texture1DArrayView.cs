using Rasterizr.Resources;

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

			public override DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _subresources[arrayIndex].Data,
					Offset = _subresources[arrayIndex].CalculateByteOffset(x)
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