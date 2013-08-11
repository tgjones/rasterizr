using System.Collections.Generic;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.GeometryShader;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;
using Rasterizr.Util;

namespace Rasterizr
{
	public class Device
	{
		private readonly Dictionary<int, DeviceChild> _deviceChildMap;
		private readonly GraphicsLoggerCollection _loggers;
		private readonly DeviceContext _immediateContext;
		private int _id;

		internal GraphicsLoggerCollection Loggers
		{
			get { return _loggers; }
		}

		public DeviceContext ImmediateContext
		{
			get { return _immediateContext; }
		}

		public Device(params GraphicsLogger[] loggers)
		{
			_deviceChildMap = new Dictionary<int, DeviceChild>();
			if (loggers == null)
				loggers = new GraphicsLogger[0];
			_loggers = new GraphicsLoggerCollection(loggers);
			_loggers.BeginOperation(OperationType.DeviceCreate);
			_immediateContext = new DeviceContext(this);
		}

		public T GetDeviceChild<T>(int id)
			where T : DeviceChild
		{
			return (T) _deviceChildMap[id];
		}

		internal int RegisterDeviceChild(DeviceChild deviceChild)
		{
			int id = _id;
			_deviceChildMap[id] = deviceChild;
			_id++;
			return id;
		}

		#region Create methods

		public BlendState CreateBlendState(BlendStateDescription description)
		{
			Loggers.BeginOperation(OperationType.CreateBlendState, description);
			return new BlendState(this, description);
		}

		public Buffer CreateBuffer<T>(BufferDescription description, T[] data)
			where T : struct
		{
			if (description.SizeInBytes == 0)
				description.SizeInBytes = data.Length * Utilities.SizeOf<T>();

			return CreateBuffer(description, Utilities.ToByteArray(data));
		}

		public Buffer CreateBuffer(BufferDescription description, byte[] initialData = null)
		{
			Loggers.BeginOperation(OperationType.CreateBuffer, description, initialData);

			var buffer = new Buffer(this, description);
			if (initialData != null)
				buffer.SetData(initialData);
			return buffer;
		}

		public DepthStencilState CreateDepthStencilState(DepthStencilStateDescription description)
		{
			Loggers.BeginOperation(OperationType.CreateDepthStencilState, description);
			return new DepthStencilState(this, description);
		}

		public DepthStencilView CreateDepthStencilView(Resource resource, DepthStencilViewDescription? description = null)
		{
			Loggers.BeginOperation(OperationType.CreateDepthStencilView, resource, description);
			return new DepthStencilView(this, resource, description);
		}

		public GeometryShader CreateGeometryShader(byte[] shaderBytecode)
		{
			Loggers.BeginOperation(OperationType.CreateGeometryShader, shaderBytecode);
			return new GeometryShader(this, shaderBytecode);
		}

		public InputLayout CreateInputLayout(InputElement[] elements, byte[] shaderBytecodeWithInputSignature)
		{
			Loggers.BeginOperation(OperationType.InputLayoutCreate, elements, shaderBytecodeWithInputSignature);
			return new InputLayout(this, elements, shaderBytecodeWithInputSignature);
		}

		public PixelShader CreatePixelShader(byte[] shaderBytecode)
		{
			Loggers.BeginOperation(OperationType.CreatePixelShader, shaderBytecode);
			return new PixelShader(this, shaderBytecode);
		}

		public RasterizerState CreateRasterizerState(RasterizerStateDescription description)
		{
			Loggers.BeginOperation(OperationType.RasterizerStateCreate, description);
			return new RasterizerState(this, description);
		}

		public RenderTargetView CreateRenderTargetView(Resource resource, RenderTargetViewDescription? description = null)
		{
			Loggers.BeginOperation(OperationType.CreateRenderTargetView, resource, description);
			return new RenderTargetView(this, resource, description);
		}

		public SamplerState CreateSamplerState(SamplerStateDescription description)
		{
			Loggers.BeginOperation(OperationType.CreateSamplerState, description);
			return new SamplerState(this, description);
		}

		public ShaderResourceView CreateShaderResourceView(Resource resource, ShaderResourceViewDescription? description = null)
		{
			Loggers.BeginOperation(OperationType.CreateShaderResourceView, resource, description);
			return new ShaderResourceView(this, resource, description);
		}

		public Texture1D CreateTexture1D(Texture1DDescription description)
		{
			Loggers.BeginOperation(OperationType.CreateTexture1D, description);
			return new Texture1D(this, description);
		}

		public Texture2D CreateTexture2D(Texture2DDescription description)
		{
			Loggers.BeginOperation(OperationType.CreateTexture2D, description);
			return new Texture2D(this, description);
		}

		public Texture3D CreateTexture3D(Texture3DDescription description)
		{
			Loggers.BeginOperation(OperationType.CreateTexture3D, description);
			return new Texture3D(this, description);
		}

		public VertexShader CreateVertexShader(byte[] shaderBytecode)
		{
			Loggers.BeginOperation(OperationType.CreateVertexShader, shaderBytecode);
			return new VertexShader(this, shaderBytecode);
		}

		#endregion
	}
}