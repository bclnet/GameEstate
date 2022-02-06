using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class VirtualAnimationPoseIK : CVariable
	{
		[Ordinal(1)] [RED("time")] 		public CFloat Time { get; set;}

		[Ordinal(2)] [RED("ids", 2,0)] 		public CArray<CEnum<ETCrEffectorId>> Ids { get; set;}

		[Ordinal(3)] [RED("positionsMS", 2,0)] 		public CArray<Vector> PositionsMS { get; set;}

		[Ordinal(4)] [RED("rotationsMS", 2,0)] 		public CArray<EulerAngles> RotationsMS { get; set;}

		[Ordinal(5)] [RED("weights", 2,0)] 		public CArray<CFloat> Weights { get; set;}

		public VirtualAnimationPoseIK(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new VirtualAnimationPoseIK(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}