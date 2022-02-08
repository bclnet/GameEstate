using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using static GameEstate.Estate;
using static GameEstate.EstateManager;

namespace GameEstate
{
    public class EstateLoader
    {
        static string[] AllEstateKeys = new[] { "AC", "Arkane", "Aurora", "Cry", "Cyanide", "Origin", "Red", "Rsi", "Tes", "Valve" };

        static EstateLoader()
        {
            Estate.Bootstrap();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var id in AllEstateKeys)
                using (var r = new StreamReader(assembly.GetManifestResourceStream($"GameEstate.Estates.{id}Estate.json")))
                {
                    var estate = ParseEstate(id, r.ReadToEnd());
                    if (estate != null) Estates.Add(estate.Id, estate);
                }
        }

        public static void Bootstrap() { }

        /// <summary>
        /// Parses the estate.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">pakFileType</exception>
        /// <exception cref="ArgumentNullException">games</exception>
        public static Estate ParseEstate(string id, string json)
        {
            var fileManager = CreateFileManager();
            var options = new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip };
            try
            {
                using var doc = JsonDocument.Parse(json, options);
                var elem = doc.RootElement;
                if (elem.TryGetProperty("fileManager", out var z)) fileManager.ParseFileManager(z);
                var locations = fileManager.Locations;
                return new Estate
                {
                    Id = (elem.TryGetProperty("id", out z) ? z.GetString() : null) ?? throw new ArgumentNullException("id"),
                    Name = (elem.TryGetProperty("name", out z) ? z.GetString() : null) ?? throw new ArgumentNullException("name"),
                    Studio = (elem.TryGetProperty("studio", out z) ? z.GetString() : null) ?? throw new ArgumentNullException("name"),
                    Description = elem.TryGetProperty("description", out z) ? z.GetString() : null,
                    PakFileType = elem.TryGetProperty("pakFileType", out z) ? Type.GetType(z.GetString(), false) ?? throw new ArgumentOutOfRangeException("pakFileType", z.GetString()) : null,
                    PakMulti = elem.TryGetProperty("pakMulti", out z) ? Enum.TryParse<PakMultiType>(z.GetString(), true, out var z1) ? z1 : throw new ArgumentOutOfRangeException("pakMulti", z.GetString()) : PakMultiType.SingleBinary,
                    Pak2FileType = elem.TryGetProperty("pak2FileType", out z) ? Type.GetType(z.GetString(), false) ?? throw new ArgumentOutOfRangeException("pak2FileType", z.GetString()) : null,
                    Pak2Multi = elem.TryGetProperty("pak2Multi", out z) ? Enum.TryParse<PakMultiType>(z.GetString(), true, out var z2) ? z2 : throw new ArgumentOutOfRangeException("pak2Multi", z.GetString()) : PakMultiType.SingleBinary,
                    Games = elem.TryGetProperty("games", out z) ? z.EnumerateObject().ToDictionary(x => x.Name, x => ParseGame(locations, x.Name, x.Value), StringComparer.OrdinalIgnoreCase) : throw new ArgumentNullException("games"),
                    FileManager = fileManager,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static FileManager CreateFileManager()
            => EstatePlatform.GetPlatformType() switch
            {
                EstatePlatform.PlatformType.Windows => new WindowsFileManager(),
                EstatePlatform.PlatformType.OSX => new MacOsFileManager(),
                EstatePlatform.PlatformType.Linux => new LinuxFileManager(),
                EstatePlatform.PlatformType.Android => new AndroidFileManager(),
                _ => throw new ArgumentOutOfRangeException(nameof(EstatePlatform.GetPlatformType), EstatePlatform.GetPlatformType().ToString()),
            };

        static EstateGame ParseGame(IDictionary<string, string> locations, string game, JsonElement elem)
        {
            var estate = new EstateGame
            {
                Game = game,
                Name = (elem.TryGetProperty("name", out var z) ? z.GetString() : null) ?? throw new ArgumentNullException("name"),
                Found = locations.ContainsKey(game),
            };
            if (elem.TryGetProperty("pak", out z))
                estate.DefaultPaks = z.ValueKind switch
                {
                    JsonValueKind.String => new[] { new Uri(z.GetString()) },
                    JsonValueKind.Array => z.EnumerateArray().Select(y => new Uri(z.GetString())).ToArray(),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            return estate;
        }
    }
}