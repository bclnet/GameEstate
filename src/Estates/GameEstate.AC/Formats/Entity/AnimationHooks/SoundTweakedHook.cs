using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class SoundTweakedHook : AnimationHook
    {
        public readonly uint SoundID;
        public readonly float Priority;
        public readonly float Probability;
        public readonly float Volume;

        public SoundTweakedHook(BinaryReader r) : base(r)
        {
            SoundID = r.ReadUInt32();
            Priority = r.ReadSingle();
            Probability = r.ReadSingle();
            Volume = r.ReadSingle();
        }
    }
}
