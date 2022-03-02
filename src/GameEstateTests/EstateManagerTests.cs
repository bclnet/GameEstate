using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GameEstate
{
    [TestClass]
    public class EstateManagerTests
    {
        [TestMethod]
        public void EstatesIsZero()
        {
            Assert.AreEqual(0, EstateManager.Estates.Count);
        }

        [TestMethod]
        public void GetEstate()
        {
            Assert.ThrowsException<ArgumentNullException>(() => EstateManager.GetEstate(null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => EstateManager.GetEstate("Missing"));
        }

        [TestMethod]
        public void ParseEstate()
        {
            Assert.ThrowsException<ArgumentNullException>(() => EstateManager.ParseEstate(null));
            Assert.IsNotNull(EstateManager.ParseEstate(Some.EstateJson.Replace("'", "\"")));
        }
    }
}
