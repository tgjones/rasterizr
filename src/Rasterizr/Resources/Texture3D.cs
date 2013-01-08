using System;

namespace Rasterizr.Resources
{
	public class Texture3D : TextureBase
	{
		internal class Texture3DSubresource : TextureSubresource
		{
			public int Width { get; private set; }
			public int Height { get; private set; }
			public int Depth { get; private set; }

			public Texture3DSubresource(int elementSize, int width, int height, int depth)
				: base(width * height * depth * elementSize, elementSize)
			{
				Width = width;
				Height = height;
				Depth = depth;
			}

			public int CalculateByteOffset(int x, int y, int z)
			{
				return ((z * Width * Height) + (y * Width) + x) * ElementSize;
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
			: base(device, description.Format)
		{
			_description = description;

			int mipMapCount = MipMapUtility.CalculateMipMapCount(description.MipLevels,
				description.Width, description.Height, description.Depth);
			_subresources = MipMapUtility.CreateMipMaps(mipMapCount, FormatHelper.SizeOfInBytes(description.Format),
				description.Width, description.Height, description.Depth);
		}

		internal override MappedSubresource Map(int subresource)
		{
			return new MappedSubresource
			{
				Data = _subresources[subresource].Data
			};
		}

		internal override void UpdateSubresource(int subresource, byte[] data)
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