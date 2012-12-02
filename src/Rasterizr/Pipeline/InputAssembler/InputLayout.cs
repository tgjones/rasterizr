using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.InputAssembler
{
	public class InputLayout : DeviceChild
	{
		private readonly ProcessedInputElement[] _elements;
		private readonly InputSlotElement[] _slots;
		private readonly InputSignatureChunk _inputSignature;

		public ProcessedInputElement[] Elements
		{
			get { return _elements; }
		}

		public InputSlotElement[] Slots
		{
			get { return _slots; }
		}

		public int ShaderInputParameterCount
		{
			get { return _inputSignature.Parameters.Count; }
		}

		internal InputLayout(Device device, InputElement[] elements, byte[] shaderBytecodeWithInputSignature)
			: base(device)
		{
			// TODO: Verify that shader bytecode matches input elements.
			_elements = ProcessElements(elements);
			_slots = ProcessSlots(elements);
			_inputSignature = BytecodeContainer.Parse(shaderBytecodeWithInputSignature).InputSignature;
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
					RegisterIndex = (int) _inputSignature.Parameters.FindRegister(element.SemanticName, (uint) element.SemanticIndex),
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