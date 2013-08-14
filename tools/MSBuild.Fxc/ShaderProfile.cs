using System;

namespace MSBuild.Fxc
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class ShaderProfile
    {
        private static readonly IList<ShaderProfileInfo> Profiles;

        static ShaderProfile()
        {
            Profiles = new List<ShaderProfileInfo>();

            Profiles.Add(new ShaderProfileInfo("EffectShader", "fx", "4_0", "4_1", "5_0"));

            Profiles.Add(new ShaderProfileInfo("VertexShader", "vs", "4_0_level_9_3", "1_1", "2_0", "2_a", "2_sw", "3_0", "3_sw", "4_0",
                                                "4_0_level_9_0", "4_0_level_9_1", "4_1", "5_0"));

            Profiles.Add(new ShaderProfileInfo("PixelShader", "ps", "4_0_level_9_3", "1_1", "2_0", "2_a", "2_b", "2_sw", "3_0", "3_sw", "4_0",
                                                "4_0_level_9_0", "4_0_level_9_1", "4_1", "5_0"));

            Profiles.Add(new ShaderProfileInfo("ComputeShader", "cs", "4_0", "4_1", "5_0"));

            Profiles.Add(new ShaderProfileInfo("GeometryShader", "gs", "4_0", "4_1", "5_0"));

            Profiles.Add(new ShaderProfileInfo("DomainShader", "ds", "5_0"));

            Profiles.Add(new ShaderProfileInfo("HullShader", "ds", "5_0"));

            Profiles.Add(new ShaderProfileInfo("TextureShader", "tx", "1_0"));
        }

        internal static ShaderProfileInfo GetInfo(string extension)
        {
            var result = Profiles.FirstOrDefault(x => "." + x.Prefix == extension);
            if (result == null)
                throw new Exception("No profile found for extension '" + extension + "'.");
            return result;
        }
    }
}