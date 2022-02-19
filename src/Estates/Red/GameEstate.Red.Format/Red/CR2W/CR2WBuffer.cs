using System.IO;
using System.Runtime.InteropServices;

namespace GameEstate.Red.Formats.Red.CR2W
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct CR2WBuffer
    {
        [FieldOffset(0)] public uint flags;
        [FieldOffset(4)] public uint index;
        [FieldOffset(8)] public uint offset;
        [FieldOffset(12)] public uint diskSize;
        [FieldOffset(16)] public uint memSize;
        [FieldOffset(20)] public uint crc32;
    }

    public class CR2WBufferWrapper
    {
        public CR2WBuffer Buffer;
        public byte[] Data;

        public CR2WBufferWrapper() => Buffer = new CR2WBuffer();
        public CR2WBufferWrapper(CR2WBuffer buffer) => Buffer = buffer;

        public void SetOffset(uint offset) => Buffer.offset = offset;

        public void ReadData(BinaryReader r)
        {
            r.Position(Buffer.offset);
            Data = r.ReadBytes((int)Buffer.memSize);
        }

        public void WriteData(BinaryWriter r)
        {
            Buffer.offset = (uint)r.Position();
            if (Data != null)
            {
                r.Write(Data);
                Buffer.memSize = (uint)Data.Length;
            }
        }

        public override string ToString() => Buffer.index.ToString();
    }
}