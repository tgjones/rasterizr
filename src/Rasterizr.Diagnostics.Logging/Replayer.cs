﻿using System;
using System.Linq;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Diagnostics.Logging.Serialization;
using Rasterizr.Math;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.VertexShader;
using SlimShader;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Diagnostics.Logging
{
	public class Replayer
	{
		private readonly TracefileFrame _frame;
		private readonly Func<Device, SwapChainDescription, SwapChain> _createSwapChainCallback;

		private SwapChain _swapChain;
		private Device _device;
		private DeviceContext _deviceContext;

		public Replayer(TracefileFrame frame, Func<Device, SwapChainDescription, SwapChain> createSwapChainCallback)
		{
			_frame = frame;
			_createSwapChainCallback = createSwapChainCallback;
		}

		public void Replay()
		{
			foreach (var @event in _frame.Events)
			{
				var args = @event.Arguments;
				switch (@event.OperationType)
				{
					case OperationType.DeviceCreate:
						_device = new Device();
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
					case OperationType.VertexShaderStageSetShader:
						_deviceContext.VertexShader.Shader = _device.GetDeviceChild<VertexShader>(args.Get<int>(0));
						break;
					case OperationType.VertexShaderCreate :
						new VertexShader(_device, BytecodeContainer.Parse(args.Get<byte[]>(0)));
						break;
					case OperationType.PixelShaderStageSetShader:
						_deviceContext.PixelShader.Shader = new PixelShader(_device, 
							BytecodeContainer.Parse(args.Get<byte[]>(0)));
						break;
					case OperationType.PixelShaderCreate :
						new PixelShader(_device, BytecodeContainer.Parse(args.Get<byte[]>(0)));
						break;
					case OperationType.SwapChainPresent:
						_swapChain.Present();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}