using Rasterizr.Pipeline;

namespace Rasterizr.Studio.Modules.GraphicsObjectTable.ViewModels
{
    public class ShaderViewModel
    {
        private readonly ShaderBase _shader;

        public string Disassembly
        {
            get { return _shader.Bytecode.ToString(); }
        }

        public ShaderViewModel(ShaderBase shader)
        {
            _shader = shader;
        }
    }
}