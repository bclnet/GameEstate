using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CR4HudModuleTimeLapse : CR4HudModuleBase
	{
		[Ordinal(1)] [RED("m_fxSetShowTimeSFF")] 		public CHandle<CScriptedFlashFunction> M_fxSetShowTimeSFF { get; set;}

		[Ordinal(2)] [RED("m_fxSetTimeLapseMessage")] 		public CHandle<CScriptedFlashFunction> M_fxSetTimeLapseMessage { get; set;}

		[Ordinal(3)] [RED("m_fxSetTimeLapseAdditionalMessage")] 		public CHandle<CScriptedFlashFunction> M_fxSetTimeLapseAdditionalMessage { get; set;}

		public CR4HudModuleTimeLapse(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CR4HudModuleTimeLapse(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}