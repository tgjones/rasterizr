using System;
using System.Collections.Generic;
using System.Linq;
using Rasterizr.Pipeline.VertexShader;
using Rasterizr.Util;
using SlimShader;
using SlimShader.Chunks.Xsgn;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Pipeline.InputAssembler
{
    public class InputAssemblerStage
	{
	    public event DiagnosticEventHandler SettingPrimitiveTopology;
        public event DiagnosticEventHandler SettingInputLayout;
        public event DiagnosticEventHandler SettingVertexBuffers;
        public event DiagnosticEventHandler SettingIndexBuffer;

        internal event InputAssemblerVertexEventHandler ProcessedVertex;

		public const int VertexInputResourceSlotCount = 32;

        private readonly Device _device;
        private readonly VertexBufferBinding[] _vertexBufferBindings;
		private IndexBufferBinding _indexBufferBinding;
		private InputLayout _inputLayout;
		private PrimitiveTopology _primitiveTopology;

		public InputLayout InputLayout
		{
			get { return _inputLayout; }
			set
			{
                DiagnosticUtilities.RaiseEvent(this, SettingInputLayout, DiagnosticUtilities.GetID(value));
				_inputLayout = value;
			}
		}

		public PrimitiveTopology PrimitiveTopology
		{
			get { return _primitiveTopology; }
			set
			{
                DiagnosticUtilities.RaiseEvent(this, SettingPrimitiveTopology, value);
				_primitiveTopology = value;
			}
		}

		public InputAssemblerStage(Device device)
		{
			_device = device;
			_vertexBufferBindings = new VertexBufferBinding[VertexInputResourceSlotCount];
		}
        
		public void GetVertexBuffers(int startSlot, int numBuffers, VertexBufferBinding[] vertexBufferBindings)
		{
			for (int i = 0; i < numBuffers; i++)
				vertexBufferBindings[i] = _vertexBufferBindings[i + startSlot];
		}

		public void SetVertexBuffers(int startSlot, params VertexBufferBinding[] vertexBufferBindings)
		{
            DiagnosticUtilities.RaiseEvent(this, SettingVertexBuffers, startSlot,
		        vertexBufferBindings.Select(x => new SerializedVertexBufferBinding
		        {
		            Buffer = x.Buffer.ID,
		            Offset = x.Offset,
		            Stride = x.Stride
		        }).ToArray());
			for (int i = 0; i < vertexBufferBindings.Length; i++)
				_vertexBufferBindings[i + startSlot] = vertexBufferBindings[i];
		}

		public void GetIndexBuffer(out Buffer indexBuffer, out Format format, out int offset)
		{
			indexBuffer = _indexBufferBinding.Buffer;
			format = _indexBufferBinding.Format;
			offset = _indexBufferBinding.Offset;
		}

		public void SetIndexBuffer(Buffer indexBuffer, Format format, int offset)
		{
            DiagnosticUtilities.RaiseEvent(this, SettingIndexBuffer, DiagnosticUtilities.GetID(indexBuffer), format, offset);
			if (format != Format.R16_UInt && format != Format.R32_UInt)
				throw new ArgumentOutOfRangeException("format");
			_indexBufferBinding = new IndexBufferBinding(indexBuffer, format, offset);
		}

		internal IEnumerable<InputAssemblerVertexOutput> GetVertexStream(
			InputSignatureChunk vertexShaderInputSignature,
			int vertexCount, 
			int startVertexLocation)
		{
			var vertexBufferIndices = _vertexBufferBindings
				.Select(x => new VertexBufferIndex(x, startVertexLocation))
				.ToArray();

			return GetVertexStreamInternal(vertexShaderInputSignature, vertexCount, 0, startVertexLocation, vertexBufferIndices);
		}

		internal IEnumerable<InputAssemblerVertexOutput> GetVertexStreamIndexed(
			InputSignatureChunk vertexShaderInputSignature,
			int indexCount, 
			int startIndexLocation, 
			int baseVertexLocation)
		{
			var vertexBufferIndices = _vertexBufferBindings
				.Select(x => new IndexedVertexBufferIndex(_indexBufferBinding, startIndexLocation, x, baseVertexLocation))
				.ToArray();

			return GetVertexStreamInternal(vertexShaderInputSignature, indexCount, 0, startIndexLocation, vertexBufferIndices);
		}

		internal IEnumerable<InputAssemblerVertexOutput> GetVertexStreamInstanced(
			InputSignatureChunk vertexShaderInputSignature,
			int vertexCountPerInstance, 
			int instanceCount, 
			int startVertexLocation, 
			int startInstanceLocation)
		{
			// Setup per-instance data (applies across draw call).
			// TODO: This isn't quite right - we need make vertexBufferIndices a dictionary, keyed on slot.
			var vertexBufferIndices = InputLayout.Slots
				.Select(x => (x.InputSlotClass == InputClassification.PerInstanceData) 
					? new InstancedVertexBufferIndex(x.InstanceDataStepRate, _vertexBufferBindings[x.InputSlot], startInstanceLocation) 
					: new VertexBufferIndex(_vertexBufferBindings[x.InputSlot], startVertexLocation))
				.ToArray();
			var perInstanceBufferIndices = vertexBufferIndices
				.Where(x => x.InputDataClass == InputClassification.PerInstanceData)
				.ToArray();
			var perVertexBufferIndices = vertexBufferIndices
				.Where(x => x.InputDataClass == InputClassification.PerVertexData)
				.ToArray();

			for (int i = 0; i < instanceCount; i++)
			{
				// Reset per-vertex data (applies to each instance).
				foreach (var perVertexBufferIndex in perVertexBufferIndices)
					perVertexBufferIndex.Reset();

				foreach (var result in GetVertexStreamInternal(vertexShaderInputSignature, vertexCountPerInstance, i, startVertexLocation, vertexBufferIndices))
					yield return result;

				foreach (var vertexBufferIndex in perInstanceBufferIndices)
					vertexBufferIndex.Increment(InputClassification.PerInstanceData);
			}
		}

		internal IEnumerable<InputAssemblerVertexOutput> GetVertexStreamIndexedInstanced(
			InputSignatureChunk vertexShaderInputSignature,
			int indexCountPerInstance, 
			int instanceCount, 
			int startIndexLocation, 
			int baseVertexLocation, 
			int startInstanceLocation)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<InputAssemblerVertexOutput> GetVertexStreamInternal(
			InputSignatureChunk vertexShaderInputSignature,
			int vertexCount, int instanceID, int vertexID, 
			VertexBufferIndex[] vertexBufferIndices)
		{
			var inputParameterCount = vertexShaderInputSignature.Parameters.Count;
			Dictionary<int, InputLayout.ProcessedInputElement> inputElementsKeyedByRegister = null;
			if (InputLayout != null)
				inputElementsKeyedByRegister = InputLayout.Elements.ToDictionary(x => x.RegisterIndex);

		    var processedVertexHandler = ProcessedVertex;

			for (int i = 0; i < vertexCount; i++)
			{
				var output = new InputAssemblerVertexOutput();
				output.VertexID = vertexID++;
				output.InstanceID = instanceID;
				output.Data = new Number4[inputParameterCount];
				
				// TODO: Support non-32-bit formats.
				foreach (var parameter in vertexShaderInputSignature.Parameters)
				{
					switch (parameter.SystemValueType)
					{
						case Name.Undefined:
						case Name.Position:
							if (inputElementsKeyedByRegister == null)
								throw new Exception("InputLayout must be set in order to use these system value types.");
							var inputElement = inputElementsKeyedByRegister[(int) parameter.Register];
							vertexBufferIndices[inputElement.InputSlot].GetData(
								output.Data, inputElement.RegisterIndex,
								inputElement.AlignedByteOffset,
								FormatHelper.SizeOfInBytes(inputElement.Format));
							break;
						case Name.VertexID:
							output.Data[parameter.Register] = new Number4(output.VertexID, 0, 0, 0);
							break;
						case Name.InstanceID:
							output.Data[parameter.Register] = new Number4(output.InstanceID, 0, 0, 0);
							break;
					}
				}

                if (processedVertexHandler != null)
                    processedVertexHandler(this, new InputAssemblerVertexEventArgs(output));

				yield return output;

				foreach (var vertexBufferIndex in vertexBufferIndices)
					vertexBufferIndex.Increment(InputClassification.PerVertexData);
			}
		}
		
		internal IEnumerable<InputAssemblerPrimitiveOutput> GetPrimitiveStream(IEnumerable<VertexShaderOutput> shadedVertices)
		{
			return PrimitiveAssembler.GetPrimitiveStream(shadedVertices, PrimitiveTopology);
		}
	}
}