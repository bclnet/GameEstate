using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class ReplaceObjectHook : AnimationHook
    {
        public readonly AnimationPartChange APChange;

        public ReplaceObjectHook(BinaryReader r) : base(r)
            => APChange = new AnimationPartChange(r, r.ReadUInt16());
    }
}
