namespace GameEstate.AC
{
    /// <summary>
    /// ACEstate
    /// </summary>
    /// <seealso cref="GameEstate.Estate" />
    public class ACEstate : Estate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ACEstate"/> class.
        /// </summary>
        public ACEstate() : base() { }

        /// <summary>
        /// Ensures this instance.
        /// </summary>
        /// <returns></returns>
        public override Estate Ensure() => DatabaseManager.Ensure(this);
    }
}