using System;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class DepthStencilView : ResourceView
	{
		public float this[int x, int y, int sampleIndex]
		{
			get
			{
				// TODO: Get rid of all these conversions.
				var texture = (Texture2D)Resource;
				float result;
				texture.GetData(out result, (y * texture.Description.Width) + x * FormatHelper.SizeOfInBytes(texture.Description.Format), sizeof(float));
				return result;
			}
			set
			{
				var texture = (Texture2D)Resource;
				texture.SetData(ref value, (y * texture.Description.Width) + x * FormatHelper.SizeOfInBytes(texture.Description.Format));
			}
		}

		public DepthStencilView(Device device, Texture2D resource)
			: base(device, resource)
		{
			
		}

		internal void Clear(DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			var texture = (Texture2D)Resource;

			if (clearFlags.HasFlag(DepthStencilClearFlags.Depth))
			{
				switch (texture.Description.Format)
				{
					case Format.D32_Float_S8X24_UInt :
						for (int i = 0; i < texture.Description.Width * texture.Description.Height; i++)
							texture.SetData(ref depth, i * FormatHelper.SizeOfInBytes(texture.Description.Format));
						break;
					default :
						throw new ArgumentException();
				}
			}
		}
	}
}