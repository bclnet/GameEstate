using GameEstate.AC.Formats.FileTypes;
using GameEstate.Formats;

namespace GameEstate.AC
{
    public class DatabaseLanguage : Database
    {
        public DatabaseLanguage(EstatePakFile pakFile) : base(pakFile)
        {
            CharacterTitles = ReadFile<StringTable>(StringTable.CharacterTitle_FileID);
        }

        public StringTable CharacterTitles { get; }
    }
}
