using SlimShader;

namespace Rasterizr.Pipeline.InputAssembler
{
	internal class VertexBufferIndex
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

		public void GetData(Number4[] dst, int dstOffset, int offset, int count)
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
}