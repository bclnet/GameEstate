using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CMaterialBlockOutputColorDeferred : CMaterialRootBlock
	{
		[Ordinal(1)] [RED("isTwoSided")] 		public CBool IsTwoSided { get; set;}

		[Ordinal(2)] [RED("rawOutput")] 		public CBool RawOutput { get; set;}

		[Ordinal(3)] [RED("maskThreshold")] 		public CFloat MaskThreshold { get; set;}

		[Ordinal(4)] [RED("terrain")] 		public CBool Terrain { get; set;}

		public CMaterialBlockOutputColorDeferred(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CMaterialBlockOutputColorDeferred(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}