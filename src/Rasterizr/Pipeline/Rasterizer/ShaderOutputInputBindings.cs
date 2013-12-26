using System.Collections.Generic;
using System.Linq;
using SlimShader;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Pipeline.Rasterizer
{
    internal class ShaderOutputInputBindings
    {
        public ShaderOutputInputBinding[] Bindings;

        /// <summary>
        /// Registers are effectively shared between outputs of the previous stage
        /// and inputs of the next stage, so we don't need to do any mapping. We only
        /// need to keep track of the interpolation mode, and system value semantic,
        /// of the inputs.
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb509650(v=vs.85).aspx"/>
        /// 
        /// TODO: Add some sort of runtime validation that signatures match. Direct3D
        /// does this, but only when debugging.
        /// </summary>
        public static ShaderOutputInputBindings FromShaderSignatures(
            OutputSignatureChunk outputSignature,
            BytecodeContainer inputShader)
        {
            var inputRegisterDeclarations = inputShader.Shader.DeclarationTokens
                .OfType<PixelShaderInputRegisterDeclarationToken>()
                .ToList();

            // Only create a binding if the value is actually used in the next shader.
            // We can gather this information from the declaration tokens. If there is
            // no declaration token for an item, then it isn't used.
            var bindings = new List<ShaderOutputInputBinding>();
            foreach (var inputParameter in inputShader.InputSignature.Parameters)
            {
                // Get register declaration for this parameter. If there is no register
                // declaration, then it isn't used, and we can move on.
                var inputRegisterDeclaration = FindInputRegisterDeclaration(
                    inputRegisterDeclarations, inputParameter);
                if (inputRegisterDeclaration == null)
                    continue;

                bindings.Add(new ShaderOutputInputBinding
                {
                    Register = (int) inputParameter.Register,
                    ComponentMask = inputParameter.Mask,
                    InterpolationMode = inputRegisterDeclaration.InterpolationMode,
                    SystemValueType = inputParameter.SystemValueType
                });
            }

            return new ShaderOutputInputBindings
            {
                Bindings = bindings.ToArray()
            };
        }

        private static PixelShaderInputRegisterDeclarationToken FindInputRegisterDeclaration(
            IEnumerable<PixelShaderInputRegisterDeclarationToken> inputRegisterDeclarations, 
            SignatureParameterDescription inputParameter)
        {
            return inputRegisterDeclarations.FirstOrDefault(x => 
                x.Operand.Indices[0].Value == inputParameter.Register &&
                    x.Operand.ComponentMask == inputParameter.ReadWriteMask);
        }
    }
}