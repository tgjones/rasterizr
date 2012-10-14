namespace Rasterizr.Core.Diagnostics
{
	public enum OperationType
	{
		CreateDevice,
		CreateSwapChain,
		DeviceClearDepthBuffer,
		DeviceClearRenderTarget,
		DeviceDraw,
		DeviceDrawIndexed,
		InputAssemblerSetInputLayout,
		InputAssemblerSetPrimitiveTopology,
		InputAssemblerSetVertices,
		InputAssemblerSetIndices,
		VertexShaderSetShader,
		PixelShaderSetShader,
		SwapChainPresent
	}
}