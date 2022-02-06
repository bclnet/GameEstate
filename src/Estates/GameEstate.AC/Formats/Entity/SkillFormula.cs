using GameEstate.AC.Formats.Props;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class SkillFormula
    {
        public readonly uint W;
        public readonly uint X;
        public readonly uint Y;
        public readonly uint Z;
        public readonly uint Attr1;
        public readonly uint Attr2;

        public SkillFormula() { }
        public SkillFormula(PropertyAttribute attr1, PropertyAttribute attr2, uint divisor)
        {
            Attr1 = (uint)attr1;
            Attr2 = (uint)attr2;
            Z = divisor;
            X = 1;
        }
        public SkillFormula(BinaryReader r)
        {
            W = r.ReadUInt32();
            X = r.ReadUInt32();
            Y = r.ReadUInt32();
            Z = r.ReadUInt32();
            Attr1 = r.ReadUInt32();
            Attr2 = r.ReadUInt32();
        }
    }
}
