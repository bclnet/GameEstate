using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CMaterialBlockSamplerTextureDetail : CMaterialBlockSampler
	{
		[Ordinal(1)] [RED("scale")] 		public Vector Scale { get; set;}

		[Ordinal(2)] [RED("BlendStartDistance")] 		public CFloat BlendStartDistance { get; set;}

		[Ordinal(3)] [RED("BlendEndDistance")] 		public CFloat BlendEndDistance { get; set;}

		public CMaterialBlockSamplerTextureDetail(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CMaterialBlockSamplerTextureDetail(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}