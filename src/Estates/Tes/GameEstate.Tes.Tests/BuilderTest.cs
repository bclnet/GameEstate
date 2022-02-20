using GameEstate.Tes.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameEstate.Tes
{
    [TestClass]
    public class BuilderTest
    {
        static readonly Estate estate = EstateManager.GetEstate("Tes").Ensure();

        [TestMethod]
        public void MapImageBuilder()
        {
            var output = @"C:\T_\GameEstate\TesMap.png";
            using var builder = new MapImageBuilder();
            Assert.IsNotNull(builder.MapImage, "Should be not null");
            builder.MapImage.Save(output);
        }
    }
}
