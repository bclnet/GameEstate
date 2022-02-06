using System.IO;

namespace GameEstate.AC.Formats.Entity.AnimationHooks
{
    public class AttackHook : AnimationHook
    {
        public readonly AttackCone AttackCone;

        public AttackHook(BinaryReader r) : base(r)
        {
            AttackCone = new AttackCone(r);
        }
    }
}
