using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class SpellComponentBase
    {
        public readonly string Name;
        public readonly uint Category;
        public readonly uint Icon;
        public readonly uint Type;
        public readonly uint Gesture;
        public readonly float Time;
        public readonly string Text;
        public readonly float CDM; // Unsure what this is

        public SpellComponentBase(BinaryReader r)
        {
            Name = r.ReadObfuscatedString();
            r.AlignBoundary();
            Category = r.ReadUInt32();
            Icon = r.ReadUInt32();
            Type = r.ReadUInt32();
            Gesture = r.ReadUInt32();
            Time = r.ReadSingle();
            Text = r.ReadObfuscatedString();
            r.AlignBoundary();
            CDM = r.ReadSingle();
        }
    }
}
