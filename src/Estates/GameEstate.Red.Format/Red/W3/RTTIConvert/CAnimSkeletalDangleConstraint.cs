using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CAnimSkeletalDangleConstraint : IAnimDangleConstraint
	{
		[Ordinal(1)] [RED("skeleton")] 		public CHandle<CSkeleton> Skeleton { get; set;}

		[Ordinal(2)] [RED("dispSkeleton")] 		public CBool DispSkeleton { get; set;}

		[Ordinal(3)] [RED("dispBoneNames")] 		public CBool DispBoneNames { get; set;}

		[Ordinal(4)] [RED("dispBoneAxis")] 		public CBool DispBoneAxis { get; set;}

		public CAnimSkeletalDangleConstraint(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CAnimSkeletalDangleConstraint(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}