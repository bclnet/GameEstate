using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class CallPESHook : AnimationHook
    {
        public readonly uint PES;
        public readonly float Pause;

        public CallPESHook(BinaryReader r) : base(r)
        {
            PES = r.ReadUInt32();
            Pause = r.ReadSingle();
        }
    }
}
