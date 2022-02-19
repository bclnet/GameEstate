using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CAIArachasCombatLogicParams : CAIMonsterCombatLogicParams
	{
		[Ordinal(1)] [RED("minChargeDist")] 		public CFloat MinChargeDist { get; set;}

		[Ordinal(2)] [RED("maxChargeDist")] 		public CFloat MaxChargeDist { get; set;}

		public CAIArachasCombatLogicParams(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CAIArachasCombatLogicParams(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}