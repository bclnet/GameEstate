using GameEstate.AC.Formats.FileTypes;
using GameEstate.Formats;

namespace GameEstate.AC
{
    public class LanguagePakFile
    {
        public LanguagePakFile(EstatePakFile pakFile)
        {
            //CharacterTitles = ReadFromDat<StringTable>(StringTable.CharacterTitle_FileID);
        }

        public StringTable CharacterTitles { get; }
    }
}
