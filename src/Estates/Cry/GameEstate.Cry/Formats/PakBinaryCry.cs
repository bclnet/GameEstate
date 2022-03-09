using GameEstate.Formats;

namespace GameEstate.Cry.Formats
{
    public class PakBinaryCry : PakBinarySystemZip
    {
        internal PakBinaryCry(byte[] key) : base(key) { }
    }
}