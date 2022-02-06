using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CR4PreparationPotionsAndBombsMenu : CR4MenuBase
	{
		[Ordinal(1)] [RED("_gridInv")] 		public CHandle<W3GuiPreparationPotionsAndBombsInventoryComponent> _gridInv { get; set;}

		[Ordinal(2)] [RED("_currentInv")] 		public CHandle<W3GuiBaseInventoryComponent> _currentInv { get; set;}

		[Ordinal(3)] [RED("_inv")] 		public CHandle<CInventoryComponent> _inv { get; set;}

		[Ordinal(4)] [RED("optionsItemActions", 2,0)] 		public CArray<CEnum<EInventoryActionType>> OptionsItemActions { get; set;}

		[Ordinal(5)] [RED("_currentQuickSlot")] 		public CEnum<EEquipmentSlots> _currentQuickSlot { get; set;}

		[Ordinal(6)] [RED("ITEMS_SIZE")] 		public CInt32 ITEMS_SIZE { get; set;}

		public CR4PreparationPotionsAndBombsMenu(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CR4PreparationPotionsAndBombsMenu(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}