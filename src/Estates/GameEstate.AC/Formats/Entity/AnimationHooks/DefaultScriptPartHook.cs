using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class DefaultScriptPartHook : AnimationHook
    {
        public readonly uint PartIndex;

        public DefaultScriptPartHook(BinaryReader r) : base(r)
        {
            PartIndex = r.ReadUInt32();
        }
    }
}
