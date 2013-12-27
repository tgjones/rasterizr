using System;
using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private abstract class InnerResourceView
		{
            public abstract Number4 GetData(uint arrayIndex, int x, int y, int sampleIndex);
            public abstract void SetData(uint arrayIndex, int x, int y, int sampleIndex, ref Number4 value);
            public abstract void Clear(ref Number4 color);

			public static InnerResourceView Create(Resource resource, RenderTargetViewDescription description)
			{
				switch (description.Dimension)
				{
					case RenderTargetViewDimension.Texture1D:
						return new Texture1DView((Texture1D) resource, description.Texture1D);
					case RenderTargetViewDimension.Texture1DArray:
						return new Texture1DArrayView((Texture1D) resource, description.Texture1DArray);
					case RenderTargetViewDimension.Texture2D:
						return new Texture2DView((Texture2D) resource, description.Texture2D);
					case RenderTargetViewDimension.Texture2DArray:
						return new Texture2DArrayView((Texture2D) resource, description.Texture2DArray);
					case RenderTargetViewDimension.Texture2DMultisampled:
						throw new NotImplementedException();
					case RenderTargetViewDimension.Texture2DMultisampledArray:
						throw new NotImplementedException();
					case RenderTargetViewDimension.Texture3D:
						return new Texture3DView((Texture3D) resource, description.Texture3D);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}