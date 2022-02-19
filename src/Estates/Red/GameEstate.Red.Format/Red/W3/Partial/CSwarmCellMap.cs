using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CSwarmCellMap : CResource
	{
		[Ordinal(1)] [RED("cellSize")] 		public CFloat CellSize { get; set;}

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CSwarmCellMap(cr2w, parent, name);

	}
}