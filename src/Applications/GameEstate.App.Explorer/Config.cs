namespace GameEstate.Explorer
{
    public static class Config
    {
        public static string DefaultEstate { get; } = EstateManager.AppDefaultOptions?.Estate;
        public static string DefaultGameId { get; } = EstateManager.AppDefaultOptions?.GameId;
        public static string ForcePath { get; } = EstateManager.AppDefaultOptions?.ForcePath;
        public static bool ForceOpen { get; } = EstateManager.AppDefaultOptions?.ForceOpen ?? false;
        public static bool UseMapBuilder { get; } = false;
    }
}
