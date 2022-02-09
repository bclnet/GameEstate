using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace GameEstate.AC.Formats.Props
{
    public static class PropertyExtensions
    {
        public static string GetDescription(this Enum prop) => prop.GetAttributeOfType<DescriptionAttribute>()?.Description ?? prop.ToString();

        /// <summary>
        /// Will add a space infront of capital letter words in a string
        /// </summary>
        /// <param name="attribute2nd"></param>
        /// <returns>string with spaces infront of capital letters</returns>
        public static string ToSentence(this Enum prop) => new string(prop.ToString().Replace("Max", "Maximum").ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());

        public static string GetValueEnumName(this PropertyDataId property, uint value)
        {
            switch (property)
            {
                case PropertyDataId.ActivationAnimation:
                case PropertyDataId.InitMotion:
                case PropertyDataId.UseTargetAnimation:
                case PropertyDataId.UseTargetFailureAnimation:
                case PropertyDataId.UseTargetSuccessAnimation:
                case PropertyDataId.UseUserAnimation: return Enum.GetName(typeof(MotionCommand), value);
                case PropertyDataId.PhysicsScript:
                case PropertyDataId.RestrictionEffect: return Enum.GetName(typeof(PlayScript), value);
                case PropertyDataId.ActivationSound:
                case PropertyDataId.UseSound: return Enum.GetName(typeof(Sound), value);
                case PropertyDataId.WieldedTreasureType:
                case PropertyDataId.DeathTreasureType: /*todo*/ return null;
                case PropertyDataId.Spell:
                case PropertyDataId.DeathSpell:
                case PropertyDataId.ProcSpell:
                case PropertyDataId.RedSurgeSpell:
                case PropertyDataId.BlueSurgeSpell:
                case PropertyDataId.YellowSurgeSpell: return Enum.GetName(typeof(SpellId), value);
                case PropertyDataId.PCAPRecordedParentLocation: return Enum.GetName(typeof(ParentLocation), value);
                default: return null;
            }
        }

        public static string GetValueEnumName(this PropertyInt property, int value)
        {
            switch (property)
            {
                case PropertyInt.ActivationResponse: return Enum.GetName(typeof(ActivationResponse), value);
                case PropertyInt.AetheriaBitfield: return Enum.GetName(typeof(AetheriaBitfield), value);
                case PropertyInt.AttackHeight: return Enum.GetName(typeof(AttackHeight), value);
                case PropertyInt.AttackType: return Enum.GetName(typeof(AttackType), value);
                case PropertyInt.Attuned: return Enum.GetName(typeof(AttunedStatus), value);
                case PropertyInt.AmmoType: return Enum.GetName(typeof(AmmoType), value);
                case PropertyInt.Bonded: return Enum.GetName(typeof(BondedStatus), value);
                case PropertyInt.ChannelsActive:
                case PropertyInt.ChannelsAllowed: return Enum.GetName(typeof(Channel), value);
                case PropertyInt.CombatMode: return Enum.GetName(typeof(CombatMode), value);
                case PropertyInt.DefaultCombatStyle:
                case PropertyInt.AiAllowedCombatStyle: return Enum.GetName(typeof(CombatStyle), value);
                case PropertyInt.CombatUse: return Enum.GetName(typeof(CombatUse), value);
                case PropertyInt.ClothingPriority: return Enum.GetName(typeof(CoverageMask), value);
                case PropertyInt.CreatureType:
                case PropertyInt.SlayerCreatureType:
                case PropertyInt.FoeType:
                case PropertyInt.FriendType: return Enum.GetName(typeof(CreatureType), value);
                case PropertyInt.DamageType: return Enum.GetName(typeof(DamageType), value);
                case PropertyInt.CurrentWieldedLocation:
                case PropertyInt.ValidLocations: return Enum.GetName(typeof(EquipMask), value);
                case PropertyInt.EquipmentSetId: return Enum.GetName(typeof(EquipmentSet), value);
                case PropertyInt.Gender: return Enum.GetName(typeof(Gender), value);
                case PropertyInt.GeneratorDestructionType:
                case PropertyInt.GeneratorEndDestructionType: return Enum.GetName(typeof(GeneratorDestruct), value);
                case PropertyInt.GeneratorTimeType: return Enum.GetName(typeof(GeneratorTimeType), value);
                case PropertyInt.GeneratorType: return Enum.GetName(typeof(GeneratorType), value);
                case PropertyInt.HeritageGroup: return Enum.GetName(typeof(HeritageGroup), value);
                case PropertyInt.HookType: return Enum.GetName(typeof(HookType), value);
                case PropertyInt.HouseType: return Enum.GetName(typeof(HouseType), value);
                case PropertyInt.ImbuedEffect:
                case PropertyInt.ImbuedEffect2:
                case PropertyInt.ImbuedEffect3:
                case PropertyInt.ImbuedEffect4:
                case PropertyInt.ImbuedEffect5: return Enum.GetName(typeof(ImbuedEffectType), value);
                case PropertyInt.HookItemType:
                case PropertyInt.ItemType:
                case PropertyInt.MerchandiseItemTypes:
                case PropertyInt.TargetType: return Enum.GetName(typeof(ItemType), value);
                case PropertyInt.ItemXpStyle: return Enum.GetName(typeof(ItemXpStyle), value);
                case PropertyInt.MaterialType: return Enum.GetName(typeof(MaterialType), value);
                case PropertyInt.PaletteTemplate: return Enum.GetName(typeof(PaletteTemplate), value);
                case PropertyInt.PhysicsState: return Enum.GetName(typeof(PhysicsState), value);
                case PropertyInt.HookPlacement:
                case PropertyInt.Placement:
                case PropertyInt.PCAPRecordedPlacement: return Enum.GetName(typeof(Placement), value);
                case PropertyInt.PortalBitmask: return Enum.GetName(typeof(PortalBitmask), value);
                case PropertyInt.PlayerKillerStatus: return Enum.GetName(typeof(PlayerKillerStatus), value);
                case PropertyInt.BoosterEnum: return Enum.GetName(typeof(PropertyAttribute2nd), value);
                case PropertyInt.ShowableOnRadar: return Enum.GetName(typeof(RadarBehavior), value);
                case PropertyInt.RadarBlipColor: return Enum.GetName(typeof(RadarColor), value);
                case PropertyInt.WeaponSkill:
                case PropertyInt.WieldSkillType:
                case PropertyInt.WieldSkillType2:
                case PropertyInt.WieldSkillType3:
                case PropertyInt.WieldSkillType4: return Enum.GetName(typeof(Skill), value);
                case PropertyInt.AccountRequirements: return Enum.GetName(typeof(SubscriptionStatus), value);
                case PropertyInt.SummoningMastery: return Enum.GetName(typeof(SummoningMastery), value);
                case PropertyInt.UiEffects: return Enum.GetName(typeof(UIEffects), value);
                case PropertyInt.ItemUseable: return Enum.GetName(typeof(Usable), value);
                case PropertyInt.WeaponType: return Enum.GetName(typeof(WeaponType), value);
                case PropertyInt.WieldRequirements:
                case PropertyInt.WieldRequirements2:
                case PropertyInt.WieldRequirements3:
                case PropertyInt.WieldRequirements4: return Enum.GetName(typeof(WieldRequirement), value);
                case PropertyInt.GeneratorStartTime:
                case PropertyInt.GeneratorEndTime: return DateTimeOffset.FromUnixTimeSeconds(value).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
                case PropertyInt.ArmorType: return Enum.GetName(typeof(ArmorType), value);
                case PropertyInt.ParentLocation: return Enum.GetName(typeof(ParentLocation), value);
                case PropertyInt.PlacementPosition: return Enum.GetName(typeof(Placement), value);
                case PropertyInt.HouseStatus: return Enum.GetName(typeof(HouseStatus), value);
                case PropertyInt.UseCreatesContractId: return Enum.GetName(typeof(ContractId), value);
                default: return null;
            }
        }
    }

    public class GenericPropertiesId<TAttribute> where TAttribute : Attribute
    {
        /// <summary>
        /// Method to return a list of enums by attribute type - in this case [AssessmentProperty] using generics to enhance code reuse.
        /// </summary>
        /// <typeparam name="T">Enum to list by [AssessmentProperty]</typeparam>
        /// <typeparam name="TResult">Type of the results</typeparam>
        static HashSet<TResult> GetValues<T, TResult>() =>
            new HashSet<TResult>(typeof(T).GetFields().Select(x => new
            {
                att = x.GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault(),
                member = x
            }).Where(x => x.att != null && !x.member.IsSpecialName).Select(x => (TResult)x.member.GetValue(null)));

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary
        public static HashSet<ushort> PropertiesInt = GetValues<PropertyInt, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesInt64 = GetValues<PropertyInt64, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesBool = GetValues<PropertyBool, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesString = GetValues<PropertyString, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesDouble = GetValues<PropertyFloat, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesDataId = GetValues<PropertyDataId, ushort>();

        /// <summary>
        /// returns a list of values for PropertyInt that are [AssessmentProperty]
        /// </summary>
        public static HashSet<ushort> PropertiesInstanceId = GetValues<PropertyInstanceId, ushort>();
    }

    public class GenericProperties<TAttribute> where TAttribute : Attribute
    {
        /// <summary>
        /// Method to return a list of enums by attribute type - in this case [Ephemeral] using generics to enhance code reuse.
        /// </summary>
        /// <typeparam name="T">Enum to list by [Ephemeral]</typeparam>
        /// <typeparam name="TResult">Type of the results</typeparam>
        static HashSet<T> GetValues<T>() =>
           new HashSet<T>(typeof(T).GetFields().Select(x => new
           {
               att = x.GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault(),
               member = x
           }).Where(x => x.att != null && !x.member.IsSpecialName).Select(x => (T)x.member.GetValue(null)));

        /// <summary>
        /// returns a list of values for PropertyInt that are [Ephemeral]
        /// </summary
        public static HashSet<PropertyInt> PropertiesInt = GetValues<PropertyInt>();

        /// <summary>
        /// returns a list of values for PropertyInt64 that are [Ephemeral]
        /// </summary>
        public static HashSet<PropertyInt64> PropertiesInt64 = GetValues<PropertyInt64>();

        /// <summary>
        /// returns a list of values for PropertyBool that are [Ephemeral]
        /// </summary>
        public static HashSet<PropertyBool> PropertiesBool = GetValues<PropertyBool>();

        /// <summary>
        /// returns a list of values for PropertyString that are [Ephemeral]
        /// </summary>
        public static HashSet<PropertyString> PropertiesString = GetValues<PropertyString>();

        /// <summary>
        /// returns a list of values for PropertyFloat that are [Ephemeral]
        /// </summary>
        public static HashSet<PropertyFloat> PropertiesDouble = GetValues<PropertyFloat>();

        /// <summary>
        /// returns a list of values for PropertyDataId that are [Ephemeral]
        /// </summary>
        public static HashSet<PropertyDataId> PropertiesDataId = GetValues<PropertyDataId>();

        /// <summary>
        /// returns a list of values for PropertyInstanceId that are [Ephemeral]
        /// </summary>
        public static HashSet<PropertyInstanceId> PropertiesInstanceId = GetValues<PropertyInstanceId>();

        /// <summary>
        /// returns a list of values for PositionType that are [Ephemeral]
        /// </summary>
        public static HashSet<PositionType> PositionTypes = GetValues<PositionType>();
    }
}
