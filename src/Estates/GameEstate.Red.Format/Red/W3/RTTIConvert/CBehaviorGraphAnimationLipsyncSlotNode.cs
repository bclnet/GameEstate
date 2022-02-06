using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBehaviorGraphAnimationLipsyncSlotNode : CBehaviorGraphAnimationBaseSlotNode
	{
		[Ordinal(1)] [RED("cachedBaseAnimInputNode")] 		public CPtr<CBehaviorGraphNode> CachedBaseAnimInputNode { get; set;}

		[Ordinal(2)] [RED("cachedAdditiveAnimInputNode")] 		public CPtr<CBehaviorGraphNode> CachedAdditiveAnimInputNode { get; set;}

		public CBehaviorGraphAnimationLipsyncSlotNode(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBehaviorGraphAnimationLipsyncSlotNode(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}