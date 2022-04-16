using System;
using System.Collections.Generic;

namespace GameEstate.Unreal
{
    public static class Helper
    {
        static readonly Estate estateUnreal = EstateManager.GetEstate("Unreal");

        public static readonly Dictionary<string, Lazy<EstatePakFile>> Paks = new()
        {
            { "Rsi:StarCitizen", new Lazy<EstatePakFile>(() => estateUnreal.OpenPakFile(new Uri("game:/Data.p4k#StarCitizen"))) },
        };
    }
}
