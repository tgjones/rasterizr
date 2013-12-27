using System.Collections.Generic;
using System.Linq;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class InputLayoutViewModel
    {
        private readonly InputLayout _inputLayout;

        public IEnumerable<InputElementViewModel> Elements
        {
            get { return _inputLayout.RawElements.Select(x => new InputElementViewModel(x)); }
        }

        public InputLayoutViewModel(InputLayout inputLayout)
        {
            _inputLayout = inputLayout;
        }
    }

    public class InputElementViewModel
    {
        private readonly InputElement _inputElement;

        public string SemanticName
        {
            get { return _inputElement.SemanticName; }
        }

        public int SemanticIndex
        {
            get { return _inputElement.SemanticIndex; }
        }

        public string Format
        {
            get { return _inputElement.Format.ToString(); }
        }

        public int InputSlot
        {
            get { return _inputElement.InputSlot; }
        }

        public int AlignedByteOffset
        {
            get { return _inputElement.AlignedByteOffset; }
        }

        public string InputSlotClass
        {
            get { return _inputElement.InputSlotClass.ToString(); }
        }

        public int InstanceDataStepRate
        {
            get { return _inputElement.InstanceDataStepRate; }
        }

        public InputElementViewModel(InputElement inputElement)
        {
            _inputElement = inputElement;
        }
    }
}