using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class DeltaDamper : CObject
	{
		[Ordinal(1)] [RED("destValue")] 		public CFloat DestValue { get; set;}

		[Ordinal(2)] [RED("currValue")] 		public CFloat CurrValue { get; set;}

		[Ordinal(3)] [RED("dampFactor")] 		public CFloat DampFactor { get; set;}

		public DeltaDamper(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new DeltaDamper(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}