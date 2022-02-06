using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CTimerScriptKeyword : CObject
	{
		[Ordinal(1)] [RED("timeDelta")] 		public CFloat TimeDelta { get; set;}

		[Ordinal(2)] [RED("timeDeltaUnscaled")] 		public CFloat TimeDeltaUnscaled { get; set;}

		public CTimerScriptKeyword(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CTimerScriptKeyword(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}