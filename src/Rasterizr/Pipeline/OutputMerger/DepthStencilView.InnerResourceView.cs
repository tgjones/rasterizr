using System;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView
	{
		private abstract class InnerResourceView
		{
			public abstract float GetData(uint arrayIndex, int x, int y, int sampleIndex);
		    public abstract void SetData(uint arrayIndex, int x, int y, int sampleIndex, float value);
			public abstract void Clear(float depth);

			public static InnerResourceView Create(Resource resource, DepthStencilViewDescription description)
			{
				switch (description.Dimension)
				{
					case DepthStencilViewDimension.Texture1D:
						return new Texture1DView((Texture1D) resource, description.Texture1D);
					case DepthStencilViewDimension.Texture1DArray:
						return new Texture1DArrayView((Texture1D) resource, description.Texture1DArray);
					case DepthStencilViewDimension.Texture2D:
						return new Texture2DView((Texture2D) resource, description.Texture2D);
					case DepthStencilViewDimension.Texture2DArray:
						return new Texture2DArrayView((Texture2D) resource, description.Texture2DArray);
					case DepthStencilViewDimension.Texture2DMultisampled:
						throw new NotImplementedException();
					case DepthStencilViewDimension.Texture2DMultisampledArray:
						throw new NotImplementedException();
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}