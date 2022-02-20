using GameEstate.AC.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameEstate.AC
{
    [TestClass]
    public class BuilderTest
    {
        static readonly Estate estate = EstateManager.GetEstate("AC").Ensure();

        [TestMethod]
        public void MapImageBuilder()
        {
            var output = @"C:\T_\GameEstate\ACMap.png";
            using var builder = new MapImageBuilder();
            Assert.IsNotNull(builder.MapImage, "Should be not null");
            builder.MapImage.Save(output);
        }
    }
}
