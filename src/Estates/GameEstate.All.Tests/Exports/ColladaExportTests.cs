//using GameEstate.Cry.Formats;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.IO;

//namespace GameEstate.Exports
//{
//    public class ColladaExportTests
//    {
//        const string AssetRoot = @"D:\StarCitizen\data";

//        [DataTestMethod]
//        //[DataRow(@"Objects\animals\fish\CleanerFish_clean_prop_animal_01.chr")]
//        [DataRow(@"Objects\buildingsets\human\hightech\prop\hydroponic\hydroponic_machine_1_incubator_01x01x02_a.cgf")]
//        //[DataRow(@"Objects\buildingsets\human\hightech\prop\hydroponic\hydroponic_machine_1_incubator_02x01x012_a.cgf")]
//        //[DataRow(@"Objects\buildingsets\human\hightech\prop\hydroponic\hydroponic_machine_1_incubator_rotary_025x01x0225_a.cga")]
//        //[DataRow(@"Objects\buildingsets\human\hightech\prop\hydroponic\hydroponic_machine_1_incubator_rotary_025x01x0225_a.cgf")]
//        public void LoadModel(string path)
//        {
//            var cryFile = new CryFile(Path.Combine(AssetRoot, path));
//            cryFile.LoadFromFile();
//            var daeFile = new ColladaObjectWriter(cryFile);
//            daeFile.Write(@"C:\T_\Models", false);
//        }
//    }
//}
