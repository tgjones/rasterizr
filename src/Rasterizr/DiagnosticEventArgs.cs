using System;

namespace Rasterizr
{
    public class DiagnosticEventArgs : EventArgs
    {
        private readonly object[] _arguments;

        public object[] Arguments
        {
            get { return _arguments; }
        }

        public DiagnosticEventArgs(params object[] arguments)
        {
            _arguments = arguments;
        }
    }

    public delegate void DiagnosticEventHandler(object sender, DiagnosticEventArgs args);
}