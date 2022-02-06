using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class DiffuseHook : AnimationHook
    {
        public readonly float Start;
        public readonly float End;
        public readonly float Time;

        public DiffuseHook(BinaryReader r) : base(r)
        {
            Start = r.ReadSingle();
            End = r.ReadSingle();
            Time = r.ReadSingle();
        }
    }
}
