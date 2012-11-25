using System;
using System.Collections.Generic;
using System.Linq;
using Buffer = Rasterizr.Resources.Buffer;

namespace Rasterizr.Pipeline.InputAssembler
{
	public class InputAssemblerStage
	{
		public const int VertexInputResourceSlotCount = 32;

		private readonly VertexBufferBinding[] _vertexBufferBindings;
		private IndexBufferBinding _indexBufferBinding;

		public InputLayout InputLayout { get; set; }
		public PrimitiveTopology PrimitiveTopology { get; set; }

		public InputAssemblerStage()
		{
			_vertexBufferBindings = new VertexBufferBinding[VertexInputResourceSlotCount];
		}

		public void SetVertexBuffers(int startSlot, VertexBufferBinding[] vertexBufferBindings)
		{
			for (int i = 0; i < vertexBufferBindings.Length; i++)
				_vertexBufferBindings[i + startSlot] = vertexBufferBindings[i];
		}

		public void SetIndexBuffer(Buffer indexBuffer, Format format, int offset)
		{
			if (format != Format.R16_UInt && format != Format.R32_UInt)
				throw new ArgumentOutOfRangeException("format");
			_indexBufferBinding = new IndexBufferBinding(indexBuffer, format, offset);
		}

		public void GetVertexBuffers(int startSlot, int numBuffers, VertexBufferBinding[] vertexBufferBindings)
		{
			for (int i = 0; i < numBuffers; i++)
				vertexBufferBindings[i] = _vertexBufferBindings[i + startSlot];
		}

		public void GetIndexBuffer(out Buffer indexBuffer, out Format format, out int offset)
		{
			indexBuffer = _indexBufferBinding.Buffer;
			format = _indexBufferBinding.Format;
			offset = _indexBufferBinding.Offset;
		}

		public IEnumerable<InputAssemblerVertexOutput> GetVertexStream(
			int vertexCount, 
			int startVertexLocation)
		{
			var vertexBufferIndices = _vertexBufferBindings
				.Select(x => new VertexBufferIndex(x, startVertexLocation))
				.ToArray();

			return GetVertexStreamInternal(vertexCount, 0, startVertexLocation, vertexBufferIndices);
		}

		public IEnumerable<InputAssemblerVertexOutput> GetVertexStreamIndexed(
			int indexCount, 
			int startIndexLocation, 
			int baseVertexLocation)
		{
			var vertexBufferIndices = _vertexBufferBindings
				.Select(x => new IndexedVertexBufferIndex(_indexBufferBinding, startIndexLocation, x, baseVertexLocation))
				.ToArray();

			return GetVertexStreamInternal(indexCount, 0, startIndexLocation, vertexBufferIndices);
		}

		public IEnumerable<InputAssemblerVertexOutput> GetVertexStreamInstanced(
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

				foreach (var result in GetVertexStreamInternal(vertexCountPerInstance, i, startVertexLocation, vertexBufferIndices))
					yield return result;

				foreach (var vertexBufferIndex in perInstanceBufferIndices)
					vertexBufferIndex.Increment(InputClassification.PerInstanceData);
			}
		}

		public IEnumerable<InputAssemblerVertexOutput> GetVertexStreamIndexedInstanced(
			int indexCountPerInstance, 
			int instanceCount, 
			int startIndexLocation, 
			int baseVertexLocation, 
			int startInstanceLocation)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<InputAssemblerVertexOutput> GetVertexStreamInternal(
			int vertexCount, 
			int instanceID,
			int vertexID, 
			VertexBufferIndex[] vertexBufferIndices)
		{
			for (int i = 0; i < vertexCount; i++)
			{
				var output = new InputAssemblerVertexOutput();
				output.VertexID = vertexID++;
				output.InstanceID = instanceID;
				output.Data = new byte[InputLayout.DataLength];

				foreach (var inputElement in InputLayout.Elements)
					vertexBufferIndices[inputElement.InputSlot].GetData(
						output.Data, inputElement.RegisterIndex * sizeof (float) * 4,
						inputElement.AlignedByteOffset,
						FormatHelper.SizeOfInBytes(inputElement.Format));

				yield return output;

				foreach (var vertexBufferIndex in vertexBufferIndices)
					vertexBufferIndex.Increment(InputClassification.PerVertexData);
			}
		}

		private class VertexBufferIndex
		{
			private readonly VertexBufferBinding _binding;
			private readonly int _offset;
			private int _index;

			public virtual InputClassification InputDataClass
			{
				get { return InputClassification.PerVertexData; }
			}

			public VertexBufferIndex(VertexBufferBinding binding, int startLocation)
			{
				_binding = binding;
				_offset = binding.Offset + (startLocation * binding.Stride);
			}

			public void GetData(byte[] dst, int dstOffset, int offset, int count)
			{
				_binding.Buffer.GetData(dstOffset, dst, _offset + (GetIndex(_index) * _binding.Stride) + offset, count);
			}

			protected virtual int GetIndex(int index)
			{
				return index;
			}

			public void Increment(InputClassification inputDataClass)
			{
				_index = GetNextIndex(inputDataClass, _index);
			}

			protected virtual int GetNextIndex(InputClassification inputDataClass, int index)
			{
				return index + 1;
			}

			public void Reset()
			{
				_index = 0;
			}
		}

		private class InstancedVertexBufferIndex : VertexBufferIndex
		{
			private readonly int _stepRate;
			private int _steps;
			private int _index;

			public override InputClassification InputDataClass
			{
				get { return InputClassification.PerInstanceData; }
			}

			public InstancedVertexBufferIndex(
				int instanceDataStepRate,
				VertexBufferBinding binding,
				int startLocation)
				: base(binding, startLocation)
			{
				_stepRate = instanceDataStepRate;
			}

			protected override int GetNextIndex(InputClassification inputDataClass, int index)
			{
				if (_stepRate == 0 || inputDataClass == InputClassification.PerVertexData)
					return index;
				if (++_steps == _stepRate)
				{
					_steps = 0;
					_index++;
				}
				return _index;
			}
		}

		private class IndexedVertexBufferIndex : VertexBufferIndex
		{
			private readonly IndexBufferBinding _indexBufferBinding;
			private readonly int _offset;

			public IndexedVertexBufferIndex(
				IndexBufferBinding indexBufferBinding, int startIndexLocation,
				VertexBufferBinding vertexBufferBinding, int startVertexLocation)
				: base(vertexBufferBinding, startVertexLocation)
			{
				_indexBufferBinding = indexBufferBinding;
				_offset = indexBufferBinding.Offset + (startIndexLocation * FormatHelper.SizeOfInBytes(indexBufferBinding.Format));
			}

			protected override int GetIndex(int index)
			{
				if (_indexBufferBinding.Format == Format.R32_UInt)
				{
					var data = new uint[1];
					_indexBufferBinding.Buffer.GetData(data, _offset + index, 1);
					return (int) data[0];
				}
				else
				{
					var data = new ushort[1];
					_indexBufferBinding.Buffer.GetData(data, _offset + index, 1);
					return data[0];
				}
			}
		}

		public struct InputAssemblerVertexOutput
		{
			public int VertexID;
			public int InstanceID;
			public byte[] Data;
		}
	}
}