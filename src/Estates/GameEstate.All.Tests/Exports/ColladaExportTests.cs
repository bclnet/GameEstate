using GameEstate.Formats.Collada;
using GameEstate.Formats.Unknown;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GameEstate.Exports
{
    [TestClass]
    public class ColladaExportTests
    {
        [DataTestMethod]
        //[DataRow("Rsi:StarCitizen", "Objects/animals/fish/CleanerFish_clean_prop_animal_01.chr")]
        [DataRow("Rsi:StarCitizen", "Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_01x01x02_a.cgf")]
        //[DataRow("Rsi:StarCitizen", "Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_02x01x012_a.cgf")]
        //[DataRow("Rsi:StarCitizen", "Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_rotary_025x01x0225_a.cga")]
        //[DataRow("Rsi:StarCitizen", "Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_rotary_025x01x0225_a.cgf")]
        public async Task ExportFileObjectAsync(string pak, string sampleFile) => await ExportFileObjectAsync(Helper.Paks[pak].Value, sampleFile);

        public async Task ExportFileObjectAsync(EstatePakFile source, string sampleFile)
        {
            var unknownSource = Helper.Paks["Unknown"].Value;
            Assert.IsTrue(source.Contains(sampleFile));
            var file = await source.LoadFileObjectAsync<IUnknownFileModel>(sampleFile, unknownSource);
            var objFile = new ColladaFileWriter(file);
            objFile.Write(@"C:\T_\Models", false);
        }
    }
}
