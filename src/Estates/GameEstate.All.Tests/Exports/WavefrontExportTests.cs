using GameEstate.Formats.Unknown;
using GameEstate.Formats.Wavefront;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GameEstate.Exports
{
    [TestClass]
    public class WavefrontExportTests
    {
        [DataTestMethod]
        //[DataRow("Rsi:StarCitizen", "Data/Objects/animals/fish/CleanerFish_clean_prop_animal_01.chr")]
        [DataRow("Rsi:StarCitizen", "Data/Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_01x01x02_a.cgf")]
        //[DataRow("Rsi:StarCitizen", "Data/Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_02x01x012_a.cgf")]
        //[DataRow("Rsi:StarCitizen", "Data/Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_rotary_025x01x0225_a/cga")]
        //[DataRow("Rsi:StarCitizen", "Data/Objects/buildingsets/human/hightech/prop/hydroponic/hydroponic_machine_1_incubator_rotary_025x01x0225_a/cgf")]
        //[DataRow("Rsi:StarCitizen", "Data/Objects/Characters/Human/male_v7/armor/nvy/pilot_flightsuit/m_nvy_pilot_light_armor_helmet_01.skin")]
        public async Task ExportFileObject(string pak, string sampleFile) => await ExportFileObjectAsync(Helper.Paks[pak].Value, sampleFile);

        public async Task ExportFileObjectAsync(EstatePakFile source, string sampleFile)
        {
            var unknownSource = Helper.Paks["Unknown"].Value;
            Assert.IsTrue(source.Contains(sampleFile));
            var file = await source.LoadFileObjectAsync<IUnknownFileModel>(sampleFile, unknownSource);
            var objFile = new WavefrontFileWriter(file);
            objFile.Write(@"C:\T_\Models", false);
        }
    }
}
