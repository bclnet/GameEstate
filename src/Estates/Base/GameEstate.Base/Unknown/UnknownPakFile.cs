using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Unknown
{
    /// <summary>
    /// UnknownPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class UnknownPakFile : EstatePakFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="name">The name.</param>
        public UnknownPakFile(Estate estate, string game, string name)
            : base(estate, game, name) { }
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