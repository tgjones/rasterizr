namespace MSBuild.Fxc
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class CompileShader : Task
    {
        private string _fxcPath = @"C:\Program Files (x86)\Windows Kits\8.0\bin\x86\fxc.exe";
        private string _entryMethod = @"main";

        [Required]
        public ITaskItem[] InputFiles { get; set; }

        [Required]
        public string OutputPath { get; set; }

        public string FxcPath { get { return _fxcPath; } set { _fxcPath = value; } }

        public string EntryMethod { get { return _entryMethod; } set { _entryMethod = value; } }

        public string Profile { get; set; }

        public override bool Execute()
        {
            var result = ValidateParameters();
            if (!result)
                return false;

            result = ProcessFiles();

            return result;
        }

        private bool ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(FxcPath) || !File.Exists(FxcPath))
            {
                Log.LogError("fxc.exe was not found at: {0}", FxcPath);
                return false;
            }

            if (string.IsNullOrWhiteSpace(EntryMethod))
            {
                Log.LogError("Entry method should not be null or whitespace.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EntryMethod))
            {
                Log.LogError("Output path should not be null or whitespace.");
                return false;
            }

            return true;
        }

        private bool ProcessFiles()
        {
            var result = true;

            foreach (var file in InputFiles)
                result = result && ProcessFile(file);

            return result;
        }

        private bool ProcessFile(ITaskItem item)
        {
            var result = true;
            LogItemInformation(item);

            try
            {
                var itemPath = GetItemPath(item);
                var resultPath = GetItemOutputPath(itemPath);

                if (OutputExists(itemPath, resultPath))
                {
                    Log.LogMessage("Exists: {0} -> {1}", itemPath, resultPath);
                    return true;
                }

                Log.LogMessage("Compiling: {0} -> {1}", itemPath, resultPath);

                result = DoCompile(itemPath, resultPath);
            }
            catch (Exception ex)
            {
                result = false;
                Log.LogError(ex.ToString());
                //Log.LogErrorFromException(ex);
            }

            return result;
        }

        private void LogItemInformation(ITaskItem item)
        {
            try
            {
                var m = item.MetadataNames
                    .OfType<string>()
                    .Select(x => string.Format("{0}={1}", x, item.GetMetadata(x)))
                    .ToList();

                var metadata = m.Count > 0 ? string.Join("; ", m) : string.Empty;
                Log.LogMessage(MessageImportance.Low, "Item: {0}, Metadata: {1}", item.ItemSpec, metadata);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
            }
        }

        private string GetItemPath(ITaskItem item)
        {
            return item.ItemSpec;
        }

        private string GetItemOutputPath(string itemPath)
        {
            var fileExtension = Path.GetExtension(itemPath) + "o";
            return Path.Combine(OutputPath, Path.ChangeExtension(itemPath, fileExtension));
        }

        private bool OutputExists(string input, string output)
        {
            if (!File.Exists(output)) return false;

            var inputTime = File.GetLastWriteTimeUtc(input);
            var outputTime = File.GetLastWriteTimeUtc(output);
            return outputTime >= inputTime;
        }

        private bool DoCompile(string input, string output)
        {
            var outputDirectory = Path.GetDirectoryName(output);
            if (!string.IsNullOrWhiteSpace(outputDirectory) && !Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            if (File.Exists(output))
                File.Delete(output);

            var arguments = BuildFxcArguments(input, output);

            var info = new ProcessStartInfo(FxcPath, arguments)
                           {
                               CreateNoWindow = true,
                               ErrorDialog = false,
                               RedirectStandardOutput = true,
                               RedirectStandardError = true,
                               UseShellExecute = false
                           };

            var proc = Process.Start(info);
            var processResult = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            if (proc.ExitCode != 0)
                ParseAndLogCompilationError(input, processResult, proc.ExitCode);

            return proc.ExitCode == 0;
        }

        private string BuildFxcArguments(string input, string output)
        {
            var prefix = Path.GetExtension(input);

            var profileInfo = ShaderProfile.GetInfo(prefix);

            var profileString = profileInfo.BuildShaderProfileString(Profile, Path.GetFileNameWithoutExtension(input));

            var sb = new StringBuilder();
            sb.AppendFormat(" /nologo");
            sb.AppendFormat(" /T{0}", profileString);
            sb.AppendFormat(" /E{0}", EntryMethod);
            sb.AppendFormat(" /Fo \"{0}\"", output);
            sb.AppendFormat(" \"{0}\"", input);

            return sb.ToString();
        }

        private void ParseAndLogCompilationError(string inputFile, string processOutput, int errorCode)
        {
            var firstLine = processOutput
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(firstLine))
            {
                Log.LogError(processOutput);
                return;
            }

            try
            {
                var match = Regex.Match(firstLine, @"\((\d+),\d+(-\d+)?\)");
                var capture = match.Groups.OfType<Capture>().ElementAt(1).Value;
                Log.LogMessage(capture);
                var line = int.Parse(capture);

                Log.LogError(string.Empty, errorCode.ToString(), string.Empty, inputFile, line, 0, 0, 0, firstLine);
            }
            catch
            {
                Log.LogError(firstLine);
            }
        }
    }
}
