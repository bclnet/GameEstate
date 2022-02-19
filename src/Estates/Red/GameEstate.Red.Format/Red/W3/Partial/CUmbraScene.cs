using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CUmbraScene : CResource
	{
		[Ordinal(1)] [RED("distanceMultiplier")] 		public CFloat DistanceMultiplier { get; set;}

		[Ordinal(2)] [RED("localUmbraOccThresholdMul")] 		public CHandle<CResourceSimplexTree> LocalUmbraOccThresholdMul { get; set;}

		public CUmbraScene(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CUmbraScene(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}