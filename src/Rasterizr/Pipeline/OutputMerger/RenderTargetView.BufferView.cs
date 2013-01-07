using Rasterizr.Math;
using Rasterizr.Resources;
using Rasterizr.Util;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView
	{
		private class BufferView : InnerResourceView
		{
			private readonly RenderTargetViewDescription.BufferResource _description;
			private readonly byte[] _data;

			public BufferView(Buffer resource, RenderTargetViewDescription.BufferResource description)
			{
				_data = resource.Data;
				_description = description;
			}

			public override DataIndex GetDataIndex(int arrayIndex, int x, int y, int sampleIndex)
			{
				return new DataIndex
				{
					Data = _data,
					Offset = _description.ElementOffset + (x * _description.ElementWidth)
				};
			}

			public override void Clear(Format format, ref Color4F color)
			{
				Utilities.Clear(_data, _description.ElementWidth, ref color);
			}
		}
	}
}