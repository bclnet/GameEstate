using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GameEstate
{
    [TestClass]
    public class FileManagerTests
    {
        [DataTestMethod]
        [DataRow("Android", "GameEstate.AndroidFileManager, GameEstate")]
        [DataRow("Linux", "GameEstate.LinuxFileManager, GameEstate")]
        [DataRow("MacOs", "GameEstate.MacOsFileManager, GameEstate")]
        [DataRow("Windows", "GameEstate.WindowsFileManager, GameEstate")]
        public void ShouldParse(string id, string fileManagerType)
        {
            var fileManager = (FileManager)Activator.CreateInstance(Type.GetType(fileManagerType));
        }
    }
}
