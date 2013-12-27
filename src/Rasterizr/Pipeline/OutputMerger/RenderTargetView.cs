using Rasterizr.Resources;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class RenderTargetView : ResourceView
	{
		private readonly RenderTargetViewDescription _description;
		private readonly InnerResourceView _innerView;

		public RenderTargetViewDescription Description
		{
			get { return _description; }
		}

		internal RenderTargetView(Device device, Resource resource, RenderTargetViewDescription? description)
			: base(device, resource)
		{
			_description = description.GetValueOrDefault(RenderTargetViewDescription.CreateDefault(resource));
			_innerView = InnerResourceView.Create(resource, _description);
		}

        internal Number4 GetColor(uint arrayIndex, int x, int y, int sampleIndex)
		{
			return _innerView.GetData(arrayIndex, x, y, sampleIndex);
		}

        internal void SetColor(uint arrayIndex, int x, int y, int sampleIndex, ref Number4 color)
		{
		    _innerView.SetData(arrayIndex, x, y, sampleIndex, ref color);
		}

        internal void Clear(ref Number4 color)
		{
			_innerView.Clear(ref color);
		}
	}
}