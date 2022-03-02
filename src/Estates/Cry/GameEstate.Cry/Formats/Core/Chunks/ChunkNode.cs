using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkNode : Chunk // cccc000b:   Node
    {
        /// <summary>
        /// Chunk Name (String[64])
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// Mesh or Helper Object ID
        /// </summary>
        public int ObjectNodeID { get; internal set; }
        /// <summary>
        /// Node parent.  if 0xFFFFFFFF, it's the top node.  Maybe...
        /// </summary>
        public int ParentNodeID { get; internal set; }  // Parent nodeID
        public int __NumChildren;
        /// <summary>
        /// Material ID for this chunk
        /// </summary>
        public int MatID { get; internal set; }
        public bool IsGroupHead { get; internal set; }
        public bool IsGroupMember { get; internal set; }
        /// <summary>
        /// Transformation Matrix
        /// </summary>
        public Matrix4x4 Transform { get; internal set; }
        /// <summary>
        /// Position vector of Transform
        /// </summary>
        public Vector3 Pos { get; internal set; }
        /// <summary>
        /// Rotation component of Transform
        /// </summary>
        public Quaternion Rot { get; internal set; }
        /// <summary>
        /// Scalar component of Transform
        /// </summary>
        public Vector3 Scale { get; internal set; }
        /// <summary>
        /// Position Controller ID - Obsolete
        /// </summary>
        public int PosCtrlID { get; internal set; }
        /// <summary>
        /// Rotation Controller ID - Obsolete
        /// </summary>
        public int RotCtrlID { get; internal set; }
        /// <summary>
        /// Scalar Controller ID - Obsolete
        /// </summary>
        public int SclCtrlID { get; internal set; }
        /// <summary>
        /// Appears to be a Blob of properties, separated by new lines
        /// </summary>
        public string Properties { get; internal set; }

        // Calculated Properties

        public Matrix4x4 LocalTransform = new Matrix4x4();              // Because Cryengine tends to store transform relative to world, we have to add all the transforms from the node to the root.  Calculated, row major.
        public Vector3 LocalTranslation = new Vector3();                // To hold the local translation vector
        public Matrix3x3 LocalRotation = new Matrix3x3();               // to hold the local rotation matrix
        public Vector3 LocalScale = new Vector3();                      // to hold the local scale matrix

        ChunkNode _parentNode;
        public ChunkNode ParentNode
        {
            get
            {
                // Turns out chunk IDs are ints, not uints.  ~0 is shorthand for -1, or 0xFFFFFFFF in the uint world.
                if (ParentNodeID == ~0) return null;
                if (_parentNode == null)
                {
                    if (_model.ChunkMap.ContainsKey(ParentNodeID)) _parentNode = _model.ChunkMap[ParentNodeID] as ChunkNode;
                    else _parentNode = _model.RootNode;
                }
                return _parentNode;
            }
            set
            {
                ParentNodeID = value == null ? ~0 : value.ID;
                _parentNode = value;
            }
        }

        public List<ChunkNode> ChildNodes { get; set; }

        Chunk _objectChunk;
        public Chunk ObjectChunk
        {
            get
            {
                if (_objectChunk == null && _model.ChunkMap.ContainsKey(ObjectNodeID)) _objectChunk = _model.ChunkMap[ObjectNodeID];
                return _objectChunk;
            }
            set => _objectChunk = value;
        }

        public Vector3 TransformSoFar => ParentNode != null
            ? ParentNode.TransformSoFar + Transform.GetTranslation()
            : Transform.GetTranslation();

        public Matrix3x3 RotSoFar => ParentNode != null
            ? Transform.GetRotation() * ParentNode.RotSoFar
            : _model.RootNode.Transform.GetRotation();

        public List<ChunkNode> AllChildNodes => __NumChildren == 0 ? null : _model.NodeMap.Values.Where(a => a.ParentNodeID == ID).ToList();

        /// <summary>
        /// Gets the transform of the vertex.  This will be both the rotation and translation of this vertex, plus all the parents.
        /// The transform matrix is a 4x4 matrix.  Vector3 is a 3x1.  We need to convert vector3 to vector4, multiply the matrix, then convert back to vector3.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 GetTransform(Vector3 transform)
        {
            var vec3 = transform;
            // Apply the local transforms (rotation and translation) to the vector
            // Do rotations.  Rotations must come first, then translate.
            vec3 = RotSoFar * vec3;
            // Do translations.  I think this is right.  Objects in right place, not rotated right.
            vec3 += TransformSoFar;
            return vec3;
        }

        public override void Read(BinaryReader r)
        {
            base.Read(r);
            // Read the Name string
            Name = r.ReadFString(64);
            ObjectNodeID = r.ReadInt32(); // Object reference ID
            ParentNodeID = r.ReadInt32();
            __NumChildren = r.ReadInt32();
            MatID = r.ReadInt32();  // Material ID?
            SkipBytes(r, 4);
            // Read the 4x4 transform matrix.  Should do a couple of for loops, but data structures...
            Transform = new Matrix4x4
            {
                M11 = r.ReadSingle(),
                M12 = r.ReadSingle(),
                M13 = r.ReadSingle(),
                M14 = r.ReadSingle(),
                M21 = r.ReadSingle(),
                M22 = r.ReadSingle(),
                M23 = r.ReadSingle(),
                M24 = r.ReadSingle(),
                M31 = r.ReadSingle(),
                M32 = r.ReadSingle(),
                M33 = r.ReadSingle(),
                M34 = r.ReadSingle(),
                M41 = r.ReadSingle(),
                M42 = r.ReadSingle(),
                M43 = r.ReadSingle(),
                M44 = r.ReadSingle(),
            };
            // Read the position Pos Vector3
            Pos = new Vector3
            {
                X = r.ReadSingle() / 100,
                Y = r.ReadSingle() / 100,
                Z = r.ReadSingle() / 100,
            };
            // Read the rotation Rot Quad
            Rot = new Quaternion
            {
                W = r.ReadSingle(),
                X = r.ReadSingle(),
                Y = r.ReadSingle(),
                Z = r.ReadSingle(),
            };
            // Read the Scale Vector 3
            Scale = new Vector3
            {
                X = r.ReadSingle(),
                Y = r.ReadSingle(),
                Z = r.ReadSingle(),
            };
            // read the controller pos/rot/scale
            PosCtrlID = r.ReadInt32();
            RotCtrlID = r.ReadInt32();
            SclCtrlID = r.ReadInt32();
            Properties = r.ReadPString();
        }

        public override void WriteChunk()
        {
            Log($"*** START Node Chunk ***");
            Log($"    ChunkType:           {ChunkType}");
            Log($"    Node ID:             {ID:X}");
            Log($"    Node Name:           {Name}");
            Log($"    Object ID:           {ObjectNodeID:X}");
            Log($"    Parent ID:           {ParentNodeID:X}");
            Log($"    Number of Children:  {__NumChildren}");
            Log($"    Material ID:         {MatID:X}"); // 0x1 is mtllib w children, 0x10 is mtl no children, 0x18 is child
            Log($"    Position:            {Pos.X:F7}   {Pos.Y:F7}   {Pos.Z:F7}");
            Log($"    Scale:               {Scale.X:F7}   {Scale.Y:F7}   {Scale.Z:F7}");
            LogFormat("    Transformation:      {0:F7}  {1:F7}  {2:F7}  {3:F7}", Transform.M11, Transform.M12, Transform.M13, Transform.M14);
            LogFormat("                         {0:F7}  {1:F7}  {2:F7}  {3:F7}", Transform.M21, Transform.M22, Transform.M23, Transform.M24);
            LogFormat("                         {0:F7}  {1:F7}  {2:F7}  {3:F7}", Transform.M31, Transform.M32, Transform.M33, Transform.M34);
            LogFormat("                         {0:F7}  {1:F7}  {2:F7}  {3:F7}", Transform.M41 / 100, Transform.M42 / 100, Transform.M43 / 100, Transform.M44);
            Log($"    Transform_sum:       {TransformSoFar.X:F7}  {TransformSoFar.Y:F7}  {TransformSoFar.Z:F7}");
            Log($"    Rotation_sum:");
            RotSoFar.LogMatrix3x3();
            Log($"*** END Node Chunk ***");
        }
    }
}
