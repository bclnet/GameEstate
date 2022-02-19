using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class BTCondAttackedDelay : IBehTreeTask
	{
		[Ordinal(1)] [RED("delay")] 		public CFloat Delay { get; set;}

		[Ordinal(2)] [RED("wasHit")] 		public CBool WasHit { get; set;}

		[Ordinal(3)] [RED("completeIfAttacked")] 		public CBool CompleteIfAttacked { get; set;}

		public BTCondAttackedDelay(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new BTCondAttackedDelay(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}