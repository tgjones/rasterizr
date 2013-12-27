using System.Collections.Generic;
using System.Linq;
using Rasterizr.Resources;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class BufferViewModel
    {
        private readonly Buffer _buffer;

        public IEnumerable<BufferDatumViewModel> Data
        {
            get { return _buffer.Data.Select((d, i) => new BufferDatumViewModel(i, d)); }
        }

        public BufferViewModel(Buffer buffer)
        {
            _buffer = buffer;
        }
    }

    public class BufferDatumViewModel
    {
        private readonly int _index;
        private readonly byte _value;

        public int Index
        {
            get { return _index; }
        }

        public string Value
        {
            get { return _value.ToString(); }
        }

        public BufferDatumViewModel(int index, byte value)
        {
            _index = index;
            _value = value;
        }
    }
}