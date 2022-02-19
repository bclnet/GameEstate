using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskBoatGrab : IBehTreeTask
	{
		[Ordinal(1)] [RED("m_Collided")] 		public CBool M_Collided { get; set;}

		[Ordinal(2)] [RED("m_TargetBoat")] 		public CHandle<CEntity> M_TargetBoat { get; set;}

		[Ordinal(3)] [RED("m_ClosestSlot")] 		public CName M_ClosestSlot { get; set;}

		public CBTTaskBoatGrab(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskBoatGrab(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}