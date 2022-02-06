using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class NoDrawHook : AnimationHook
    {
        public readonly uint NoDraw;

        public NoDrawHook(BinaryReader r) : base(r)
        {
            NoDraw = r.ReadUInt32();
        }
    }
}
