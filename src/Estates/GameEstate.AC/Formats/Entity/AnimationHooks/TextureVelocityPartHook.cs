using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class TextureVelocityPartHook : AnimationHook
    {
        public readonly uint PartIndex;
        public readonly float USpeed;
        public readonly float VSpeed;

        public TextureVelocityPartHook(BinaryReader r) : base(r)
        {
            PartIndex = r.ReadUInt32();
            USpeed = r.ReadSingle();
            VSpeed = r.ReadSingle();
        }
    }
}
