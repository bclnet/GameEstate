using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CDeltaAnimationCompression : IAnimationCompression
	{
		[Ordinal(1)] [RED("quantizationBits")] 		public CUInt8 QuantizationBits { get; set;}

		[Ordinal(2)] [RED("positionTolerance")] 		public CFloat PositionTolerance { get; set;}

		[Ordinal(3)] [RED("rotationTolerance")] 		public CFloat RotationTolerance { get; set;}

		[Ordinal(4)] [RED("scaleTolerance")] 		public CFloat ScaleTolerance { get; set;}

		public CDeltaAnimationCompression(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CDeltaAnimationCompression(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}