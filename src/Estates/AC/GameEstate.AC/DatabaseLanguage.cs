using GameEstate.AC.Formats.FileTypes;

namespace GameEstate.AC
{
    public class DatabaseLanguage : Database
    {
        public DatabaseLanguage(EstatePakFile pakFile) : base(pakFile)
        {
            CharacterTitles = GetFile<StringTable>(StringTable.CharacterTitle_FileID);
        }

        public StringTable CharacterTitles { get; }
    }
}
