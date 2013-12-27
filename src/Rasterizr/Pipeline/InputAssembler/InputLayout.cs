using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.InputAssembler
{
	public class InputLayout : DeviceChild
	{
	    private readonly InputElement[] _rawElements;
		private readonly ProcessedInputElement[] _elements;
		private readonly InputSlotElement[] _slots;
		private readonly InputSignatureChunk _inputSignature;

	    internal InputElement[] RawElements
	    {
	        get { return _rawElements; }
	    }

		public ProcessedInputElement[] Elements
		{
			get { return _elements; }
		}

		public InputSlotElement[] Slots
		{
			get { return _slots; }
		}

		internal InputLayout(Device device, InputElement[] elements, byte[] shaderBytecodeWithInputSignature)
			: this(device, elements, BytecodeContainer.Parse(shaderBytecodeWithInputSignature).InputSignature)
		{
			
		}

		/// <summary>
		/// Only used in unit tests.
		/// </summary>
		internal InputLayout(Device device, InputElement[] elements, InputSignatureChunk inputSignature)
			: base(device)
		{
			// TODO: Verify that shader bytecode matches input elements.
			_inputSignature = inputSignature;
		    _rawElements = elements;
			_elements = ProcessElements(elements);
			_slots = ProcessSlots(elements);
		}

		private ProcessedInputElement[] ProcessElements(InputElement[] elements)
		{
			var slotOffsets = new int[InputAssemblerStage.VertexInputResourceSlotCount];

			var result = new List<ProcessedInputElement>();
			foreach (var element in elements)
			{
			    var registerIndex = _inputSignature.Parameters.FindRegister(
			        element.SemanticName, (uint) element.SemanticIndex);

			    // If registerIndex is null, it means that this element in the input
			    // data has no corresponding vertex shader input. That's okay -
			    // we just ignore it.
			    if (registerIndex == null)
			        continue;

			    if (element.AlignedByteOffset != InputElement.AppendAligned)
			        slotOffsets[element.InputSlot] = element.AlignedByteOffset;

			    result.Add(new ProcessedInputElement
			    {
			        RegisterIndex = (int) registerIndex.Value,
			        Format = element.Format,
			        InputSlot = element.InputSlot,
			        AlignedByteOffset = slotOffsets[element.InputSlot],
			        InputSlotClass = element.InputSlotClass,
			        InstanceDataStepRate = element.InstanceDataStepRate
			    });

			    slotOffsets[element.InputSlot] += FormatHelper.SizeOfInBytes(element.Format);
			}
			return result.ToArray();
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