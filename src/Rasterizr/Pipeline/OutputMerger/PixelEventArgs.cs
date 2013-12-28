using System;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.VertexShader;
using SlimShader;

namespace Rasterizr.Pipeline.OutputMerger
{
    internal class PixelEventArgs : EventArgs
    {
        private readonly VertexShaderOutput[] _vertices;
        private readonly int _primitiveID;
        private readonly uint _renderTargetArrayIndex;
        private readonly int _x;
        private readonly int _y;
        private readonly Number4 _pixelShader;
        private readonly Number4 _previous;
        private readonly Number4? _result;
        private readonly PixelExclusionReason _exclusionReason;

        public VertexShaderOutput[] Vertices
        {
            get { return _vertices; }
        }

        public int PrimitiveID
        {
            get { return _primitiveID; }
        }

        public uint RenderTargetArrayIndex
        {
            get { return _renderTargetArrayIndex; }
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public Number4 PixelShader
        {
            get { return _pixelShader; }
        }

        public Number4 Previous
        {
            get { return _previous; }
        }

        public Number4? Result
        {
            get { return _result; }
        }

        public PixelExclusionReason ExclusionReason
        {
            get { return _exclusionReason; }
        }

        public PixelEventArgs(
            VertexShaderOutput[] vertices, 
            int primitiveID,
            uint renderTargetArrayIndex,
            int x,
            int y,
            ref Number4 pixelShader,
            ref Number4 previous,
            Number4? result,
            PixelExclusionReason exclusionReason)
        {
            _vertices = vertices;
            _primitiveID = primitiveID;
            _renderTargetArrayIndex = renderTargetArrayIndex;
            _x = x;
            _y = y;
            _pixelShader = pixelShader;
            _previous = previous;
            _result = result;
            _exclusionReason = exclusionReason;
        }
    }

    internal delegate void PixelEventHandler(object sender, PixelEventArgs args);
}