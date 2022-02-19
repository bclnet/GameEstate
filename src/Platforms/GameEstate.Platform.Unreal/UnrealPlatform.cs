namespace GameEstate
{
    public static class UnrealPlatform
    {
        public static unsafe bool Startup()
        {
            try
            {
                EstatePlatform.Platform = "Unreal";
                EstatePlatform.GraphicFactory = source => new UnrealGraphic(source);
                EstateDebug.AssertFunc = x => System.Diagnostics.Debug.Assert(x);
                EstateDebug.LogFunc = a => System.Diagnostics.Debug.Print(a);
                EstateDebug.LogFormatFunc = (a, b) => System.Diagnostics.Debug.Print(a, b);
                return true;
            }
            catch { return false; }
        }
    }
}