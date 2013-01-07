using Rasterizr.Math;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView : ResourceView
	{
		private readonly RenderTargetViewDescription _description;
		private readonly InnerResourceView _innerView;
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
			_description = description.GetValueOrDefault(RenderTargetViewDescription.CreateDefault(resource));
			_innerView = InnerResourceView.Create(resource, _description);
			_actualFormat = ResourceViewUtility.GetActualFormat(_description.Format, resource);
		}

		internal Color4F GetColor(int arrayIndex, int x, int y, int sampleIndex)
		{
			return FormatHelper.Convert(_actualFormat, _innerView.GetDataIndex(arrayIndex, x, y, sampleIndex));
		}

		internal void SetColor(int arrayIndex, int x, int y, int sampleIndex, Color4F color)
		{
			FormatHelper.Convert(_actualFormat, color, _innerView.GetDataIndex(arrayIndex, x, y, sampleIndex));
		}

		internal void Clear(ref Color4F color)
		{
			_innerView.Clear(_actualFormat, ref color);
		}
	}
}