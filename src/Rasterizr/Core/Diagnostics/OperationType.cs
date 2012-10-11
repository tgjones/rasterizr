namespace Rasterizr.Core.Diagnostics
{
	public enum OperationType
	{
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