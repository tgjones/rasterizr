using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.InputAssembler
{
	public class InputLayout : DeviceChild
	{
		private readonly InputSignatureChunk _inputSignature;
		private readonly ProcessedInputElement[] _elements;
		private readonly InputSlotElement[] _slots;

		public int DataLength
		{
			get { return _inputSignature.Parameters.Count * sizeof(float) * 4; }
		}

		public ProcessedInputElement[] Elements
		{
			get { return _elements; }
		}

		public InputSlotElement[] Slots
		{
			get { return _slots; }
		}

		public InputLayout(Device device, InputSignatureChunk inputSignature, InputElement[] elements)
			: base(device)
		{
			_inputSignature = inputSignature;
			// TODO: Verify that shader bytecode matches input elements.
			_elements = ProcessElements(elements);
			_slots = ProcessSlots(elements);
		}

		private ProcessedInputElement[] ProcessElements(InputElement[] elements)
		{
			var slotOffsets = new int[InputAssemblerStage.VertexInputResourceSlotCount];
			var result = new ProcessedInputElement[elements.Length];
			for (int i = 0; i < elements.Length; i++)
			{
				var element = elements[i];

				if (element.AlignedByteOffset != InputElement.AppendAligned)
					slotOffsets[element.InputSlot] = element.AlignedByteOffset;

				result[i] = new ProcessedInputElement
				{
					RegisterIndex = FindRegisterIndex(element.SemanticName, element.SemanticIndex),
					Format = element.Format,
					InputSlot = element.InputSlot,
					AlignedByteOffset = slotOffsets[element.InputSlot],
					InputSlotClass = element.InputSlotClass,
					InstanceDataStepRate = element.InstanceDataStepRate
				};

				slotOffsets[element.InputSlot] += FormatHelper.SizeOfInBytes(element.Format);
			}
			return result;
		}

		private int FindRegisterIndex(string semanticName, int semanticIndex)
		{
			var inputParameter = _inputSignature.Parameters
				.SingleOrDefault(x => x.SemanticName == semanticName && x.SemanticIndex == semanticIndex);
			if (inputParameter == null)
				throw new Exception(string.Format("No matching input parameter for semantic name '{0}' and index '{1}'.",
					semanticName, semanticIndex));
			return (int) inputParameter.Register;
		}

		private InputSlotElement[] ProcessSlots(InputElement[] elements)
		{
			var result = new List<InputSlotElement>();
			for (int i = 0; i < elements.Length; i++)
			{
				var element = elements[i];
				var existingSlotElement = result.SingleOrDefault(x => x.InputSlot == element.InputSlot);
				if (existingSlotElement == null)
				{
					result.Add(new InputSlotElement
					{
						InputSlot = element.InputSlot,
						InputSlotClass = element.InputSlotClass,
						InstanceDataStepRate = element.InstanceDataStepRate
					});
				}
				else
				{
					if (element.InputSlotClass != existingSlotElement.InputSlotClass)
						throw new Exception(string.Format("Element[{0}]'s InputSlotClass is different from the InputSlotClass of a previously defined element at the same input slot.  All elements from a given input slot must have the same InputSlotClass and InstanceDataStepRate.", i));
					if (element.InstanceDataStepRate != existingSlotElement.InstanceDataStepRate)
						throw new Exception(string.Format("Element[{0}]'s InstanceDataStepRate is different from the InstanceDataStepRate of a previously defined element at the same input slot.  All elements from a given input slot must have the same InputSlotClass and InstanceDataStepRate.", i));
				}
			}

			return result.ToArray();
		}

		public class ProcessedInputElement
		{
			public int RegisterIndex;
			public Format Format;
			public int InputSlot;
			public int AlignedByteOffset;
			public InputClassification InputSlotClass;
			public int InstanceDataStepRate;
		}

		public class InputSlotElement
		{
			public int InputSlot;
			public InputClassification InputSlotClass;
			public int InstanceDataStepRate;
		}
	}
}