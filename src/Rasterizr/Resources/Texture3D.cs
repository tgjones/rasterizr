using System;
using System.Linq;
using Rasterizr.Util;
using SlimShader;

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

            public Number4 GetData(int x, int y, int z)
			{
				return Data[((z * Width * Height) + (y * Width) + x)];
			}

            public void SetData(int x, int y, int z, ref Number4 value)
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

        internal override int Size
        {
            get { return _subresources.Sum(x => x.Size); }
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

        public override Color4[] GetData(int subresource)
        {
            var data = _subresources[subresource].Data;
            var result = new Color4[data.Length];
            Utilities.Copy(data, result);
            return result;
        }

        internal override void SetData(int subresource, Color4[] data)
		{
            Utilities.Copy(data, _subresources[subresource].Data);
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

        internal override void GenerateMips()
        {
            throw new NotImplementedException();
        }
	}
}