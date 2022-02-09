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

        AnimationHook() => HookType = AnimationHookType.AnimationDone;
        /// <summary>
        /// WARNING: If you're reading a hook from the dat, you should use AnimationHook.ReadHook(reader).
        /// If you read a hook from the dat using this function, it is likely you will not read all the data correctly.
        /// </summary>
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
            return hookType switch
            {
                AnimationHookType.Sound => new SoundHook(r),
                AnimationHookType.SoundTable => new SoundTableHook(r),
                AnimationHookType.Attack => new AttackHook(r),
                AnimationHookType.ReplaceObject => new ReplaceObjectHook(r),
                AnimationHookType.Ethereal => new EtherealHook(r),
                AnimationHookType.TransparentPart => new TransparentPartHook(r),
                AnimationHookType.Luminous => new LuminousHook(r),
                AnimationHookType.LuminousPart => new LuminousPartHook(r),
                AnimationHookType.Diffuse => new DiffuseHook(r),
                AnimationHookType.DiffusePart => new DiffusePartHook(r),
                AnimationHookType.Scale => new ScaleHook(r),
                AnimationHookType.CreateParticle => new CreateParticleHook(r),
                AnimationHookType.DestroyParticle => new DestroyParticleHook(r),
                AnimationHookType.StopParticle => new StopParticleHook(r),
                AnimationHookType.NoDraw => new NoDrawHook(r),
                AnimationHookType.DefaultScriptPart => new DefaultScriptPartHook(r),
                AnimationHookType.CallPES => new CallPESHook(r),
                AnimationHookType.Transparent => new TransparentHook(r),
                AnimationHookType.SoundTweaked => new SoundTweakedHook(r),
                AnimationHookType.SetOmega => new SetOmegaHook(r),
                AnimationHookType.TextureVelocity => new TextureVelocityHook(r),
                AnimationHookType.TextureVelocityPart => new TextureVelocityPartHook(r),
                AnimationHookType.SetLight => new SetLightHook(r),
                AnimationHookType.CreateBlockingParticle => new CreateBlockingParticle(r),
                // The following HookTypes have no additional properties:
                AnimationHookType.AnimationDone => new AnimationHook(r),
                AnimationHookType.DefaultScript => new AnimationHook(r),
                _ => throw new FormatException($"Not Implemented Hook type encountered: {hookType}"),
            };
        }

        //: Entity.AnimationHook
        public override string ToString() => $"HookType: {HookType}, Dir: {Direction}";
    }
}
