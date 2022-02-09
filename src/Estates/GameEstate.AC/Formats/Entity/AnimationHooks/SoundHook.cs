using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class SoundHook : AnimationHook
    {
        public readonly uint Id;

        public SoundHook(BinaryReader r) : base(r)
            => Id = r.ReadUInt32();
    }
}
