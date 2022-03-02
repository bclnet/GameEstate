using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkCompiledBones : Chunk     //  0xACDC0000:  Bones info
    {
        public string RootBoneName; // Controller ID?  Name?  Not sure yet.
        public CompiledBone RootBone; // First bone in the data structure.  Usually Bip01
        public int NumBones; // Number of bones in the chunk
        // Bones are a bit different than Node Chunks, since there is only one CompiledBones Chunk, and it contains all the bones in the model.
        public Dictionary<int, CompiledBone> BoneDictionary = new Dictionary<int, CompiledBone>(); // Dictionary of all the CompiledBone objects based on parent offset(?).
        public List<CompiledBone> BoneList = new List<CompiledBone>();

        public CompiledBone GetParentBone(CompiledBone bone, int boneIndex) => bone.offsetParent != 0 ? BoneDictionary[boneIndex + bone.offsetParent] : null; // Should only be one parent.

        public List<CompiledBone> GetAllChildBones(CompiledBone bone) => bone.numChildren > 0 ? BoneList.Where(a => bone.childIDs.Contains(a.ControllerID)).ToList() : null;

        public List<string> GetBoneNames() => BoneList.Select(a => a.boneName).ToList(); // May need to replace space in bone names with _.

        protected void AddChildIDToParent(CompiledBone bone)
        {
            // Root bone parent ID will be zero.
            if (bone.parentID != 0) BoneList.Where(a => a.ControllerID == bone.parentID).FirstOrDefault()?.childIDs.Add(bone.ControllerID); // Should only be one parent.
        }

        protected Matrix4x4 GetTransformFromParts(Vector3 localTranslation, Matrix3x3 localRotation) => new Matrix4x4
        {
            // Translation part
            M14 = localTranslation.X,
            M24 = localTranslation.Y,
            M34 = localTranslation.Z,
            // Rotation part
            M11 = localRotation.M11,
            M12 = localRotation.M12,
            M13 = localRotation.M13,
            M21 = localRotation.M21,
            M22 = localRotation.M22,
            M23 = localRotation.M23,
            M31 = localRotation.M31,
            M32 = localRotation.M32,
            M33 = localRotation.M33,
            // Set final row
            M41 = 0,
            M42 = 0,
            M43 = 0,
            M44 = 1
        };

        protected void SetRootBoneLocalTransformMatrix()
        {
            RootBone.LocalTransform.M11 = RootBone.boneToWorld.boneToWorld[0, 0];
            RootBone.LocalTransform.M12 = RootBone.boneToWorld.boneToWorld[0, 1];
            RootBone.LocalTransform.M13 = RootBone.boneToWorld.boneToWorld[0, 2];
            RootBone.LocalTransform.M14 = RootBone.boneToWorld.boneToWorld[0, 3];
            RootBone.LocalTransform.M21 = RootBone.boneToWorld.boneToWorld[1, 0];
            RootBone.LocalTransform.M22 = RootBone.boneToWorld.boneToWorld[1, 1];
            RootBone.LocalTransform.M23 = RootBone.boneToWorld.boneToWorld[1, 2];
            RootBone.LocalTransform.M24 = RootBone.boneToWorld.boneToWorld[1, 3];
            RootBone.LocalTransform.M31 = RootBone.boneToWorld.boneToWorld[2, 0];
            RootBone.LocalTransform.M32 = RootBone.boneToWorld.boneToWorld[2, 1];
            RootBone.LocalTransform.M33 = RootBone.boneToWorld.boneToWorld[2, 2];
            RootBone.LocalTransform.M34 = RootBone.boneToWorld.boneToWorld[2, 3];
            RootBone.LocalTransform.M41 = 0;
            RootBone.LocalTransform.M42 = 0;
            RootBone.LocalTransform.M43 = 0;
            RootBone.LocalTransform.M44 = 1;
        }

        public override void WriteChunk()
        {
            Log($"*** START CompiledBone Chunk ***");
            Log($"    ChunkType:           {ChunkType}");
            Log($"    Node ID:             {ID:X}");
        }
    }
}
