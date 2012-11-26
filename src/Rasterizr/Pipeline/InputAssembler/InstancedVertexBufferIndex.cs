namespace Rasterizr.Pipeline.InputAssembler
{
	internal class InstancedVertexBufferIndex : VertexBufferIndex
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
}