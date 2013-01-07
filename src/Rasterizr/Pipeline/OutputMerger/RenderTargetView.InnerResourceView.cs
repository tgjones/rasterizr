using System;
using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private abstract class InnerResourceView
		{
			public abstract DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex);
			public abstract void Clear(Format format, ref Color4F color);

			public static InnerResourceView Create(Resource resource, RenderTargetViewDescription description)
			{
				switch (description.Dimension)
				{
					case RenderTargetViewDimension.Buffer:
						return new BufferView((Resources.Buffer) resource, description.Buffer);
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