using OpenStack;

namespace GameEstate
{
    public static class TestPlatform
    {
        public static bool Startup()
        {
            try
            {
                EstatePlatform.Platform = "Test";
                EstatePlatform.GraphicFactory = source => new TestGraphic(source);
                Debug.AssertFunc = x => System.Diagnostics.Debug.Assert(x);
                Debug.LogFunc = a => System.Diagnostics.Debug.Print(a);
                Debug.LogFormatFunc = (a, b) => System.Diagnostics.Debug.Print(a, b);
                return true;
            }
            catch { return false; }
        }
    }
}