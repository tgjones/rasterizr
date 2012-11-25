namespace Rasterizr.Pipeline.InputAssembler
{
	public struct InputElement
	{
		public static int AppendAligned
		{
			get { return -1; }
		}

		public string SemanticName;
		public int SemanticIndex;
		public Format Format;
		public int InputSlot;
		public int AlignedByteOffset;
		public InputClassification InputSlotClass;
		public int InstanceDataStepRate;

		public InputElement(string semanticName, int semanticIndex, Format format, int inputSlot, int alignedByteOffset,
			InputClassification inputSlotClass, int instanceDataStepRate)
		{
			SemanticName = semanticName;
			SemanticIndex = semanticIndex;
			Format = format;
			InputSlot = inputSlot;
			AlignedByteOffset = alignedByteOffset;
			InputSlotClass = inputSlotClass;
			InstanceDataStepRate = instanceDataStepRate;
		}

		public InputElement(string semanticName, int semanticIndex, Format format, int inputSlot, int alignedByteOffset)
			: this(semanticName, semanticIndex, format, inputSlot, alignedByteOffset, InputClassification.PerVertexData, 0)
		{
			
		}

		public InputElement(string semanticName, int semanticIndex, Format format, int inputSlot)
			: this(semanticName, semanticIndex, format, inputSlot, AppendAligned)
		{
			
		}
	}
}