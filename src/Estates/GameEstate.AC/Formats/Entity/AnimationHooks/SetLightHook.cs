using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class SetLightHook : AnimationHook
    {
        public readonly int LightsOn;

        public SetLightHook(BinaryReader r) : base(r)
            => LightsOn = r.ReadInt32();
    }
}
