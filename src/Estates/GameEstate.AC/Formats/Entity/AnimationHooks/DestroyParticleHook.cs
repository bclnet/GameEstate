using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class DestroyParticleHook : AnimationHook
    {
        public readonly uint EmitterId;

        public DestroyParticleHook(BinaryReader r) : base(r)
        {
            EmitterId = r.ReadUInt32();
        }
    }
}
