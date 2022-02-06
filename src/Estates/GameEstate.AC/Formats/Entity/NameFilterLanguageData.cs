using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class NameFilterLanguageData
    {
        public readonly uint MaximumVowelsInARow; 
        public readonly uint FirstNCharactersMustHaveAVowel;
        public readonly uint VowelContainingSubstringLength;
        public readonly uint ExtraAllowedCharacters;
        public readonly byte Unknown;
        public readonly string[] CompoundLetterGroups;

        public NameFilterLanguageData(BinaryReader r)
        {
            MaximumVowelsInARow = r.ReadUInt32();
            FirstNCharactersMustHaveAVowel = r.ReadUInt32();
            VowelContainingSubstringLength = r.ReadUInt32();
            ExtraAllowedCharacters = r.ReadUInt32();
            Unknown = r.ReadByte();
            CompoundLetterGroups = r.ReadL32Array(x => x.ReadUnicodeString());
        }
    }
}
