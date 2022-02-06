using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class CreateParticleHook : AnimationHook
    {
        public readonly uint EmitterInfoId;
        public readonly uint PartIndex;
        public readonly Frame Offset;
        public readonly uint EmitterId;

        public CreateParticleHook(BinaryReader r) : base(r)
        {
            EmitterInfoId = r.ReadUInt32();
            PartIndex = r.ReadUInt32();
            Offset = new Frame(r);
            EmitterId = r.ReadUInt32();
        }
    }
}
