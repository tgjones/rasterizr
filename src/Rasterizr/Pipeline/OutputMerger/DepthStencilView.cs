using System;
using Rasterizr.Resources;
using Rasterizr.Util;

namespace Rasterizr.Pipeline.OutputMerger
{
	public partial class DepthStencilView : ResourceView
	{
		private readonly DepthStencilViewDescription _description;
		private readonly InnerResourceView _innerView;
		private readonly Format _actualFormat;

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
			_actualFormat = ResourceViewUtility.GetActualFormat(_description.Format, resource);
		}

		internal float GetDepth(int arrayIndex, int x, int y, int sampleIndex)
		{
			var dataIndex = _innerView.GetDataIndex(arrayIndex, x, y, sampleIndex);

			float result;
			Utilities.FromByteArray(out result, dataIndex.Data, dataIndex.Offset, sizeof(float));
			return result;
		}

		internal void SetDepth(int arrayIndex, int x, int y, int sampleIndex, float depth)
		{
			var dataIndex = _innerView.GetDataIndex(arrayIndex, x, y, sampleIndex);
			Utilities.ToByteArray(ref depth, dataIndex.Data, dataIndex.Offset);
		}

		internal void Clear(DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			if (clearFlags.HasFlag(DepthStencilClearFlags.Depth))
			{
				switch (_actualFormat)
				{
					case Format.D32_Float_S8X24_UInt :
						_innerView.Clear(depth);
						break;
					default :
						throw new ArgumentException();
				}
			}
		}
	}
}