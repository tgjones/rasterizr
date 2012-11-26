using System;
using Rasterizr.Math;
using Rasterizr.Util;

namespace Rasterizr.Resources
{
	public class RenderTargetView : ResourceView
	{
		private readonly RenderTargetViewDescription _description;
		private readonly Color4[] _colors;

		public RenderTargetViewDescription Description
		{
			get { return _description; }
		}

		public Color4F this[int x, int y, int sampleIndex]
		{
			get
			{
				// TODO: Get rid of all these conversions.
				var texture = (Texture2D) Resource;
				return _colors[(y * texture.Description.Width) + x].ToColor4F();
			}
			set
			{
				var texture = (Texture2D) Resource;
				_colors[(y * texture.Description.Width) + x] = value.ToColor4();

				// TODO: only do this at the end of the frame.
				texture.SetData(_colors);
			}
		}

		public RenderTargetView(Device device, Texture2D resource, RenderTargetViewDescription description)
			: base(device, resource)
		{
			_description = description;
			_colors = new Color4[resource.Description.Width * resource.Description.Height];
		}

		public RenderTargetView(Device device, Texture2D resource)
			: this(device, resource, new RenderTargetViewDescription
			{
				Format = Format.Unknown,
				Dimension = RenderTargetViewDimension.Unknown
			})
		{
			
		}

		public unsafe void Clear(Color4F color)
		{
			// TODO: Use RenderTargetView description to access resource.
			var typedColor = color.ToColor4();
			var invertedColor = new Color4(typedColor.B, typedColor.G, typedColor.R, typedColor.A);
			var texture = (Texture2D) Resource;

			// Fill first line.
			int width = texture.Description.Width;
			for (int x = 0; x < width; x++)
				_colors[x] = invertedColor;

			// Copy first line.
			int height = texture.Description.Height;
			int sizeToCopy = Utilities.SizeOf<Color4>() * width;
			void* src = Interop.Fixed(_colors);
			for (int y = 1; y < height; y++)
			{
				var dest = (void*) ((IntPtr) src + y * sizeToCopy);
				Interop.memcpy(dest, src, sizeToCopy);
			}

			texture.SetData(_colors);
		}
	}
}