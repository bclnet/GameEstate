using System.IO;

namespace GameEstate.Red.Formats.Red.CR2W
{
    public class CR2WHeaderData
    {
        public uint crc;
        public uint offset;
        public uint size;

        public CR2WHeaderData() { }
        public CR2WHeaderData(BinaryReader r) => Read(r); 

        public void Read(BinaryReader r)
        {
            offset = r.ReadUInt32();
            size = r.ReadUInt32();
            crc = r.ReadUInt32();
        }

        public void Write(BinaryWriter r)
        {
            if (size == 0)
            {
                offset = 0;
                crc = 0;
            }
            r.Write(offset);
            r.Write(size);
            r.Write(crc);
        }
    }
}