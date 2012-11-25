using System;

namespace Rasterizr.Resources
{
	[Flags]
	public enum BindFlags
	{
		None = 0,
		VertexBuffer = 1,
		IndexBuffer = 2,
		ConstantBuffer = 4,
		ShaderResource = 8,
		StreamOutput = 16,
		RenderTarget = 32,
		DepthStencil = 64,
		UnorderedAccess = 128
	}
}