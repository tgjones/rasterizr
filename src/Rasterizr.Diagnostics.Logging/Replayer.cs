using System;
using System.IO;
using System.Linq;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Diagnostics.Logging
{
	public class Replayer
	{
		private readonly TracefileFrame _frame;
		private readonly TracefileEvent _lastEvent;
		private readonly Func<Device, SwapChainDescription, SwapChain> _createSwapChainCallback;

		private SwapChain _swapChain;
		private Device _device;
		private DeviceContext _deviceContext;

		private readonly TracefileGraphicsLogger _logger;

		public TracefileGraphicsLogger Logger
		{
			get { return _logger; }
		}

		public Replayer(TracefileFrame frame, TracefileEvent lastEvent, 
			Func<Device, SwapChainDescription, SwapChain> createSwapChainCallback, 
			bool includePixelEvents)
		{
			_frame = frame;
			_lastEvent = lastEvent;
			_createSwapChainCallback = createSwapChainCallback;
			_logger = new TracefileGraphicsLogger(new StringWriter(), includePixelEvents);
		}

		public void Replay()
		{
			bool presented = false;

			foreach (var @event in _frame.Events)
			{
				var args = @event.Arguments;
				switch (@event.OperationType)
				{
					case OperationType.CreateBlendState :
						_device.CreateBlendState(args.Get<BlendStateDescription>(0));
						break;
					case OperationType.CreateBuffer :
						_device.CreateBuffer(args.Get<BufferDescription>(0), args.Get<byte[]>(1));
						break;
					case OperationType.CreateDepthStencilState :
						_device.CreateDepthStencilState(args.Get<DepthStencilStateDescription>(0));
						break;
					case OperationType.DeviceCreate:
						_device = new Device(_logger);
						_deviceContext = _device.ImmediateContext;
						break;
					case OperationType.SwapChainCreate:
						_swapChain = _createSwapChainCallback(_device, args.Get<SwapChainDescription>(0));
						break;
					case OperationType.DeviceContextClearDepthStencilView:
						_deviceContext.ClearDepthStencilView(
							_device.GetDeviceChild<DepthStencilView>(args.Get<int>(0)),
							args.Get<DepthStencilClearFlags>(1),
							args.Get<float>(2),
							args.Get<byte>(3));
						break;
					case OperationType.DeviceContextClearRenderTargetView:
						_deviceContext.ClearRenderTargetView(
							_device.GetDeviceChild<RenderTargetView>(args.Get<int>(0)),
							args.Get<Color4F>(1));
						break;
					case OperationType.DeviceContextDraw:
						_deviceContext.Draw(args.Get<int>(0), args.Get<int>(1));
						break;
					case OperationType.DeviceContextDrawIndexed:
						_deviceContext.DrawIndexed(args.Get<int>(0), args.Get<int>(1), args.Get<int>(2));
						break;
					case OperationType.InputAssemblerStageSetInputLayout:
						_deviceContext.InputAssembler.InputLayout = _device.GetDeviceChild<InputLayout>(args.Get<int>(0));
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
						_deviceContext.InputAssembler.SetIndexBuffer(_device.GetDeviceChild<Buffer>(args.Get<int>(0)),
							args.Get<Format>(1), args.Get<int>(2));
						break;
					case OperationType.InputLayoutCreate :
						_device.CreateInputLayout(args.Get<InputElement[]>(0), args.Get<byte[]>(1));
						break;
					case OperationType.CreateRenderTargetView :
						_device.CreateRenderTargetView(_device.GetDeviceChild<Texture2D>(args.Get<int>(0)),
							args.Get<RenderTargetViewDescription?>(1));
						break;
					case OperationType.OutputMergerStageSetTargets :
						var depthStencilView = (args.Get<int?>(0) != null) ?
							_device.GetDeviceChild<DepthStencilView>(args.Get<int>(0)) 
							: null;
						_deviceContext.OutputMerger.SetTargets(depthStencilView,
							args.Get<SerializedDeviceChildArray>(1).IDs
								.Select(x => _device.GetDeviceChild<RenderTargetView>(x)).ToArray());
						break;
					case OperationType.PixelShaderStageSetShader:
						_deviceContext.PixelShader.Shader = _device.GetDeviceChild<PixelShader>(args.Get<int>(0));
						break;
					case OperationType.CreatePixelShader:
						_device.CreatePixelShader(args.Get<byte[]>(0));
						break;
					case OperationType.RasterizerStageSetViewports :
						_deviceContext.Rasterizer.SetViewports(args.Get<Viewport[]>(0));
						break;
					case OperationType.RasterizerStateCreate :
						_device.CreateRasterizerState(args.Get<RasterizerStateDescription>(0));
						break;
					case OperationType.SwapChainPresent:
						_swapChain.Present();
						presented = true;
						break;
					case OperationType.CreateTexture2D:
						_device.CreateTexture2D(args.Get<Texture2DDescription>(0));
						break;
					case OperationType.VertexShaderStageSetShader:
						_deviceContext.VertexShader.Shader = _device.GetDeviceChild<VertexShader>(args.Get<int>(0));
						break;
					case OperationType.CreateVertexShader :
						_device.CreateVertexShader(args.Get<byte[]>(0));
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (@event == _lastEvent)
					break;
			}

			if (!presented)
				_swapChain.Present();
		}
	}
}