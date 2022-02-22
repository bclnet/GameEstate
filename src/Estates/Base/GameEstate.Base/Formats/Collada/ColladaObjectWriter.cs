using GameEstate.Formats.Generic;
using grendgine_collada;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using static GameEstate.EstateDebug;

namespace GameEstate.Formats.Collada
{
    /// <summary>
    /// export to .dae format (COLLADA)
    /// </summary>
    /// <seealso cref="Generic.GenericObjectWriter" />
    public partial class ColladaObjectWriter : GenericObjectWriter
    {
        public ColladaObjectWriter(IGenericFile file) : base(file) { }

        public FileInfo ModelFile;

        public XmlSchema schema = new XmlSchema // Get the schema from kronos.org. Needs error checking in case it's offline
        {
            ElementFormDefault = XmlSchemaForm.Qualified,
            TargetNamespace = "https://www.khronos.org/files/collada_schema_1_5",
        };

        public Grendgine_Collada daeObject = new Grendgine_Collada // This is the serializable class.
        {
            //Collada_Version = "1.5.0", // Blender doesn't like 1.5. :(
            Collada_Version = "1.4.1",
        };

        public override void Write(string outputDir = null, bool preservePath = true)
        {
            // The root of the functions to write Collada files
            // At this point, we should have a cryData.Asset object, fully populated.
            Log("*** Starting WriteCOLLADA() ***");

            Log($"Number of models: {File.Models.Count()}");
            foreach (var model in File.Models) Log($"\tNumber of nodes in model: {model.NodeMap.Count}");
            OutputTest();

            // File name will be "object name.dae"
            ModelFile = GetFileInfo("dae", outputDir, preservePath);

            SetAsset();
            SetLibraryImages();
            SetScene();
            SetLibraryEffects();
            SetLibraryMaterials();
            SetLibraryGeometries();
            // If there is Skinning info, create the controller library and set up visual scene to refer to it. Otherwise just write the Visual Scene
            if (File.SkinningInfo.HasSkinningInfo)
            {
                SetLibraryControllers();
                SetLibraryVisualScenesWithSkeleton();
            }
            else SetLibraryVisualScenes();

            // Make the StreamWriter and Serializes the daeObject
            if (!ModelFile.Directory.Exists) ModelFile.Directory.Create();
            using (var w = new StreamWriter(ModelFile.FullName)) new XmlSerializer(typeof(Grendgine_Collada)).Serialize(w, daeObject);

            // Validate that the Collada document is ok
#if false
            ValidateXml(); // validates against the schema
            ValidateDoc(); // validates IDs and URLs
#endif
            Log("End of Write Collada. Export complete.");
        }

        /// <summary>
        /// Retrieves the worldtobone (bind pose matrix) for the bone.
        /// </summary>
        /// <param name="compiledBones">List of bones to get the BPM from.</param>
        /// <returns>The float_array that represents the BPM of all the bones, in order.</returns>
        string GetBindPoseArray(ICollection<IGenericBone> compiledBones)
        {
            var b = new StringBuilder();
            foreach (var compiledBone in compiledBones) b.Append(CreateStringFromMatrix44(compiledBone.WorldToBone.GetMatrix44()) + " ");
            return b.ToString().TrimEnd();
        }

        string GetRootBoneName(ChunkCompiledBones bones) => bones.RootBone.boneName;


        void CalculateTransform(ChunkNode node)
        {
            var localTranslation = node.Transform.GetScale();
            var localRotation = node.Transform.GetRotation();
            var localScale = node.Transform.GetTranslation();
            node.LocalTranslation = localTranslation;
            node.LocalScale = localScale;
            node.LocalRotation = localRotation;
            node.LocalTransform = node.LocalTransform.GetTransformFromParts(localScale, localRotation, localTranslation);
        }

        string CreateStringFromMatrix44(Matrix4x4 matrix)
        {
            var b = new StringBuilder();
            b.AppendFormat("{0:F6} {1:F6} {2:F6} {3:F6} {4:F6} {5:F6} {6:F6} {7:F6} {8:F6} {9:F6} {10:F6} {11:F6} {12:F6} {13:F6} {14:F6} {15:F6}",
                matrix.m00,
                matrix.m01,
                matrix.m02,
                matrix.m03,
                matrix.m10,
                matrix.m11,
                matrix.m12,
                matrix.m13,
                matrix.m20,
                matrix.m21,
                matrix.m22,
                matrix.m23,
                matrix.m30,
                matrix.m31,
                matrix.m32,
                matrix.m33);
            CleanNumbers(b);
            return b.ToString();
        }

        /// <summary>Takes the Material file name and returns just the file name with no extension</summary>
        string CleanMtlFileName(string cleanMe)
        {
            var stringSeparators = new string[] { "\\", "/" };
            string[] f;
            // Take out path info
            if (cleanMe.Contains("/") || cleanMe.Contains("\\")) { f = cleanMe.Split(stringSeparators, StringSplitOptions.None); cleanMe = f[^1]; }
            // Check to see if extension is added, and if so strip it out. Look for "."
            if (cleanMe.Contains(".")) { var periodSep = new string[] { @"." }; f = cleanMe.Split(periodSep, StringSplitOptions.None); cleanMe = f[0]; }
            //Console.WriteLine("Cleanme is {0}", cleanMe);
            return cleanMe;
        }

        //double safe(double value) =>
        //    value == double.NegativeInfinity
        //        ? double.MinValue
        //        : value == double.PositiveInfinity ? double.MaxValue : value == double.NaN ? 0 : value;

        void CleanNumbers(StringBuilder b) =>
            b.Replace("0.000000", "0")
            .Replace("-0.000000", "0")
            .Replace("1.000000", "1")
            .Replace("-1.000000", "-1");
    }
}