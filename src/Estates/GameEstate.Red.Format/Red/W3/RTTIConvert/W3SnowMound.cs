using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class W3SnowMound : CInteractiveEntity
	{
		[Ordinal(1)] [RED("TagRemovedAfterMelt")] 		public CName TagRemovedAfterMelt { get; set;}

		[Ordinal(2)] [RED("isMelted")] 		public CBool IsMelted { get; set;}

		public W3SnowMound(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new W3SnowMound(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}