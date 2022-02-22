using GameEstate.Formats;

namespace GameEstate.Cry.Formats
{
    public class PakBinaryCry : PakBinaryZip2
    {
        public static readonly PakBinary Instance = new PakBinaryCry();

        PakBinaryCry() : base(getObjectFactory: FormatExtensions.GetObjectFactory) { }
    }
}