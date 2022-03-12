using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using GameEstate.Red.Formats.Red.Types.BufferStructs;
using System.IO;
using System.Linq;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CSkeleton : CResource
    {
        [Ordinal(1000), REDBuffer(true)] public CCompressedBuffer<SSkeletonRigData> rigdata { get; set; }

        public CSkeleton(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
            => rigdata = new CCompressedBuffer<SSkeletonRigData>(cr2w, this, nameof(rigdata)) { IsSerialized = true };

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            // get bonecount for compressed buffers
            var bonecount = Bones.Count;
            rigdata.Read(r, (uint)bonecount * 48, bonecount);
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            rigdata.Write(w);
        }
    }
}