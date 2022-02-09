using System.IO;
using System.Numerics;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class SetOmegaHook : AnimationHook
    {
        public readonly Vector3 Axis;

        public SetOmegaHook(BinaryReader r) : base(r)
            => Axis = r.ReadVector3();
    }
}
