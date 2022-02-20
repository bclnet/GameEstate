using GameEstate.Formats;
using System.Collections.Concurrent;
using static GameEstate.EstateDebug;

namespace GameEstate.Tes
{
    public class Database
    {
        public readonly BinaryPakManyFile Source;

        public Database(EstatePakFile source) => Source = source as BinaryPakManyFile;

        public override string ToString() => Source.Name;

        //public ConcurrentDictionary<uint, FileType> FileCache { get; } = new ConcurrentDictionary<uint, FileType>();
    }
}
