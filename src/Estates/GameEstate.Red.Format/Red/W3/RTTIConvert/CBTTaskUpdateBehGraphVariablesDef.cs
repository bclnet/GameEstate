using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskUpdateBehGraphVariablesDef : IBehTreeTaskDefinition
	{
		[Ordinal(1)] [RED("updateOnlyOnActivate")] 		public CBool UpdateOnlyOnActivate { get; set;}

		[Ordinal(2)] [RED("DistanceToTarget")] 		public CBool DistanceToTarget { get; set;}

		[Ordinal(3)] [RED("AngleToTarget")] 		public CBool AngleToTarget { get; set;}

		[Ordinal(4)] [RED("TargetIsOnGround")] 		public CBool TargetIsOnGround { get; set;}

		[Ordinal(5)] [RED("predictionDelay")] 		public CFloat PredictionDelay { get; set;}

		[Ordinal(6)] [RED("useCombatTarget")] 		public CBool UseCombatTarget { get; set;}

		public CBTTaskUpdateBehGraphVariablesDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskUpdateBehGraphVariablesDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}