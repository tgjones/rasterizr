namespace MSBuild.Fxc
{
    using System;
    using System.Linq;

    internal class ShaderProfileInfo
    {
        public readonly string Name;
        public readonly string Prefix;
        public readonly string[] Versions;
        public readonly string DefaultVersion;

        public ShaderProfileInfo(string name, string prefix, params string[] versions)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (prefix == null) throw new ArgumentNullException("prefix");
            if (versions.Length < 1)

                Name = name;
            Prefix = prefix;
            Versions = versions;
            DefaultVersion = versions[0];
        }

        public string BuildShaderProfileString(string filename)
        {
            var version = Versions.FirstOrDefault(filename.Contains) ?? DefaultVersion;
            return string.Format("{0}_{1}", Prefix, version);
        }
    }
}