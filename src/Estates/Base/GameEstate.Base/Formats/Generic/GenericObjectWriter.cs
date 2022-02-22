using System.IO;

namespace GameEstate.Formats.Generic
{
    public abstract class GenericObjectWriter
    {
        // ARGS
        public DirectoryInfo DataDir = null;
        public const bool NoConflicts = false;
        public const bool TiffTextures = false;
        public const bool SkipShieldNodes = false;
        public const bool SkipStreamNodes = false;
        public const bool GroupMeshes = true;
        public const bool Smooth = true;

        public IGenericFile File { get; internal set; }

        public GenericObjectWriter(IGenericFile file) => File = file;

        public abstract void Write(string outputDir = null, bool preservePath = true);

        protected FileInfo GetFileInfo(string extension, string outputDir = null, bool preservePath = true)
        {
            var fileName = $"temp.{extension}";
            // Empty output directory means place alongside original models If you want relative path, use "."
            if (string.IsNullOrWhiteSpace(outputDir)) fileName = Path.Combine(new FileInfo(File.Path).DirectoryName, $"{Path.GetFileNameWithoutExtension(File.Path)}{(NoConflicts ? "_out" : string.Empty)}.{extension}");
            else
            {
                // If we have an output directory
                var preserveDir = preservePath ? Path.GetDirectoryName(File.Path) : string.Empty;
                // Remove drive letter if necessary
                if (!string.IsNullOrWhiteSpace(preserveDir) && !string.IsNullOrWhiteSpace(Path.GetPathRoot(preserveDir))) preserveDir = preserveDir.Replace(Path.GetPathRoot(preserveDir), string.Empty);
                fileName = Path.Combine(outputDir, preserveDir, Path.ChangeExtension(Path.GetFileNameWithoutExtension(File.Path), extension));
            }
            return new FileInfo(fileName);
        }
    }
}
