using GameEstate.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEstate
{
    /// <summary>
    /// Estate
    /// </summary>
    public class Estate
    {
        static unsafe Estate()
        {
            foreach (var startup in EstatePlatform.Startups) if (startup()) return;
            EstatePlatform.Platform = EstatePlatform.PlatformUnknown;
            EstatePlatform.GraphicFactory = source => throw new Exception("No GraphicFactory");
            EstateDebug.AssertFunc = x => System.Diagnostics.Debug.Assert(x);
            EstateDebug.LogFunc = a => System.Diagnostics.Debug.Print(a);
            EstateDebug.LogFormatFunc = (a, b) => System.Diagnostics.Debug.Print(a, b);
        }

        /// <summary>
        /// Touches this instance.
        /// </summary>
        public static void Bootstrap() { }

        public enum PakMultiType
        {
            SingleBinary,
            Full,
        }

        /// <summary>
        /// Resource
        /// </summary>
        public struct Resource
        {
            /// <summary>
            /// The stream pak
            /// </summary>
            public bool StreamPak;
            /// <summary>
            /// The host
            /// </summary>
            public Uri Host;
            /// <summary>
            /// The paths
            /// </summary>
            public string[] Paths;
            /// <summary>
            /// The game
            /// </summary>
            public string Game;
        }

        /// <summary>
        /// EstateGame
        /// </summary>
        public class EstateGame
        {
            /// <summary>
            /// The identifier
            /// </summary>
            public string Game { get; set; }
            /// <summary>
            /// Gets the name of the found.
            /// </summary>
            /// <summary>
            /// The name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// The default paks
            /// </summary>
            public IList<Uri> DefaultPaks { get; set; }
            /// <summary>
            /// The has location
            /// </summary>
            public bool Found { get; set; }

            public string DisplayedName => $"{Name}{(Found ? " - found" : null)}";
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
            => Name;

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The estate name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the studio.
        /// </summary>
        /// <value>
        /// The estate studio.
        /// </value>
        public string Studio { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The estate description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the type of the pak file.
        /// </summary>
        /// <value>
        /// The type of the pak file.
        /// </value>
        public Type PakFileType { get; set; }

        /// <summary>
        /// Gets or sets the pak multi.
        /// </summary>
        /// <value>
        /// The multi-pak.
        /// </value>
        public PakMultiType PakMulti { get; set; }

        /// <summary>
        /// Gets the type of the dat file.
        /// </summary>
        /// <value>
        /// The type of the pak file.
        /// </value>
        public Type Pak2FileType { get; set; }

        /// <summary>
        /// Gets or sets the pak multi.
        /// </summary>
        /// <value>
        /// The multi-pak.
        /// </value>
        public PakMultiType Pak2Multi { get; set; }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <param name="id">The game id.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">game</exception>
        public (string id, EstateGame game) GetGame(string id)
            => Games.TryGetValue(id, out var game) ? (id, game) : throw new ArgumentOutOfRangeException(nameof(id), id);

        /// <summary>
        /// Gets the estates games.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, EstateGame> Games { get; set; }

        /// <summary>
        /// Gets the estates file manager.
        /// </summary>
        /// <value>
        /// The file manager.
        /// </value>
        public FileManager FileManager { get; set; }

        /// <summary>
        /// Parses the estates resource.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public Resource ParseResource(Uri uri)
            => FileManager.ParseResource(this, uri);

        #region Pak

        static EstatePakFile WithPlatformGraphic(EstatePakFile pakFile)
        {
            pakFile.Graphic = EstatePlatform.GraphicFactory(pakFile);
            return pakFile;
        }

        /// <summary>
        /// Opens the estates pak file.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public EstatePakFile OpenPakFile(string[] filePaths, string game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (PakFileType == null || filePaths.Length == 0) return null;
            return PakMulti switch
            {
                PakMultiType.SingleBinary => filePaths.Length == 1
                    ? WithPlatformGraphic((EstatePakFile)Activator.CreateInstance(PakFileType, this, game, filePaths[0], null))
                    : WithPlatformGraphic(new MultiPakFile(this, game, "Many", filePaths.Select(x => (EstatePakFile)Activator.CreateInstance(PakFileType, this, game, x, null)).ToArray())),
                PakMultiType.Full => WithPlatformGraphic((EstatePakFile)Activator.CreateInstance(PakFileType, this, game, filePaths)),
                _ => throw new ArgumentOutOfRangeException(nameof(PakMulti), PakMulti.ToString()),
            };
        }

        /// <summary>
        /// Opens the estates pak file.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public EstatePakFile OpenPakFile(Resource resource)
            => !resource.StreamPak
            ? OpenPakFile(resource.Paths, resource.Game)
            : WithPlatformGraphic(new MultiPakFile(this, resource.Game, "Many", resource.Paths.Select(x => new StreamPakFile(FileManager.HostFactory, this, resource.Game, x, resource.Host)).ToArray()));

        /// <summary>
        /// Opens the estates pak file.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="many">if set to <c>true</c> [many].</param>
        /// <returns></returns>
        public EstatePakFile OpenPakFile(Uri uri)
            => OpenPakFile(FileManager.ParseResource(this, uri));

        #endregion
    }
}