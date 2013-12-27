using System.Collections.Generic;
using System.Linq;
using Rasterizr.Pipeline.OutputMerger;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class BlendStateViewModel
    {
        private readonly BlendState _blendState;

        public bool AlphaToCoverageEnable
        {
            get { return _blendState.Description.AlphaToCoverageEnable; }
        }

        public bool IndependentBlendEnable
        {
            get { return _blendState.Description.IndependentBlendEnable; }
        }

        public IEnumerable<RenderTargetBlendViewModel> RenderTargets
        {
            get { return _blendState.Description.RenderTarget.Select((x, i) => new RenderTargetBlendViewModel(i, x)); }
        }

        public BlendStateViewModel(BlendState blendState)
        {
            _blendState = blendState;
        }
    }

    public class RenderTargetBlendViewModel
    {
        private readonly int _number;
        private readonly RenderTargetBlendDescription _description;

        public int Number
        {
            get { return _number; }
        }

        public bool IsBlendEnabled
        {
            get { return _description.IsBlendEnabled; }
        }

        public BlendOption SourceBlend
        {
            get { return _description.SourceBlend; }
        }

        public BlendOption DestinationBlend
        {
            get { return _description.DestinationBlend; }
        }

        public BlendOperation BlendOperation
        {
            get { return _description.BlendOperation; }
        }

        public BlendOption SourceAlphaBlend
        {
            get { return _description.SourceAlphaBlend; }
        }

        public BlendOption DestinationAlphaBlend
        {
            get { return _description.DestinationAlphaBlend; }
        }

        public BlendOperation AlphaBlendOperation
        {
            get { return _description.AlphaBlendOperation; }
        }

        public ColorWriteMaskFlags RenderTargetWriteMask
        {
            get { return _description.RenderTargetWriteMask; }
        }

        public RenderTargetBlendViewModel(int number, RenderTargetBlendDescription description)
        {
            _number = number;
            _description = description;
        }
    }
}