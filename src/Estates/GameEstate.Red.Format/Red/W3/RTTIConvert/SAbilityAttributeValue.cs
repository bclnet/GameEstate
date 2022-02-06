using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class SAbilityAttributeValue : CVariable
	{
		[Ordinal(1)] [RED("valueAdditive")] 		public CFloat ValueAdditive { get; set;}

		[Ordinal(2)] [RED("valueMultiplicative")] 		public CFloat ValueMultiplicative { get; set;}

		[Ordinal(3)] [RED("valueBase")] 		public CFloat ValueBase { get; set;}

		public SAbilityAttributeValue(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SAbilityAttributeValue(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}