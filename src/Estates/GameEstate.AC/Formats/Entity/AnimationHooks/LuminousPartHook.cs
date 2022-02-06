using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class LuminousPartHook : AnimationHook
    {
        public readonly uint Part;
        public readonly float Start;
        public readonly float End;
        public readonly float Time;

        public LuminousPartHook(BinaryReader r) : base(r)
        {
            Part = r.ReadUInt32();
            Start = r.ReadSingle();
            End = r.ReadSingle();
            Time = r.ReadSingle();
        }
    }
}
