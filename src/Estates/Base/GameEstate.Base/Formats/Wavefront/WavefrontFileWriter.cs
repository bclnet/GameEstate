using GameEstate.Formats.Unknown;
using System;
using System.IO;
using System.Reflection;

namespace GameEstate.Formats.Wavefront
{
    /// <summary>
    /// export to .obj/.mat format (WAVEFRONT)
    /// </summary>
    /// <seealso cref="UnknownFileWriter" />
    public partial class WavefrontFileWriter : UnknownFileWriter
    {
        public WavefrontFileWriter(IUnknownFileModel file) : base(file) { }

        public FileInfo ModelFile { get; internal set; }
        public FileInfo MaterialFile { get; internal set; }
        public int CurrentVertexPosition { get; internal set; }
        public int TempIndicesPosition { get; internal set; }
        public int TempVertexPosition { get; internal set; }
        public int CurrentIndicesPosition { get; internal set; }
        public string GroupOverride { get; internal set; }
        public int FaceIndex { get; internal set; }

        /// <summary>
        /// Renders an .obj file, and matching .mat file for the current model
        /// </summary>
        /// <param name="outputDir">Folder to write files to</param>
        /// <param name="preservePath">When using an <paramref name="outputDir"/>, preserve the original hierarchy</param>
        public override void Write(string outputDir = null, bool preservePath = true)
        {
            // We need to create the obj header, then for each submesh write the vertex, UV and normal data.
            // First, let's figure out the name of the output file.  Should be <object name>.obj
            // Each Mesh will have a mesh subset and a series of datastream objects.  Need temporary pointers to these so we can manipulate
            // Get object name.  This is the Root Node chunk Name
            // Get the objOutputFile name

            ModelFile = GetFileInfo("obj", outputDir, preservePath);
            MaterialFile = GetFileInfo("mtl", outputDir, preservePath);
            if (GroupMeshes) GroupOverride = Path.GetFileNameWithoutExtension(ModelFile.Name);
            Console.WriteLine(@"Output file is {0}\...\{1}", outputDir, ModelFile.Name);

            if (!ModelFile.Directory.Exists) ModelFile.Directory.Create();

            WriteMaterialFile(File);

            using var w = new StreamWriter(ModelFile.FullName);
            w.WriteLine($"# cgf-converter .obj export version {Assembly.GetExecutingAssembly().GetName().Version}");
            w.WriteLine("#");
            if (MaterialFile.Exists) w.WriteLine($"mtllib {MaterialFile.Name}");

            FaceIndex = 1;
            foreach (var mesh in File.Meshes) WriteMesh(w, mesh);

            //var nullParents = File.Nodes.Where(p => p.Parent == null).ToArray();
            //if (nullParents.Length > 1) foreach (var node in nullParents) Log($"Rendering node with null parent {node.Name}");
            //foreach (var node in File.Nodes)
            //{
            //    // Cry:Don't render shields
            //    if (SkipShieldNodes && node.Name.StartsWith("$shield")) { Log($"Skipped shields node {node.Name}"); continue; }
            //    // Cry:Don't render stream
            //    if (SkipStreamNodes && node.Name.StartsWith("stream")) { Log($"Skipped stream node {node.Name}"); continue; }
            //    if (node.Object == null) { Log($"Skipped node with missing Object {node.Name}"); continue; }
            //    switch (node.Object.Type)
            //    {
            //        case ChunkType.Mesh:
            //            // Render Meshes
            //            if (node.Parent != null && node.Parent.Type != ChunkType.Node) Log($"Rendering {node.Name} to parent {node.Parent.Name}");
            //            // Grab the mesh and process that.
            //            WriteObjNode(file, node);
            //            break;
            //        case ChunkType.Helper: break; // Ignore Helpers nodes
            //        default: Log($"Skipped a {node.Object.Type} chunk"); break; // Warn us if we're skipping other nodes of interest
            //    }
            //}

            // If this has proxies, just write out the hitbox info. OBJ files can't do armatures.
            if (File.Proxies != null) WriteHitbox(w, File.Proxies);
        }
    }
}