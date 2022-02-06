namespace GameEstate
{
    public static class UnrealPlatform
    {
        public static unsafe bool Startup()
        {
            try
            {
                EstatePlatform.Platform = "Unreal";
                return true;
            }
            catch { return false; }
        }
    }
}