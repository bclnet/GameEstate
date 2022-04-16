using System;
using System.Collections.Generic;

namespace GameEstate.Unity
{
    public static class Helper
    {
        static readonly Estate estateUnity = EstateManager.GetEstate("Unity");

        public static readonly Dictionary<string, Lazy<EstatePakFile>> Paks = new()
        {
            { "Rsi:StarCitizen", new Lazy<EstatePakFile>(() => estateUnity.OpenPakFile(new Uri("game:/Data.p4k#StarCitizen"))) },
        };
    }
}
