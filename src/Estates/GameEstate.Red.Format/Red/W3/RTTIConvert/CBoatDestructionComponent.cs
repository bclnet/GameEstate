using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBoatDestructionComponent : CComponent
	{
		[Ordinal(1)] [RED("autoGeneratedVolumesX")] 		public CUInt32 AutoGeneratedVolumesX { get; set;}

		[Ordinal(2)] [RED("autoGeneratedVolumesY")] 		public CUInt32 AutoGeneratedVolumesY { get; set;}

		[Ordinal(3)] [RED("autoGeneratorVolumesResizer")] 		public CFloat AutoGeneratorVolumesResizer { get; set;}

		[Ordinal(4)] [RED("destructionVolumes", 2,0)] 		public CArray<SBoatDestructionVolume> DestructionVolumes { get; set;}

		[Ordinal(5)] [RED("boatComponent")] 		public CHandle<CBoatComponent> BoatComponent { get; set;}

		[Ordinal(6)] [RED("collisionForceThreshold")] 		public CFloat CollisionForceThreshold { get; set;}

		[Ordinal(7)] [RED("partsConfig", 2,0)] 		public CArray<SBoatPartsConfig> PartsConfig { get; set;}

		[Ordinal(8)] [RED("attachedSirens", 2,0)] 		public CArray<CHandle<CActor>> AttachedSirens { get; set;}

		[Ordinal(9)] [RED("freeSirenGrabSlots", 2,0)] 		public CArray<CName> FreeSirenGrabSlots { get; set;}

		[Ordinal(10)] [RED("lockedSirenGrabSlots", 2,0)] 		public CArray<CName> LockedSirenGrabSlots { get; set;}

		public CBoatDestructionComponent(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBoatDestructionComponent(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}