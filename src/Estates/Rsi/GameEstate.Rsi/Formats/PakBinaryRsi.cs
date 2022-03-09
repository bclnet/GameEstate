using GameEstate.Formats;

namespace GameEstate.Rsi.Formats
{
    /// <summary>
    /// PakBinaryRsi
    /// </summary>
    /// <seealso cref="PakBinaryP4k" />
    public class PakBinaryRsi : PakBinaryP4k
    {
        public static readonly PakBinary Instance = new PakBinaryRsi();
        static readonly byte[] Key = new byte[] { 0x5E, 0x7A, 0x20, 0x02, 0x30, 0x2E, 0xEB, 0x1A, 0x3B, 0xB6, 0x17, 0xC3, 0x0F, 0xDE, 0x1E, 0x47 };

        PakBinaryRsi() : base(Key) { }
    }
}