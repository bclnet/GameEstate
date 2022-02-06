using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class LightInfo
    {
        public readonly Frame ViewerSpaceLocation;
        public readonly uint Color; // _RGB Color. Red is bytes 3-4, Green is bytes 5-6, Blue is bytes 7-8. Bytes 1-2 are always FF (?)
        public readonly float Intensity;
        public readonly float Falloff;
        public readonly float ConeAngle;

        public LightInfo(BinaryReader r)
        {
            ViewerSpaceLocation = new Frame(r);
            Color = r.ReadUInt32();
            Intensity = r.ReadSingle();
            Falloff = r.ReadSingle();
            ConeAngle = r.ReadSingle();
        }

        public override string ToString() => $"Viewer Space Location: {ViewerSpaceLocation}, Color: {Color}, Intensity: {Intensity}, Falloff: {Falloff}, Cone Angle: {ConeAngle}";
    }
}
