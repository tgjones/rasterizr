namespace Rasterizr.Diagnostics
{
	public enum OperationType
	{
        // Device operations.
		DeviceCreateBlendState,
		DeviceCreateBuffer,
		DeviceCreateDepthStencilState,
		DeviceCreateDepthStencilView,
		DeviceCreateGeometryShader,
        DeviceCreateInputLayout,
		DeviceCreatePixelShader,
        DeviceCreateRasterizerState,
        DeviceCreateRenderTargetView,
        DeviceCreateSamplerState,
        DeviceCreateShaderResourceView,
        DeviceCreateSwapChain,
        DeviceCreateTexture1D,
        DeviceCreateTexture2D,
        DeviceCreateTexture3D,
		DeviceCreateVertexShader,

        // Device context operations.
		DeviceContextClearDepthStencilView,
		DeviceContextClearRenderTargetView,
		DeviceContextDraw,
		DeviceContextDrawIndexed,
		DeviceContextDrawInstanced,
        DeviceContextGenerateMips,
        DeviceContextPresent,
        DeviceContextSetBufferData,
        DeviceContextSetTextureData,

        // Input assembler stage operations.
        InputAssemblerStageSetInputLayout,
        InputAssemblerStageSetPrimitiveTopology,
        InputAssemblerStageSetVertexBuffers,
        InputAssemblerStageSetIndexBuffer,

        // Vertex shader stage operations.
        VertexShaderStageSetShader,
        VertexShaderStageSetConstantBuffers,
        VertexShaderStageSetSamplers,
        VertexShaderStageSetShaderResources,

        // Geometry shader stage operations.
		GeometryShaderStageSetShader,
        GeometryShaderStageSetConstantBuffers,
        GeometryShaderStageSetSamplers,
        GeometryShaderStageSetShaderResources,

        // Rasterizer stage operations.
        RasterizerStageSetState,
        RasterizerStageSetViewports,

        // Pixel shader stage operations.
        PixelShaderStageSetShader,
        PixelShaderStageSetConstantBuffers,
        PixelShaderStageSetSamplers,
        PixelShaderStageSetShaderResources,

        // Output merger stage operations.
        OutputMergerStageSetDepthStencilState,
        OutputMergerStageSetDepthStencilReference,
        OutputMergerStageSetBlendState,
        OutputMergerStageSetBlendFactor,
        OutputMergerStageSetBlendSampleMask,
		OutputMergerStageSetTargets
	}
}