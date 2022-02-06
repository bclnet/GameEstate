using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class ScaleHook : AnimationHook
    {
        public readonly float End;
        public readonly float Time;

        public ScaleHook(BinaryReader r) : base(r)
        {
            End = r.ReadSingle();
            Time = r.ReadSingle();
        }
    }
}
