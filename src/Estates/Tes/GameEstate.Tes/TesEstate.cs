namespace GameEstate.Tes
{
    /// <summary>
    /// TesEstate
    /// </summary>
    /// <seealso cref="GameEstate.Estate" />
    public class TesEstate : Estate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TesEstate"/> class.
        /// </summary>
        public TesEstate() : base() { }

        /// <summary>
        /// Ensures this instance.
        /// </summary>
        /// <returns></returns>
        public override Estate Ensure() => DatabaseManager.Ensure(this);
    }
}