using System;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class DepthStencilView : ResourceView
	{
		private readonly DepthStencilViewDescription _description;

		public DepthStencilViewDescription Description
		{
			get { return _description; }
		}

		internal float this[int x, int y, int sampleIndex]
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

		internal DepthStencilView(Device device, Resource resource, DepthStencilViewDescription? description)
			: base(device, resource)
		{
			if (description == null)
				description = new DepthStencilViewDescription
				{
					Format = Format.Unknown,
					Dimension = DepthStencilViewDimension.Unknown
				};
			_description = description.Value;
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