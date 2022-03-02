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
        //public static readonly Estate Empty = new Estate
        //{
        //    Id = string.Empty,
        //    Name = "Empty",
        //    Games = new Dictionary<string, EstateGame>(),
        //    FileManager = EstateManager.CreateFileManager(),
        //};

        static unsafe Estate()
        {
            if (EstatePlatform.InTestHost && EstatePlatform.Startups.Count == 0) EstatePlatform.Startups.Add(TestPlatform.Startup);
            foreach (var startup in EstatePlatform.Startups) if (startup()) return;
            EstatePlatform.Platform = EstatePlatform.PlatformUnknown;
            EstatePlatform.GraphicFactory = source => throw new Exception("No GraphicFactory");
            EstateDebug.AssertFunc = x => System.Diagnostics.Debug.Assert(x);
            EstateDebug.LogFunc = a => System.Diagnostics.Debug.Print(a);
            EstateDebug.LogFormatFunc = (a, b) => System.Diagnostics.Debug.Print(a, b);
        }

        protected internal Estate() { }

        /// <summary>
        /// Touches this instance.
        /// </summary>
        public static void Bootstrap() { }

        /// <summary>
        /// Ensures this instance.
        /// </summary>
        public virtual Estate Ensure() => this;

        [Flags]
        public enum PakOption
        {
            Paths = 0x1,
            Stream = 0x2,
        }

        /// <summary>
        /// Resource
        /// </summary>
        public struct Resource
        {
            /// <summary>
            /// The pak options
            /// </summary>
            public PakOption Options;
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
            /// The paks
            /// </summary>
            public IList<Uri> Paks { get; set; }
            /// <summary>
            /// The dats
            /// </summary>
            public IList<Uri> Dats { get; set; }
            /// <summary>
            /// The has location
            /// </summary>
            public bool Found { get; set; }

            /// <summary>
            /// Gets the name of the displayed.
            /// </summary>
            /// <value>
            /// The name of the displayed.
            /// </value>
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
        public PakOption PakOptions { get; set; }

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
        public PakOption Pak2Options { get; set; }

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

        /// <summary>
        /// Withes the platform graphic.
        /// </summary>
        /// <param name="pakFile">The pak file.</param>
        /// <returns></returns>
        static EstatePakFile WithPlatformGraphic(EstatePakFile pakFile)
        {
            if (pakFile != null) pakFile.Graphic = EstatePlatform.GraphicFactory?.Invoke(pakFile);
            return pakFile;
        }

        /// <summary>
        /// Paks the file factory.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <param name="game">The game.</param>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        EstatePakFile CreatePakFile(PakOption options, object value, int index, string game, Uri host)
            => WithPlatformGraphic(value switch
            {
                string path when index == 0 && PakFileType != null => (EstatePakFile)Activator.CreateInstance(PakFileType, this, game, path, null),
                string path when index == 1 && Pak2FileType != null => (EstatePakFile)Activator.CreateInstance(Pak2FileType, this, game, path, null),
                string path when (options & PakOption.Stream) != 0 => new StreamPakFile(FileManager.HostFactory, this, game, path, host),
                string[] paths when (options & PakOption.Paths) != 0 && index == 0 && PakFileType != null => (EstatePakFile)Activator.CreateInstance(PakFileType, this, game, paths),
                string[] paths when (options & PakOption.Paths) != 0 && index == 1 && Pak2FileType != null => (EstatePakFile)Activator.CreateInstance(Pak2FileType, this, game, paths),
                string[] paths when paths.Length == 1 => CreatePakFile(options, paths[0], index, game, host),
                string[] paths when paths.Length > 1 => new MultiPakFile(this, game, "Many", paths.Select(path => CreatePakFile(options, path, index, game, host)).ToArray()),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"{value}"),
            });

        /// <summary>
        /// Opens the estates pak file.
        /// </summary>
        /// <param name="paths">The file paths.</param>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public EstatePakFile OpenPakFile(string[] paths, string game, int index = 0)
            => CreatePakFile(PakOptions, paths, index, game ?? throw new ArgumentNullException(nameof(game)), null);

        /// <summary>
        /// Opens the estates pak file.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public EstatePakFile OpenPakFile(Resource resource)
            => CreatePakFile(resource.Options, resource.Paths, 0, resource.Game ?? throw new ArgumentNullException(nameof(resource.Game)), resource.Host);

        /// <summary>
        /// Opens the estates pak file.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="many">if set to <c>true</c> [many].</param>
        /// <returns></returns>
        public EstatePakFile OpenPakFile(Uri uri)
        {
            var resource = FileManager.ParseResource(this, uri);
            return CreatePakFile(resource.Options, resource.Paths, 0, resource.Game ?? throw new ArgumentNullException(nameof(resource.Game)), resource.Host);
        }

        #endregion
    }
}