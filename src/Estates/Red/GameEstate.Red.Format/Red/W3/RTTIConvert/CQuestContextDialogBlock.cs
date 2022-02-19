using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CQuestContextDialogBlock : CQuestGraphBlock
	{
		[Ordinal(1)] [RED("scene")] 		public CSoft<CStoryScene> Scene { get; set;}

		[Ordinal(2)] [RED("targetScene")] 		public CSoft<CStoryScene> TargetScene { get; set;}

		public CQuestContextDialogBlock(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CQuestContextDialogBlock(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}