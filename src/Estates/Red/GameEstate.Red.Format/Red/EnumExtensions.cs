using GameEstate.Red.Formats.Red.Types;
using System;
using System.Collections.Generic;

namespace GameEstate.Red.Formats.Red
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Convert an enumerable collection of CName values into a Enum value.
        /// </summary>
        /// <param name="enumType">The enum type to convert to.</param>
        /// <param name="names">The collection of CNames</param>
        /// <returns>The Enum value</returns>
        public static object ConvertToEnum(Type enumType, IEnumerable<CName> names)
        {
            if (!enumType.IsEnum) throw new ArgumentException();
            try
            {
                var enumString = string.Join(", ", names);
                return Enum.Parse(enumType, enumString);
            }
            catch { return Enum.ToObject(enumType, 0); }
        }
    }
}