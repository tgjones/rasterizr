using System;
using Rasterizr.Math;

namespace Rasterizr.Resources
{
	public class Texture3D : TextureBase
	{
		internal class Texture3DSubresource : TextureSubresource
		{
			public int Width { get; private set; }
			public int Height { get; private set; }
			public int Depth { get; private set; }

			public Texture3DSubresource(int width, int height, int depth)
				: base(width * height * depth)
			{
				Width = width;
				Height = height;
				Depth = depth;
			}

			public Color4F GetData(int x, int y, int z)
			{
				return Data[((z * Width * Height) + (y * Width) + x)];
			}

            public void SetData(int x, int y, int z, ref Color4F value)
            {
                Data[((z * Width * Height) + (y * Width) + x)] = value;
            }
		}

		private readonly Texture3DDescription _description;
		private readonly Texture3DSubresource[] _subresources;

		public Texture3DDescription Description
		{
			get { return _description; }
		}

		internal override ResourceType ResourceType
		{
			get { return ResourceType.Texture3D; }
		}

		internal Texture3D(Device device, Texture3DDescription description)
			: base(device)
		{
			_description = description;

			int mipMapCount = MipMapUtility.CalculateMipMapCount(description.MipLevels,
				description.Width, description.Height, description.Depth);
			_subresources = MipMapUtility.CreateMipMaps(mipMapCount,
				description.Width, description.Height, description.Depth);
		}

        public override Color4F[] GetData(int subresource)
        {
            return _subresources[subresource].Data;
        }

		public override void SetData(int subresource, Color4F[] data)
		{
			Array.Copy(data, _subresources[subresource].Data, data.Length);
		}

		internal Texture3DSubresource GetSubresource(int mipSlice)
		{
			return _subresources[mipSlice];
		}

		internal void GetDimensions(int mipLevel, out int width, out int height, out int depth, out int numberOfLevels)
		{
			GetDimensions(mipLevel, out width, out height, out depth);
			numberOfLevels = _subresources.Length;
		}

		internal void GetDimensions(int mipLevel, out int width, out int height, out int depth)
		{
			var subresource = _subresources[mipLevel];
			width = subresource.Width;
			height = subresource.Height;
			depth = subresource.Depth;
		}

		internal void GetDimensions(out int width, out int height, out int depth)
		{
			GetDimensions(0, out width, out height, out depth);
		}
	}
}