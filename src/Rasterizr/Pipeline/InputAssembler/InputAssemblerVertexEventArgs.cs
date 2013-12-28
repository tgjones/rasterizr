using System;

namespace Rasterizr.Pipeline.InputAssembler
{
    internal class InputAssemblerVertexEventArgs : EventArgs
    {
        private readonly InputAssemblerVertexOutput _vertex;

        public InputAssemblerVertexOutput Vertex
        {
            get { return _vertex; }
        }

        public InputAssemblerVertexEventArgs(InputAssemblerVertexOutput vertex)
        {
            _vertex = vertex;
        }
    }

    internal delegate void InputAssemblerVertexEventHandler(object sender, InputAssemblerVertexEventArgs args);
}