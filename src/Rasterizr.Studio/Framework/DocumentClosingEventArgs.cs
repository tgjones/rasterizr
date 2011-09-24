using System;

namespace Rasterizr.Studio.Framework
{
    /// <summary>
    /// Arguments to the DocumentClosing event.
    /// </summary>
    public sealed class DocumentClosingEventArgs : EventArgs
    {
        internal DocumentClosingEventArgs(object document)
        {
            this.Document = document;
            this.Cancel = false;
        }

        /// <summary>
        /// View-model object for the document being closed.
        /// </summary>
        public object Document
        {
            get;
            private set;
        }

        /// <summary>
        /// Set to 'true' to cancel closing of the document.
        /// The defualt value is 'false'.
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
    }
}
