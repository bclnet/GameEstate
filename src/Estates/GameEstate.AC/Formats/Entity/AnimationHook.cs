using GameEstate.AC.Formats.Entity.AnimationHooks;
using GameEstate.AC.Formats.Props;
using System;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class AnimationHook
    {
        public static readonly AnimationHook AnimDoneHook = new AnimationHook();
        public readonly AnimationHookType HookType;
        public readonly AnimationHookDir Direction;

        /// <summary>
        /// WARNING: If you're reading a hook from the dat, you should use AnimationHook.ReadHook(reader).
        /// If you read a hook from the dat using this function, it is likely you will not read all the data correctly.
        /// </summary>
        AnimationHook()
        {
            HookType = AnimationHookType.AnimationDone;
        }
        public AnimationHook(BinaryReader r)
        {
            HookType = (AnimationHookType)r.ReadUInt32();
            Direction = (AnimationHookDir)r.ReadInt32();
        }

        public static AnimationHook Factory(BinaryReader r)
        {
            // We peek forward to get the hook type, then revert our position.
            var hookType = (AnimationHookType)r.ReadUInt32();
            r.BaseStream.Position -= 4;
            switch (hookType)
            {
                case AnimationHookType.Sound: return new SoundHook(r);
                case AnimationHookType.SoundTable: return new SoundTableHook(r);
                case AnimationHookType.Attack: return new AttackHook(r);
                case AnimationHookType.ReplaceObject: return new ReplaceObjectHook(r);
                case AnimationHookType.Ethereal: return new EtherealHook(r);
                case AnimationHookType.TransparentPart: return new TransparentPartHook(r);
                case AnimationHookType.Luminous: return new LuminousHook(r);
                case AnimationHookType.LuminousPart: return new LuminousPartHook(r);
                case AnimationHookType.Diffuse: return new DiffuseHook(r);
                case AnimationHookType.DiffusePart: return new DiffusePartHook(r);
                case AnimationHookType.Scale: return new ScaleHook(r);
                case AnimationHookType.CreateParticle: return new CreateParticleHook(r);
                case AnimationHookType.DestroyParticle: return new DestroyParticleHook(r);
                case AnimationHookType.StopParticle: return new StopParticleHook(r);
                case AnimationHookType.NoDraw: return new NoDrawHook(r);
                case AnimationHookType.DefaultScriptPart: return new DefaultScriptPartHook(r);
                case AnimationHookType.CallPES: return new CallPESHook(r);
                case AnimationHookType.Transparent: return new TransparentHook(r);
                case AnimationHookType.SoundTweaked: return new SoundTweakedHook(r);
                case AnimationHookType.SetOmega: return new SetOmegaHook(r);
                case AnimationHookType.TextureVelocity: return new TextureVelocityHook(r);
                case AnimationHookType.TextureVelocityPart: return new TextureVelocityPartHook(r);
                case AnimationHookType.SetLight: return new SetLightHook(r);
                case AnimationHookType.CreateBlockingParticle: return new CreateBlockingParticle(r);
                // The following HookTypes have no additional properties:
                // AnimationHookType.AnimationDone
                // AnimationHookType.DefaultScript
                case AnimationHookType.AnimationDone:
                case AnimationHookType.DefaultScript: return new AnimationHook(r);
                default: Console.WriteLine($"Not Implemented Hook type encountered: {hookType}"); return null;
            }
        }

        public override string ToString() => $"HookType: {HookType}, Dir: {Direction}";
    }
}
