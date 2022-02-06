using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskCombatStyleDodge : CBTTaskDodge
	{
		[Ordinal(1)] [RED("parentCombatStyle")] 		public CEnum<EBehaviorGraph> ParentCombatStyle { get; set;}

		[Ordinal(2)] [RED("humanCombatDataStorage")] 		public CHandle<CHumanAICombatStorage> HumanCombatDataStorage { get; set;}

		public CBTTaskCombatStyleDodge(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskCombatStyleDodge(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}