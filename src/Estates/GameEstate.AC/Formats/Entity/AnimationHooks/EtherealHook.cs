using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class EtherealHook : AnimationHook
    {
        public readonly int Ethereal;

        public EtherealHook(BinaryReader r) : base(r)
        {
            Ethereal = r.ReadInt32();
        }
    }
}
