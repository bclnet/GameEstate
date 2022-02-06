using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class SSoundAmbientDynamicSoundEvents : CVariable
	{
		[Ordinal(1)] [RED("eventName")] 		public StringAnsi EventName { get; set;}

		[Ordinal(2)] [RED("repeatTime")] 		public CFloat RepeatTime { get; set;}

		[Ordinal(3)] [RED("repeatTimeVariance")] 		public CFloat RepeatTimeVariance { get; set;}

		[Ordinal(4)] [RED("triggerOnActivation")] 		public CBool TriggerOnActivation { get; set;}

		public SSoundAmbientDynamicSoundEvents(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SSoundAmbientDynamicSoundEvents(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}