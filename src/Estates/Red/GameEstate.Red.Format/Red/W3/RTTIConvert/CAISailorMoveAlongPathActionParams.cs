using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CAISailorMoveAlongPathActionParams : ISailorActionParameters
	{
		[Ordinal(1)] [RED("boatTag")] 		public CName BoatTag { get; set;}

		[Ordinal(2)] [RED("pathTag")] 		public CName PathTag { get; set;}

		[Ordinal(3)] [RED("upThePath")] 		public CBool UpThePath { get; set;}

		[Ordinal(4)] [RED("startFromBeginning")] 		public CBool StartFromBeginning { get; set;}

		public CAISailorMoveAlongPathActionParams(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CAISailorMoveAlongPathActionParams(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}