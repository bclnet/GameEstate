using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.Entity
{
    public class SpellSet
    {
        // uint key is the total combined item level of all the equipped pieces in the set
        // client calls this m_PieceCount
        public readonly SortedDictionary<uint, SpellSetTiers> SpellSetTiers;
        public readonly uint HighestTier;
        public readonly SortedDictionary<uint, SpellSetTiers> SpellSetTiersNoGaps;

        public SpellSet(BinaryReader r)
        {
            SpellSetTiers = r.ReadL16SortedMany<uint, SpellSetTiers>(sizeof(uint), x => new SpellSetTiers(x), offset: 2);
            HighestTier = SpellSetTiers.Keys.LastOrDefault();
            SpellSetTiersNoGaps = new SortedDictionary<uint, SpellSetTiers>();
            var lastSpellSetTier = default(SpellSetTiers);
            for (var i = 0U; i <= HighestTier; i++)
            {
                if (SpellSetTiers.TryGetValue(i, out var spellSetTiers))
                    lastSpellSetTier = spellSetTiers;
                if (lastSpellSetTier != null)
                    SpellSetTiersNoGaps.Add(i, lastSpellSetTier);
            }
        }
    }
}
