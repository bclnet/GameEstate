using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBehaviorGraphDefensiveComboStateNode : CBehaviorGraphComboStateNode
	{
		[Ordinal(1)] [RED("varHitTime")] 		public CName VarHitTime { get; set;}

		[Ordinal(2)] [RED("varLevel")] 		public CName VarLevel { get; set;}

		[Ordinal(3)] [RED("varParry")] 		public CName VarParry { get; set;}

		[Ordinal(4)] [RED("defaultHits", 2,0)] 		public CArray<CName> DefaultHits { get; set;}

		public CBehaviorGraphDefensiveComboStateNode(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBehaviorGraphDefensiveComboStateNode(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}