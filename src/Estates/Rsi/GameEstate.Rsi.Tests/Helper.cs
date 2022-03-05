using System;
using System.Collections.Generic;

namespace GameEstate.Rsi
{
    public static class Helper
    {
        static readonly Estate estateRsi = EstateManager.GetEstate("Rsi");

        public static readonly Dictionary<string, Lazy<EstatePakFile>> Paks = new()
        {
            { "Rsi:StarCitizen", new Lazy<EstatePakFile>(() => estateRsi.OpenPakFile(new Uri("game:/Data.p4k#StarCitizen"))) },
        };
    }
}
