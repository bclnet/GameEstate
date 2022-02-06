using System.Threading;

namespace System
{
    public static class SecureRandom
    {
        static Random _random = new Random();
        public static int RandomValue(int low, int high) => _random.Next(low, high + 1);
    }

    // important class, ensure unit tests pass for this
    // todo: implement exactly the way AC handles it.. which we'll never know unless we get original source code
    public static class ThreadSafeRandom
    {
        static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random());

        /// <summary>
        /// Returns a random floating-point number between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static float Next(float min, float max) => (float)(random.Value.NextDouble() * (max - min) + min);

        /// <summary>
        /// Returns a random integer between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static int Next(int min, int max) => random.Value.Next(min, max + 1);
    }
}
