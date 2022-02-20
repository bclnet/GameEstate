using System;
using System.Collections.Generic;

namespace GameEstate
{
    public class EstateManager
    {
        public class DefaultOptions
        {
            public string Estate { get; set; }
            public string GameId { get; set; }
            public string ForcePath { get; set; }
            public bool ForceOpen { get; set; }

        }

        public static DefaultOptions AppDefaultOptions = new DefaultOptions
        {
            Estate = "Aurora",

            //Estate = "AC",
            GameId = null,
            //ForcePath = "TabooTable/0E00001E.taboo",
            ForceOpen = true,
        };

        static EstateManager()
            => EstateLoader.Bootstrap();

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
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">estateName</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">name</exception>
        public static Estate GetEstate(string estateName)
            => Estates.TryGetValue(estateName, out var estate) ? estate : throw new ArgumentOutOfRangeException(nameof(estateName), estateName);
    }
}