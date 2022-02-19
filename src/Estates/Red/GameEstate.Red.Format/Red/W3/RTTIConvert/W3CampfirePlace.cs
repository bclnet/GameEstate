using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class W3CampfirePlace : W3CookingPlace
	{
		[Ordinal(1)] [RED("victims", 2,0)] 		public CArray<CHandle<CActor>> Victims { get; set;}

		[Ordinal(2)] [RED("bombs", 2,0)] 		public CArray<SItemUniqueId> Bombs { get; set;}

		public W3CampfirePlace(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new W3CampfirePlace(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}