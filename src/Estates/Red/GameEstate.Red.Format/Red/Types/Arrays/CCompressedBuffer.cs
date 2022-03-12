using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta]
    public class CCompressedBuffer<T> : CBufferBase<T> where T : CVariable
    {
        public CCompressedBuffer(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CCompressedBuffer<T>(cr2w, parent, name);

        public new void Read(BinaryReader file, uint size, int count) => base.Read(file, size, count);

        /// <summary>
        /// This should not be called for CCompressedBuffers. Call Read(BinaryReader file, uint size, int count) instead.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="size"></param>
        public override void Read(BinaryReader r, uint size) => throw new NotImplementedException();

        public override void Write(BinaryWriter w) => base.Write(w);
    }
}