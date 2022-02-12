using GameEstate.AC.Formats.FileTypes;
using System;

namespace GameEstate.AC
{
    public class Database
    {
        EstatePakFile pakFile;

        public Database(EstatePakFile pakFile) => this.pakFile = pakFile;

        //public T Files => pakFile.

        public T ReadFile<T>(uint fileId) where T : FileType
        {
            throw new NotImplementedException();
        }
    }
}
