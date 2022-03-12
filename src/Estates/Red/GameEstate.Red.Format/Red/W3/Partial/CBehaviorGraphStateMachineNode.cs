using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CBehaviorGraphStateMachineNode : CBehaviorGraphContainerNode
    {
        [Ordinal(1), RED("globalTransitions", 2, 0)] public CArray<CPtr<CBehaviorGraphStateTransitionGlobalBlendNode>> GlobalTransitions { get; set; }
        [Ordinal(2), RED("resetStateOnExit")] public CBool ResetStateOnExit { get; set; }
        [Ordinal(3), RED("applySyncTags")] public CBool ApplySyncTags { get; set; }

        public CBehaviorGraphStateMachineNode(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBehaviorGraphStateMachineNode(cr2w, parent, name);

        public override void Read(BinaryReader file, uint size) => base.Read(file, size);

        public override void Write(BinaryWriter file) => base.Write(file);
    }
}