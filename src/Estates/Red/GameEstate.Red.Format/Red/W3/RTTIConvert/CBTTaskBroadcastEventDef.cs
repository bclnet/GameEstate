using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskBroadcastEventDef : IBehTreeTaskDefinition
	{
		[Ordinal(1)] [RED("eventName")] 		public CName EventName { get; set;}

		[Ordinal(2)] [RED("lifetime")] 		public CFloat Lifetime { get; set;}

		[Ordinal(3)] [RED("distance")] 		public CFloat Distance { get; set;}

		[Ordinal(4)] [RED("broadcastInterval")] 		public CFloat BroadcastInterval { get; set;}

		[Ordinal(5)] [RED("recipientCount")] 		public CInt32 RecipientCount { get; set;}

		[Ordinal(6)] [RED("broadcastScene")] 		public CBool BroadcastScene { get; set;}

		[Ordinal(7)] [RED("skipInvoker")] 		public CBool SkipInvoker { get; set;}

		public CBTTaskBroadcastEventDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskBroadcastEventDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}