using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class ReplaceObjectHook : AnimationHook
    {
        public readonly AnimationPartChange APChange;

        public ReplaceObjectHook(BinaryReader r) : base(r)
        {
            // The structure of AnimationPartChange here is slightly different for some reason than the other imeplementations.
            // So we'll read in the 2-byte PartIndex and send that to our other implementation of the Unpack function.
            var apChangePartIndex = r.ReadUInt16();
            APChange = new AnimationPartChange(r, apChangePartIndex);
        }
    }
}
