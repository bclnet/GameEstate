using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static GameEstate.Estate;

namespace GameEstate
{
    [TestClass]
    public class EstateTests
    {
        [TestMethod]
        public void Bootstrap_CanRegisterAnotherStartup()
        {
            lock (this)
            {
                EstatePlatform.Startups.Clear();
                Assert.AreEqual(0, EstatePlatform.Startups.Count, "None registered");
                EstatePlatform.Startups.Add(SomePlatform.Startup);
                Bootstrap();
                Assert.AreEqual(1, EstatePlatform.Startups.Count, "Single Startup");
                Assert.AreEqual(SomePlatform.Startup, EstatePlatform.Startups.First(), $"Default is {nameof(SomePlatform.Startup)}");
            }
        }

        [TestMethod]
        public void GetGame()
        {
            var estate = Some.Estate;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => estate.GetGame("Wrong"));
            Assert.IsNotNull(estate.GetGame("Found"));
        }

        [TestMethod]
        public void OpenPakFile_Paths()
        {
            var estate = Some.Estate;
            Assert.ThrowsException<ArgumentNullException>(() => estate.OpenPakFile(null, null));
            Assert.AreEqual(null, estate.OpenPakFile(null, "Found"));
            Assert.IsNotNull(estate.OpenPakFile(new string[] { "path" }, "Found"));
        }

        [TestMethod]
        public void OpenPakFile_Resource()
        {
            var estate = Some.Estate;
            Assert.ThrowsException<ArgumentNullException>(() => estate.OpenPakFile(new Resource { }));
            Assert.IsNotNull(estate.OpenPakFile(new Resource { Paths = new[] { "path" }, Game = "Found" }));
        }

        [TestMethod]
        public void OpenPakFile_Uri()
        {
            var estate = Some.Estate;
            Assert.AreEqual(null, estate.OpenPakFile(null));
            //// game-scheme
            //Assert.AreEqual(null, estate.OpenPakFile(new Uri("game:/path#Found")));
            //// file-scheme
            //Assert.AreEqual(null, estate.OpenPakFile(new Uri("file://path#Found")));
            //// network-scheme
            //Assert.AreEqual(null, estate.OpenPakFile(new Uri("https://path#Found")));
        }
    }
}
