using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class PortalPoly
    {
        public readonly short PortalIndex;
        public readonly short PolygonId;

        public PortalPoly(BinaryReader r)
        {
            PortalIndex = r.ReadInt16();
            PolygonId = r.ReadInt16();
        }

        public override string ToString() => $"PortalIdx: {PortalIndex}, PolygonId: {PolygonId}";
    }
}
