using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Resources;
using Rasterizr.Toolkit.Effects;

namespace Rasterizr.Toolkit
{
	public class ModelMesh
	{
		private readonly Device _device;
		private readonly List<VertexPositionNormalTexture> _vertices;
		private readonly List<int> _indices;
		private InputLayout _inputLayout;
		private Effect _effect;
		private Buffer _vertexBuffer;
		private Buffer _indexBuffer;

		public Effect Effect
		{
			get { return _effect; }
			set
			{
				_effect = value;
				_inputLayout = _device.CreateInputLayout(
                    VertexPositionNormalTexture.InputElements,
					Effect.VertexShader.Bytecode.RawBytes);
			}
		}

		public string Name { get; set; }

		public ModelMesh(Device device, List<VertexPositionNormalTexture> vertices, List<int> indices)
		{
			_device = device;
			_vertices = vertices;
			_indices = indices;
			//_vertexBuffer = device.CreateBuffer();
			//_indexBuffer = device.CreateBuffer();
		}

		public void Draw()
		{
			_device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
			_device.ImmediateContext.InputAssembler.SetVertexBuffers(0,
				new VertexBufferBinding(_vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes));
			_device.ImmediateContext.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);

		    _effect.Apply();
            _device.ImmediateContext.DrawIndexed(_indices.Count, 0, 0);
		}
	}
}