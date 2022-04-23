using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using static GameEstate.Estate;

namespace GameEstate
{
    public class EstateManager
    {
        static string[] EstateKeys = new[] { "AC", "Arkane", "Aurora", "Cry", "Cyanide", "Origin", "Red", "Rsi", "Tes", "Unity", "Unknown", "Unreal", "Valve" };

        public class DefaultOptions
        {
            public string Estate { get; set; }
            public string GameId { get; set; }
            public string ForcePath { get; set; }
            public bool ForceOpen { get; set; }
        }

        public static DefaultOptions AppDefaultOptions = new DefaultOptions
        {
            ForceOpen = true,
            //Estate = "AC", ForcePath = "TabooTable/0E00001E.taboo",
            //Estate = "Cry", GameId = "Hunt",
            Estate = "Rsi",
            GameId = "StarCitizen",
            ForcePath = "Data/Textures/asteroids/asteroid_dmg_brown_organic_01_ddn.dds"
            //Estate = "Valve", GameId = "Dota2", ForcePath = "materials/console_background_color_psd_b9e26a4.vtex_c"
        };

        static EstateManager()
        {
            Bootstrap();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var id in EstateKeys)
                using (var r = new StreamReader(assembly.GetManifestResourceStream($"GameEstate.Estates.{id}Estate.json")))
                {
                    var estate = ParseEstate(r.ReadToEnd());
                    if (estate != null) Estates.Add(estate.Id, estate);
                }
        }

        /// <summary>
        /// Gets the estates.
        /// </summary>
        /// <value>
        /// The estates.
        /// </value>
        public static IDictionary<string, Estate> Estates { get; } = new Dictionary<string, Estate>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the specified estate.
        /// </summary>
        /// <param name="estateName">Name of the estate.</param>
        /// <param name="throwOnError">Throw on error.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">estateName</exception>
        public static Estate GetEstate(string estateName, bool throwOnError = true)
            => Estates.TryGetValue(estateName, out var estate) ? estate
            : throwOnError ? throw new ArgumentOutOfRangeException(nameof(estateName), estateName) : (Estate)null;

        #region Parse

        static FileManager CreateFileManager()
            => EstatePlatform.GetPlatformType() switch
            {
                EstatePlatform.PlatformType.Windows => new WindowsFileManager(),
                EstatePlatform.PlatformType.OSX => new MacOsFileManager(),
                EstatePlatform.PlatformType.Linux => new LinuxFileManager(),
                EstatePlatform.PlatformType.Android => new AndroidFileManager(),
                _ => throw new ArgumentOutOfRangeException(nameof(EstatePlatform.GetPlatformType), EstatePlatform.GetPlatformType().ToString()),
            };

        /// <summary>
        /// Parses the estate.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">pakFileType</exception>
        /// <exception cref="ArgumentNullException">games</exception>
        public static Estate ParseEstate(string json)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));
            var fileManager = CreateFileManager();
            var options = new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip };
            try
            {
                using var doc = JsonDocument.Parse(json, options);
                var elem = doc.RootElement;
                if (elem.TryGetProperty("fileManager", out var z)) fileManager.ParseFileManager(z);
                var locations = fileManager.Paths;
                var estateType = elem.TryGetProperty("estateType", out z) ? Type.GetType(z.GetString(), false) ?? throw new ArgumentOutOfRangeException("estateType", z.GetString()) : null;
                var estate = estateType != null ? (Estate)Activator.CreateInstance(estateType) : new Estate();
                estate.Id = (elem.TryGetProperty("id", out z) ? z.GetString() : null) ?? throw new ArgumentNullException("id");
                estate.Name = (elem.TryGetProperty("name", out z) ? z.GetString() : null) ?? throw new ArgumentNullException("name");
                estate.Studio = (elem.TryGetProperty("studio", out z) ? z.GetString() : null) ?? string.Empty;
                estate.Description = elem.TryGetProperty("description", out z) ? z.GetString() : string.Empty;
                estate.PakFileType = elem.TryGetProperty("pakFileType", out z) ? Type.GetType(z.GetString(), false) ?? throw new ArgumentOutOfRangeException("pakFileType", z.GetString()) : null;
                estate.PakOptions = elem.TryGetProperty("pakOptions", out z) ? Enum.TryParse<PakOption>(z.GetString(), true, out var z1) ? z1 : throw new ArgumentOutOfRangeException("pakOptions", z.GetString()) : 0;
                estate.Pak2FileType = elem.TryGetProperty("pak2FileType", out z) ? Type.GetType(z.GetString(), false) ?? throw new ArgumentOutOfRangeException("pak2FileType", z.GetString()) : null;
                estate.Pak2Options = elem.TryGetProperty("pak2Options", out z) ? Enum.TryParse<PakOption>(z.GetString(), true, out var z2) ? z2 : throw new ArgumentOutOfRangeException("pak2Options", z.GetString()) : 0;
                estate.Games = elem.TryGetProperty("games", out z) ? z.EnumerateObject().ToDictionary(x => x.Name, x => ParseGame(locations, x.Name, x.Value), StringComparer.OrdinalIgnoreCase) : throw new ArgumentNullException("games");
                estate.FileManager = fileManager;
                return estate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static bool TryParseKey(string str, out object value)
        {
            if (string.IsNullOrEmpty(str)) { value = null; return false; }
            if (str.StartsWith("aes:", StringComparison.OrdinalIgnoreCase))
            {
                var keyStr = str[4..];
                var key = keyStr.StartsWith("/")
                    ? Enumerable.Range(0, keyStr.Length >> 2).Select(x => byte.Parse(keyStr.Substring((x << 2) + 2, 2), NumberStyles.HexNumber)).ToArray()
                    : Enumerable.Range(0, keyStr.Length >> 1).Select(x => byte.Parse(keyStr.Substring(x << 1, 2), NumberStyles.HexNumber)).ToArray();
                value = new AesKey { Key = key };
            }
            else throw new ArgumentOutOfRangeException(nameof(str), str);
            return true;
        }

        static EstateGame ParseGame(IDictionary<string, HashSet<string>> locations, string game, JsonElement elem)
        {
            var estate = new EstateGame
            {
                Game = game,
                Name = (elem.TryGetProperty("name", out var z) ? z.GetString() : null) ?? throw new ArgumentNullException("name"),
                Key = elem.TryGetProperty("key", out z) ? TryParseKey(z.GetString(), out var z2) ? z2 : throw new ArgumentOutOfRangeException("key", z.GetString()) : null,
                Found = locations.ContainsKey(game),
            };
            if (elem.TryGetProperty("pak", out z))
                estate.Paks = z.ValueKind switch
                {
                    JsonValueKind.String => new[] { new Uri(z.GetString()) },
                    JsonValueKind.Array => z.EnumerateArray().Select(y => new Uri(z.GetString())).ToArray(),
                    _ => throw new ArgumentOutOfRangeException("pak", $"{z}"),
                };
            if (elem.TryGetProperty("dat", out z))
                estate.Dats = z.ValueKind switch
                {
                    JsonValueKind.String => new[] { new Uri(z.GetString()) },
                    JsonValueKind.Array => z.EnumerateArray().Select(y => new Uri(z.GetString())).ToArray(),
                    _ => throw new ArgumentOutOfRangeException("dat", $"{z}"),
                };
            return estate;
        }

        #endregion
    }
}