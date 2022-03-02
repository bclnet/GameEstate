using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate
{
    internal class SomePlatform
    {
        public static bool Startup() => true;
    }

    internal static class Some
    {
        public const string EstateJson =
@"{
    'id': 'Some',
    'name': 'Some Estate',
    'pakFileType': 'GameEstate.Some+PakFile, GameEstateTests',
    'games': {
        'Found': {
            'name': 'Found',
            'pak': 'game:/path#Found'
        },
        'Missing': {
            'name': 'Missing',
            'pak': 'game:/path#Missing'
        }
    },
    'fileManager': {
    }
}";
        public static readonly Estate Estate = EstateManager.ParseEstate(EstateJson.Replace("'", "\""));

        public class PakFile : EstatePakFile
        {
            public PakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, "Some Name") { }
            public override void Dispose() { }
            public override int Count => 0;
            public override void Close() { }
            public override bool Contains(string path) => false;
            public override bool Contains(int fileId) => false;
            public override Task<Stream> LoadFileDataAsync(string path, Action<FileMetadata, string> exception = null) => throw new NotImplementedException();
            public override Task<Stream> LoadFileDataAsync(int fileId, Action<FileMetadata, string> exception = null) => throw new NotImplementedException();
            public override Task<T> LoadFileObjectAsync<T>(string path, Action<FileMetadata, string> exception = null) => throw new NotImplementedException();
            public override Task<T> LoadFileObjectAsync<T>(int fileId, Action<FileMetadata, string> exception = null) => throw new NotImplementedException();
        }
    }
}
