using System;

namespace Rasterizr.Toolkit.Effects
{
    internal static class BasicEffectCode
    {
        public static readonly byte[] VertexShaderCode = new byte[2];
        public static readonly byte[] PixelShaderCode = new byte[2];

        static BasicEffectCode()
        {
            VertexShaderCode = ReadEmbeddedResource("BasicEffect.hlsl.vso");
            PixelShaderCode = ReadEmbeddedResource("BasicEffect.hlsl.pso");
        }

        private static byte[] ReadEmbeddedResource(string resourceName)
        {
            var assembly = typeof(BasicEffectCode).Assembly;
            using (var stream = assembly.GetManifestResourceStream(typeof(BasicEffectCode), resourceName))
            {
                if (stream == null)
                    throw new Exception("Could not find embedded shader:" + resourceName);
                var result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                return result;
            }
        }
    }
}