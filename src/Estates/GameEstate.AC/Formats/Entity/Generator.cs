using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class Generator
    {
        public readonly string Name;
        public readonly uint Id;
        public readonly Generator[] Items;

        public Generator(BinaryReader r)
        {
            Name = r.ReadObfuscatedString(); r.AlignBoundary();
            Id = r.ReadUInt32();
            Items = r.ReadL32Array(x => new Generator(x));
        }
    }
}
