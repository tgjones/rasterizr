using System;
using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class RenderTargetView : ResourceView
	{
		private readonly RenderTargetViewDescription _description;
		private readonly Format _actualFormat;

		public RenderTargetViewDescription Description
		{
			get { return _description; }
		}

		internal Format ActualFormat
		{
			get { return _actualFormat; }
		}

		internal RenderTargetView(Device device, Resource resource, RenderTargetViewDescription? description)
			: base(device, resource)
		{
			if (description == null)
				description = new RenderTargetViewDescription
				{
					Format = Format.Unknown,
					Dimension = RenderTargetViewDimension.Unknown
				};

			if (description.Value.Format == Format.Unknown && description.Value.Dimension == RenderTargetViewDimension.Buffer)
				throw new ArgumentException();

			_description = description.Value;
			_actualFormat = ResourceViewUtility.GetActualFormat(_description.Format, resource);
		}

		internal Color4F GetColor(int x, int y, int sampleIndex)
		{
			return FormatHelper.Convert(_actualFormat, Resource.Data, Resource.CalculateByteOffset(x, y, 0));
		}

		internal void SetColor(int x, int y, int sampleIndex, Color4F color)
		{
			FormatHelper.Convert(_actualFormat, color, Resource.Data, Resource.CalculateByteOffset(x, y, 0));
		}

		internal void Clear(Color4F color)
		{
			FormatHelper.Fill(Resource, _actualFormat, ref color);
		}
	}
}