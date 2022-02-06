using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using GameEstate.Red.Formats.Red.Types.BufferStructs;
using GameEstate.Red.Formats.Red.Types.BufferStructs.Complex;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CSectorData : ISerializable
    {
        [Ordinal(1000), REDBuffer] public CUInt64 Unknown1 { get; set; }
        [Ordinal(1001), REDBuffer] public CBufferVLQInt32<CSectorDataResource> Resources { get; set; }
        [Ordinal(1002), REDBuffer] public CBufferVLQInt32<CSectorDataObject> Objects { get; set; }
        [Ordinal(1003), REDBuffer(true)] public CVLQInt32 blocksize { get; set; }
        [Ordinal(1004), REDBuffer(true)] public CCompressedBuffer<SBlockData> BlockData { get; set; }

        public CSectorData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            blocksize = new CVLQInt32(cr2w, this, nameof(blocksize)) { IsSerialized = true };
            BlockData = new CCompressedBuffer<SBlockData>(cr2w, this, nameof(BlockData)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            blocksize.Read(r, 1);
            for (var i = 0; i < Objects.elements.Count; i++)
            {
                var curobj = Objects[i];
                var curoffset = curobj.offset.val;
                var type = curobj.type.val;
                if (!(type == 0x1 || type == 0x2))
                {
                    //System.Diagnostics.Debugger.Break();
                    //throw new NotImplementedException();
                }

                ulong len;
                if (i < Objects.elements.Count - 1)
                {
                    var nextobj = Objects[i + 1];
                    var nextoffset = nextobj.offset.val;
                    len = nextoffset - curoffset;
                }
                else
                    len = (ulong)blocksize.val - curoffset;

                var blockdata = new SBlockData(cr2w, BlockData, "")
                {
                    packedObjectType = (Enums.BlockDataObjectType)curobj.type.val
                };
                blockdata.Read(r, (uint)len);
                BlockData.AddVariable(blockdata);
            }
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            byte[] buffer;
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                BlockData.Write(bw);
                blocksize.val = (int)ms.Length;
                buffer = ms.ToArray();
            }
            blocksize.Write(w);
            w.Write(buffer);
        }

        public override string ToString() => "";
    }
}