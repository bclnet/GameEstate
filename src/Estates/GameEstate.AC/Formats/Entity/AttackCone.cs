using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class AttackCone
    {
        public readonly uint PartIndex;
        // these Left and Right are technically Vec2D types
        public readonly float LeftX; public readonly float LeftY;
        public readonly float RightX; public readonly float RightY;
        public readonly float Radius; public readonly float Height;

        public AttackCone(BinaryReader r)
        {
            PartIndex = r.ReadUInt32();
            LeftX = r.ReadSingle(); LeftY = r.ReadSingle();
            RightX = r.ReadSingle(); RightY = r.ReadSingle();
            Radius = r.ReadSingle(); Height = r.ReadSingle();
        }
    }
}
