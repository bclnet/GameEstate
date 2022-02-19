using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CPointLightComponent : CLightComponent
	{
		[Ordinal(1)] [RED("cacheStaticShadows")] 		public CBool CacheStaticShadows { get; set;}

		[Ordinal(2)] [RED("dynamicShadowsFaceMask")] 		public CEnum<ELightCubeSides> DynamicShadowsFaceMask { get; set;}

		public CPointLightComponent(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CPointLightComponent(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}