using System.Collections.Generic;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.GeometryShader;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.PixelShader;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Resources;
using Rasterizr.Util;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr
{
	public class Device
	{
        public event DiagnosticEventHandler CreatingBlendState;
        public event DiagnosticEventHandler CreatingBuffer;
        public event DiagnosticEventHandler CreatingDepthStencilState;
        public event DiagnosticEventHandler CreatingDepthStencilView;
        public event DiagnosticEventHandler CreatingGeometryShader;
        public event DiagnosticEventHandler CreatingInputLayout;
        public event DiagnosticEventHandler CreatingPixelShader;
        public event DiagnosticEventHandler CreatingRasterizerState;
        public event DiagnosticEventHandler CreatingRenderTargetView;
        public event DiagnosticEventHandler CreatingSamplerState;
        public event DiagnosticEventHandler CreatingShaderResourceView;
        public event DiagnosticEventHandler CreatingSwapChain;
        public event DiagnosticEventHandler CreatingTexture1D;
        public event DiagnosticEventHandler CreatingTexture2D;
        public event DiagnosticEventHandler CreatingTexture3D;
        public event DiagnosticEventHandler CreatingVertexShader;

	    private readonly Dictionary<int, DeviceChild> _deviceChildMap;
		private readonly DeviceContext _immediateContext;
		private int _id;

        public IEnumerable<DeviceChild> DeviceChildren
	    {
	        get { return _deviceChildMap.Values; }
	    }

		public DeviceContext ImmediateContext
		{
			get { return _immediateContext; }
		}

		public Device()
		{
			_deviceChildMap = new Dictionary<int, DeviceChild>();
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
            DiagnosticUtilities.RaiseEvent(this, CreatingBlendState, description);
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
            DiagnosticUtilities.RaiseEvent(this, CreatingBuffer, description, initialData);

			var buffer = new Buffer(this, description);
			if (initialData != null)
				buffer.SetData(initialData);
			return buffer;
		}

		public DepthStencilState CreateDepthStencilState(DepthStencilStateDescription description)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingDepthStencilState, description);
			return new DepthStencilState(this, description);
		}

		public DepthStencilView CreateDepthStencilView(Resource resource, DepthStencilViewDescription? description = null)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingDepthStencilView, DiagnosticUtilities.GetID(resource), description);
			return new DepthStencilView(this, resource, description);
		}

		public GeometryShader CreateGeometryShader(byte[] shaderBytecode)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingGeometryShader, shaderBytecode);
			return new GeometryShader(this, shaderBytecode);
		}

		public InputLayout CreateInputLayout(InputElement[] elements, byte[] shaderBytecodeWithInputSignature)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingInputLayout, elements, shaderBytecodeWithInputSignature);
			return new InputLayout(this, elements, shaderBytecodeWithInputSignature);
		}

        public InputLayout CreateInputLayout(InputElement[] elements, InputSignatureChunk inputSignature)
        {
            return CreateInputLayout(elements, inputSignature.Container.RawBytes);
        }

		public PixelShader CreatePixelShader(byte[] shaderBytecode)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingPixelShader, shaderBytecode);
			return new PixelShader(this, shaderBytecode);
		}

		public RasterizerState CreateRasterizerState(RasterizerStateDescription description)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingRasterizerState, description);
			return new RasterizerState(this, description);
		}

		public RenderTargetView CreateRenderTargetView(Resource resource, RenderTargetViewDescription? description = null)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingRenderTargetView, DiagnosticUtilities.GetID(resource), description);
			return new RenderTargetView(this, resource, description);
		}

		public SamplerState CreateSamplerState(SamplerStateDescription description)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingSamplerState, description);
			return new SamplerState(this, description);
		}

		public ShaderResourceView CreateShaderResourceView(Resource resource, ShaderResourceViewDescription? description = null)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingShaderResourceView, DiagnosticUtilities.GetID(resource), description);
			return new ShaderResourceView(this, resource, description);
		}

        public SwapChain CreateSwapChain(SwapChainDescription description, ISwapChainPresenter presenter)
        {
            DiagnosticUtilities.RaiseEvent(this, CreatingSwapChain, description);
            return new SwapChain(this, description, presenter);
        }

        public SwapChain CreateSwapChain(int width, int height, ISwapChainPresenter presenter)
        {
            return CreateSwapChain(new SwapChainDescription(width, height), presenter);
        }

		public Texture1D CreateTexture1D(Texture1DDescription description)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingTexture1D, description);
			return new Texture1D(this, description);
		}

		public Texture2D CreateTexture2D(Texture2DDescription description)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingTexture2D, description);
			return new Texture2D(this, description);
		}

		public Texture3D CreateTexture3D(Texture3DDescription description)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingTexture3D, description);
			return new Texture3D(this, description);
		}

		public VertexShader CreateVertexShader(byte[] shaderBytecode)
		{
            DiagnosticUtilities.RaiseEvent(this, CreatingVertexShader, shaderBytecode);
			return new VertexShader(this, shaderBytecode);
		}

		#endregion
	}
}