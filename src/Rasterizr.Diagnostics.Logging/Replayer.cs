using System;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Nexus.Graphics.Colors;
using Rasterizr.InputAssembler;
using Rasterizr.ShaderCore;
using Rasterizr.ShaderCore.PixelShader;
using Int32Collection = Nexus.Int32Collection;

namespace Rasterizr.Diagnostics.Logging
{
	public class Replayer
	{
		private readonly TracefileFrame _frame;

		private SwapChain _swapChain;
		private WriteableBitmap _output;
		private RasterizrDevice _device;

		public Replayer(TracefileFrame frame)
		{
			_frame = frame;
		}

		public void Replay()
		{
			foreach (var @event in _frame.Events)
			{
				var args = @event.Arguments;
				switch (@event.OperationType)
				{
					case OperationType.CreateDevice :
						_device = new RasterizrDevice();
						break;
					case OperationType.CreateSwapChain:
					{
						var width = args.Get<int>(1);
						var height = args.Get<int>(2);
						_output = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
						_swapChain = new SwapChain(_device, _output, width, height, args.Get<int>(3));
						break;
					}
					case OperationType.DeviceClearDepthBuffer:
						_device.ClearDepthBuffer(args.Get<float>(0));
						break;
					case OperationType.DeviceClearRenderTarget:
						_device.ClearRenderTarget(args.Get<ColorF>(0));
						break;
					case OperationType.DeviceDraw:
						_device.Draw(args.Get<int>(0), args.Get<int>(1));
						break;
					case OperationType.DeviceDrawIndexed:
						_device.DrawIndexed(args.Get<int>(0), args.Get<int>(1), args.Get<int>(2));
						break;
					case OperationType.InputAssemblerSetInputLayout:
						_device.InputAssembler.InputLayout = args.Get<InputLayout>(0);
						break;
					case OperationType.InputAssemblerSetPrimitiveTopology:
						_device.InputAssembler.PrimitiveTopology = args.Get<PrimitiveTopology>(0);
						break;
					case OperationType.InputAssemblerSetVertices:
						_device.InputAssembler.Vertices = args.Get<IList>(0);
						break;
					case OperationType.InputAssemblerSetIndices:
						_device.InputAssembler.Indices = args.Get<Int32Collection>(0);
						break;
					case OperationType.VertexShaderSetShader:
						_device.VertexShader.VertexShader = args.Get<IShader>(0);
						break;
					case OperationType.PixelShaderSetShader:
						_device.PixelShader.PixelShader = args.Get<IPixelShader>(0);
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