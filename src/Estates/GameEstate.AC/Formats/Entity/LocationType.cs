using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class LocationType
    {
        public readonly int PartId;
        public readonly Frame Frame;

        public LocationType(BinaryReader r)
        {
            PartId = r.ReadInt32();
            Frame = new Frame(r);
        }
        
        public override string ToString() => $"PartID: {PartId}, Frame: {Frame}";
    }
}
