namespace Rasterizr.Diagnostics
{
	public enum OperationType
	{
		DeviceCreate,
		SwapChainCreate,
		DepthStencilViewCreate,
		DeviceContextClearDepthStencilView,
		DeviceContextClearRenderTargetView,
		DeviceContextDraw,
		DeviceContextDrawIndexed,
		DeviceContextDrawInstanced,
		InputAssemblerStageSetInputLayout,
		InputAssemblerStageSetPrimitiveTopology,
		InputAssemblerStageSetVertexBuffers,
		InputAssemblerStageSetIndexBuffer,
		InputLayoutCreate,
		VertexShaderCreate,
		VertexShaderStageSetShader,
		PixelShaderCreate,
		PixelShaderStageSetShader,
		SwapChainPresent
	}
}