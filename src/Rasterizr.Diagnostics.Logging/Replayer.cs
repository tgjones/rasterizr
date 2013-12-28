using System;
using System.Linq;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.GeometryShader;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;
using Rasterizr.Util;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Diagnostics.Logging
{
	public class Replayer
	{
		private readonly TracefileFrame _frame;
		private readonly TracefileEvent _lastEvent;
	    private readonly ISwapChainPresenter _swapChainPresenter;

        private readonly Device _device;
		private readonly DeviceContext _deviceContext;

        private readonly TracefileBuilder _logger;

	    public Device Device
	    {
	        get { return _device; }
	    }

        public TracefileBuilder Logger
		{
			get { return _logger; }
		}

		public Replayer(TracefileFrame frame, TracefileEvent lastEvent, 
			ISwapChainPresenter swapChainPresenter, 
            int? renderTargetViewID = null, int? renderTargetArrayIndex = null,
            int? pixelX = null, int? pixelY = null)
		{
			_frame = frame;
			_lastEvent = lastEvent;
		    _swapChainPresenter = swapChainPresenter;
		    _device = new Device();
		    _deviceContext = _device.ImmediateContext;
            _logger = new TracefileBuilder(_device, 
                renderTargetViewID, renderTargetArrayIndex, 
                pixelX, pixelY);
		}

		public void Replay()
		{
			foreach (var @event in _frame.Events)
			{
				var args = @event.Arguments;
				switch (@event.OperationType)
				{
				    // Device operations.
				    case OperationType.DeviceCreateBlendState:
				        _device.CreateBlendState(args.Get<BlendStateDescription>(0));
				        break;
				    case OperationType.DeviceCreateBuffer:
				        _device.CreateBuffer(args.Get<BufferDescription>(0), args.Get<byte[]>(1));
				        break;
				    case OperationType.DeviceCreateDepthStencilState:
				        _device.CreateDepthStencilState(args.Get<DepthStencilStateDescription>(0));
				        break;
				    case OperationType.DeviceCreateDepthStencilView:
				        _device.CreateDepthStencilView(_device.GetDeviceChild<Resource>(args.Get<int>(0)),
				            args.Get<DepthStencilViewDescription?>(1));
				        break;
				    case OperationType.DeviceCreateGeometryShader:
				        _device.CreateGeometryShader(args.Get<byte[]>(0));
				        break;
				    case OperationType.DeviceCreateInputLayout:
				        _device.CreateInputLayout(args.Get<InputElement[]>(0), args.Get<byte[]>(1));
				        break;
				    case OperationType.DeviceCreatePixelShader:
				        _device.CreatePixelShader(args.Get<byte[]>(0));
				        break;
				    case OperationType.DeviceCreateRasterizerState:
				        _device.CreateRasterizerState(args.Get<RasterizerStateDescription>(0));
				        break;
				    case OperationType.DeviceCreateRenderTargetView:
				        _device.CreateRenderTargetView(args.Get<Texture2D>(_device, 0),
				            args.Get<RenderTargetViewDescription?>(1));
				        break;
				    case OperationType.DeviceCreateSamplerState:
				        _device.CreateSamplerState(args.Get<SamplerStateDescription>(0));
				        break;
				    case OperationType.DeviceCreateShaderResourceView:
				        _device.CreateShaderResourceView(args.Get<Resource>(_device, 0),
				            args.Get<ShaderResourceViewDescription?>(1));
				        break;
				    case OperationType.DeviceCreateSwapChain:
				        _device.CreateSwapChain(
                            args.Get<SwapChainDescription>(0), 
                            _swapChainPresenter);
				        break;
				    case OperationType.DeviceCreateTexture1D:
				        _device.CreateTexture1D(args.Get<Texture1DDescription>(0));
				        break;
				    case OperationType.DeviceCreateTexture2D:
				        _device.CreateTexture2D(args.Get<Texture2DDescription>(0));
				        break;
				    case OperationType.DeviceCreateTexture3D:
				        _device.CreateTexture3D(args.Get<Texture3DDescription>(0));
				        break;
				    case OperationType.DeviceCreateVertexShader:
				        _device.CreateVertexShader(args.Get<byte[]>(0));
				        break;

				    // Device context operations.
				    case OperationType.DeviceContextClearDepthStencilView:
				        _deviceContext.ClearDepthStencilView(
				            args.Get<DepthStencilView>(_device, 0),
				            args.Get<DepthStencilClearFlags>(1),
				            args.Get<float>(2),
				            args.Get<byte>(3));
				        break;
				    case OperationType.DeviceContextClearRenderTargetView:
				        _deviceContext.ClearRenderTargetView(
				            _device.GetDeviceChild<RenderTargetView>(args.Get<int>(0)),
				            args.Get<Color4>(1));
				        break;
				    case OperationType.DeviceContextDraw:
				        _deviceContext.Draw(args.Get<int>(0), args.Get<int>(1));
				        break;
				    case OperationType.DeviceContextDrawIndexed:
				        _deviceContext.DrawIndexed(
				            args.Get<int>(0), args.Get<int>(1),
				            args.Get<int>(2));
				        break;
				    case OperationType.DeviceContextDrawInstanced:
				        _deviceContext.DrawInstanced(
				            args.Get<int>(0), args.Get<int>(1),
				            args.Get<int>(2), args.Get<int>(3));
				        break;
				    case OperationType.DeviceContextGenerateMips:
				        _deviceContext.GenerateMips(args.Get<TextureBase>(_device, 0));
				        break;
				    case OperationType.DeviceContextPresent:
				        _deviceContext.Present(args.Get<SwapChain>(_device, 0));
				        break;
				    case OperationType.DeviceContextSetBufferData:
				        _deviceContext.SetBufferData(args.Get<Buffer>(_device, 0),
				            args.Get<byte[]>(1), args.Get<int>(2));
				        break;
                    case OperationType.DeviceContextSetTextureData:
                        _deviceContext.SetTextureData(args.Get<TextureBase>(_device, 0),
                            args.Get<int>(1), args.Get<Color4[]>(2));
                        break;

				    // Input assembler stage operations.
				    case OperationType.InputAssemblerStageSetInputLayout:
				        _deviceContext.InputAssembler.InputLayout = args.Get<InputLayout>(_device, 0);
				        break;
				    case OperationType.InputAssemblerStageSetPrimitiveTopology:
				        _deviceContext.InputAssembler.PrimitiveTopology = args.Get<PrimitiveTopology>(0);
				        break;
				    case OperationType.InputAssemblerStageSetVertexBuffers:
				        _deviceContext.InputAssembler.SetVertexBuffers(args.Get<int>(0),
				            args.Get<SerializedVertexBufferBinding[]>(1)
				                .Select(x => new VertexBufferBinding
				                {
				                    Buffer = _device.GetDeviceChild<Buffer>(x.Buffer),
				                    Offset = x.Offset,
				                    Stride = x.Stride
				                }).ToArray());
				        break;
				    case OperationType.InputAssemblerStageSetIndexBuffer:
				        _deviceContext.InputAssembler.SetIndexBuffer(args.Get<Buffer>(_device, 0),
				            args.Get<Format>(1), args.Get<int>(2));
				        break;

				    // Vertex shader stage operations.
				    case OperationType.VertexShaderStageSetShader:
				        _deviceContext.VertexShader.Shader = args.Get<VertexShader>(_device, 0);
				        break;
				    case OperationType.VertexShaderStageSetConstantBuffers:
				        _deviceContext.VertexShader.SetConstantBuffers(args.Get<int>(0),
				            args.GetArray<Buffer>(_device, 1));
				        break;
				    case OperationType.VertexShaderStageSetSamplers:
				        _deviceContext.VertexShader.SetSamplers(args.Get<int>(0),
				            args.GetArray<SamplerState>(_device, 1));
				        break;
				    case OperationType.VertexShaderStageSetShaderResources:
				        _deviceContext.VertexShader.SetShaderResources(args.Get<int>(0),
				            args.GetArray<ShaderResourceView>(_device, 1));
				        break;

				    // Geometry shader stage operations.
				    case OperationType.GeometryShaderStageSetShader:
				        _deviceContext.GeometryShader.Shader = args.Get<GeometryShader>(_device, 0);
				        break;
				    case OperationType.GeometryShaderStageSetConstantBuffers:
				        _deviceContext.GeometryShader.SetConstantBuffers(args.Get<int>(0),
				            args.GetArray<Buffer>(_device, 1));
				        break;
				    case OperationType.GeometryShaderStageSetSamplers:
				        _deviceContext.GeometryShader.SetSamplers(args.Get<int>(0),
				            args.GetArray<SamplerState>(_device, 1));
				        break;
				    case OperationType.GeometryShaderStageSetShaderResources:
				        _deviceContext.GeometryShader.SetShaderResources(args.Get<int>(0),
				            args.GetArray<ShaderResourceView>(_device, 1));
				        break;

				    // Rasterizer stage operations.
				    case OperationType.RasterizerStageSetState:
				        _deviceContext.Rasterizer.State = args.Get<RasterizerState>(_device, 0);
				        break;
				    case OperationType.RasterizerStageSetViewports:
				        _deviceContext.Rasterizer.SetViewports(args.Get<Viewport[]>(0));
				        break;

				    // Pixel shader stage operations.
				    case OperationType.PixelShaderStageSetShader:
				        _deviceContext.PixelShader.Shader = args.Get<PixelShader>(_device, 0);
				        break;
				    case OperationType.PixelShaderStageSetConstantBuffers:
				        _deviceContext.PixelShader.SetConstantBuffers(args.Get<int>(0),
				            args.GetArray<Buffer>(_device, 1));
				        break;
				    case OperationType.PixelShaderStageSetSamplers:
				        _deviceContext.PixelShader.SetSamplers(args.Get<int>(0),
				            args.GetArray<SamplerState>(_device, 1));
				        break;
				    case OperationType.PixelShaderStageSetShaderResources:
				        _deviceContext.PixelShader.SetShaderResources(args.Get<int>(0),
				            args.GetArray<ShaderResourceView>(_device, 1));
				        break;

				    // Output merger stage operations.
				    case OperationType.OutputMergerStageSetDepthStencilState:
				        _deviceContext.OutputMerger.DepthStencilState = args.Get<DepthStencilState>(_device, 0);
				        break;
				    case OperationType.OutputMergerStageSetDepthStencilReference:
				        _deviceContext.OutputMerger.DepthStencilReference = args.Get<int>(0);
				        break;
				    case OperationType.OutputMergerStageSetBlendState:
				        _deviceContext.OutputMerger.BlendState = args.Get<BlendState>(_device, 0);
				        break;
				    case OperationType.OutputMergerStageSetBlendFactor:
				        _deviceContext.OutputMerger.BlendFactor = args.Get<Color4>(0);
				        break;
				    case OperationType.OutputMergerStageSetBlendSampleMask:
				        _deviceContext.OutputMerger.BlendSampleMask = args.Get<int>(0);
				        break;
				    case OperationType.OutputMergerStageSetTargets:
				        _deviceContext.OutputMerger.SetTargets(
				            args.Get<DepthStencilView>(_device, 0),
				            args.GetArray<RenderTargetView>(_device, 1));
				        break;

				    default:
				        throw new ArgumentOutOfRangeException();
				}

			    if (@event == _lastEvent)
					break;
			}
		}
	}
}