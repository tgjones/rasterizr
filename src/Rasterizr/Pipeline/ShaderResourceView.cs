using Rasterizr.Resources;
using SlimShader.VirtualMachine.Resources;

namespace Rasterizr.Pipeline
{
	public partial class ShaderResourceView : ResourceView
	{
		private readonly ShaderResourceViewDescription _description;
		private readonly InnerResourceView _innerView;

		internal ITexture InnerView
		{
			get { return _innerView; }
		}

		internal ShaderResourceView(Device device, Resource resource, ShaderResourceViewDescription? description)
			: base(device, resource)
		{
			_description = description.GetValueOrDefault(ShaderResourceViewDescription.CreateDefault(resource));
			_innerView = InnerResourceView.Create(resource, _description);
		}
	}
}