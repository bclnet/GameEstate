using GameEstate.Formats;

namespace GameEstate.Cry.Formats
{
    public class PakBinaryCry : PakBinarySystemZip
    {
        public static readonly PakBinary Instance = new PakBinaryCry();

        PakBinaryCry() : base() { }
    }
}