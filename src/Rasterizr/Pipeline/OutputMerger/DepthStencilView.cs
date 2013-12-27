using System;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView : ResourceView
	{
		private readonly DepthStencilViewDescription _description;
		private readonly InnerResourceView _innerView;

		public DepthStencilViewDescription Description
		{
			get { return _description; }
		}

		internal DepthStencilView(Device device, Resource resource, DepthStencilViewDescription? description)
			: base(device, resource)
		{
			switch (resource.ResourceType)
			{
				case ResourceType.Buffer:
					throw new ArgumentException("Invalid resource type for depth stencil view: " + resource.ResourceType);
			}

			_description = description.GetValueOrDefault(DepthStencilViewDescription.CreateDefault(resource));
			_innerView = InnerResourceView.Create(resource, _description);
		}

		internal float GetDepth(uint arrayIndex, int x, int y, int sampleIndex)
		{
		    return _innerView.GetData(arrayIndex, x, y, sampleIndex);
		}

		internal void SetDepth(uint arrayIndex, int x, int y, int sampleIndex, float depth)
		{
		    _innerView.SetData(arrayIndex, x, y, sampleIndex, depth);
		}

		internal void Clear(DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
		    if (clearFlags.HasFlag(DepthStencilClearFlags.Depth))
		        _innerView.Clear(depth);
		}
	}
}