using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CEncounterGlobalSettings : CObject
	{
		[Ordinal(1)] [RED("defaultSpawnStrategy")] 		public CPtr<ISpawnTreeSpawnStrategy> DefaultSpawnStrategy { get; set;}

		[Ordinal(2)] [RED("groupLimits", 2,0)] 		public CArray<SEncounterGroupLimit> GroupLimits { get; set;}

		public CEncounterGlobalSettings(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CEncounterGlobalSettings(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}