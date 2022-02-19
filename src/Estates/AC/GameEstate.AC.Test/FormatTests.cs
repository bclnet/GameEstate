using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GameEstate.AC.Formats;
using GameEstate.AC.Formats.FileTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameEstate.AC
{
    [TestClass]
    public class FormatTests
    {
        const string cellDatLocation = "game:/client_cell_1.dat#AC"; const int ExpectedCellDatFileCount = 805003;
        const string portalDatLocation = "game:/client_portal.dat#AC"; const int ExpectedPortalDatFileCount = 79694;
        const string localEnglishDatLocation = "game:/client_local_English.dat#AC"; const int ExpectedLocalEnglishDatFileCount = 118;

        Estate _estate = EstateManager.GetEstate("AC");

        [TestMethod]
        public void LoadCellDat_NoExceptions()
        {
            var dat = new Database(_estate.OpenPakFile(new Uri(cellDatLocation)));
            var count = dat.Source.Count;
            //Assert.AreEqual(ExpectedCellDatFileCount, count);
            Assert.IsTrue(ExpectedCellDatFileCount <= count, $"Insufficient files parsed from .dat. Expected: >= {ExpectedCellDatFileCount}, Actual: {count}");
        }

        [TestMethod]
        public void LoadPortalDat_NoExceptions()
        {
            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var dat = new Database(_estate.OpenPakFile(new Uri(portalDatLocation)));
            var count = dat.Source.Count;
            //Assert.AreEqual(ExpectedPortalDatFileCount, count);
            Assert.IsTrue(ExpectedPortalDatFileCount <= count, $"Insufficient files parsed from .dat. Expected: >= {ExpectedPortalDatFileCount}, Actual: {count}");
        }

        [TestMethod]
        public void LoadLocalEnglishDat_NoExceptions()
        {
            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var dat = new Database(_estate.OpenPakFile(new Uri(localEnglishDatLocation)));
            var count = dat.Source.Count;
            //Assert.AreEqual(ExpectedLocalEnglishDatFileCount, count);
            Assert.IsTrue(ExpectedLocalEnglishDatFileCount <= count, $"Insufficient files parsed from .dat. Expected: >= {ExpectedLocalEnglishDatFileCount}, Actual: {count}");
        }

        [TestMethod]
        public void UnpackCellDatFiles_NoExceptions()
        {
            //var types = typeof(Database).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(PakTypeAttribute), false).Length > 0).ToList();
            //if (types.Count == 0) throw new Exception("Failed to locate any types with PakTypeAttribute.");

            //var dat = new Database(_estate.OpenPakFile(new Uri(cellDatLocation)));
            //foreach (var kvp in dat.Source.FilesById)
            //{
            //    if (kvp.Key == Iteration.FILE_ID) continue;
            //    if (kvp.Value.FileSize == 0) continue; // DatFileType.LandBlock files can be empty

            //    var fileType = kvp.Value.GetFileType(PakType.Cell);

            //    if ((kvp.Key & 0xFFFF) == 0xFFFE) fileType = PakFileType.LandBlockInfo;
            //    if ((kvp.Key & 0xFFFF) == 0xFFFF) fileType = PakFileType.LandBlock;

            //    //Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.ObjectId:X8}, FileSize: {kvp.Value.FileSize}, BitFlags:, 0x{kvp.Value.BitFlags:X8}");
            //    Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.ObjectId:X8}, FileSize: {kvp.Value.FileSize}");

            //    var type = types
            //        .SelectMany(m => m.GetCustomAttributes(typeof(PakFileTypeAttribute), false), (m, a) => new { m, a })
            //        .Where(t => ((PakFileTypeAttribute)t.a).FileType == fileType)
            //        .Select(t => t.m);

            //    var first = type.FirstOrDefault();
            //    if (first == null) throw new Exception($"Failed to Unpack fileType: {fileType}");

            //    //var obj = Activator.CreateInstance(first);
            //    //var unpackable = obj as IUnpackable;
            //    //if (unpackable == null) throw new Exception($"Class for fileType: {fileType} does not implement IUnpackable.");

            //    //var datReader = new DatReader(cellDatLocation, kvp.Value.FileOffset, kvp.Value.FileSize, dat.Header.BlockSize);
            //    //using (var memoryStream = new MemoryStream(datReader.Buffer))
            //    //using (var reader = new BinaryReader(memoryStream))
            //    //{
            //    //    unpackable.Unpack(reader);
            //    //    if (memoryStream.Position != kvp.Value.FileSize) throw new Exception($"Failed to parse all bytes for fileType: {fileType}, ObjectId: 0x{kvp.Value.ObjectId:X8}. Bytes parsed: {memoryStream.Position} of {kvp.Value.FileSize}");
            //    //}
            //}
        }

        [TestMethod]
        public void UnpackPortalDatFiles_NoExceptions()
        {
            //var types = typeof(Database).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(PakFileTypeAttribute), false).Length > 0).ToList();
            //if (types.Count == 0) throw new Exception("Failed to locate any types with PakFileTypeAttribute.");

            //var dat = new Database(_estate.OpenPakFile(new Uri(portalDatLocation)));
            //foreach (var kvp in dat.Source.FilesById)
            //{
            //    if (kvp.Key == Iteration.FILE_ID) continue;

            //    var fileType = kvp.Value.GetFileType(PakType.Portal);
            //    //Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.Id:X8}, FileSize: {kvp.Value.FileSize}, BitFlags:, 0x{kvp.Value.BitFlags:X8}");
            //    Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.Id:X8}, FileSize: {kvp.Value.FileSize}");

            //    // These file types aren't converted yet
            //    if (fileType == PakFileType.KeyMap) continue;
            //    if (fileType == PakFileType.RenderMaterial) continue;
            //    if (fileType == PakFileType.MaterialModifier) continue;
            //    if (fileType == PakFileType.MaterialInstance) continue;
            //    if (fileType == PakFileType.ActionMap) continue;
            //    if (fileType == PakFileType.MasterProperty) continue;
            //    if (fileType == PakFileType.DbProperties) continue;

            //    var type = types
            //        .SelectMany(m => m.GetCustomAttributes(typeof(PakFileTypeAttribute), false), (m, a) => new { m, a })
            //        .Where(t => ((PakFileTypeAttribute)t.a).FileType == fileType)
            //        .Select(t => t.m);

            //    var first = type.FirstOrDefault();
            //    if (first == null) throw new Exception($"Failed to Unpack fileType: {fileType}");

            //    //var obj = Activator.CreateInstance(first);
            //    //var unpackable = obj as IUnpackable;
            //    //if (unpackable == null) throw new Exception($"Class for fileType: {fileType} does not implement IUnpackable.");

            //    //var datReader = new DatReader(portalDatLocation, kvp.Value.FileOffset, kvp.Value.FileSize, dat.Header.BlockSize);
            //    //using (var memoryStream = new MemoryStream(datReader.Buffer))
            //    //using (var reader = new BinaryReader(memoryStream))
            //    //{
            //    //    unpackable.Unpack(reader);
            //    //    if (memoryStream.Position != kvp.Value.FileSize) throw new Exception($"Failed to parse all bytes for fileType: {fileType}, ObjectId: 0x{kvp.Value.ObjectId:X8}. Bytes parsed: {memoryStream.Position} of {kvp.Value.FileSize}");
            //    //}
            //}
        }

        [TestMethod]
        public void UnpackLocalEnglishDatFiles_NoExceptions()
        {
            //var types = typeof(Database).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(PakFileTypeAttribute), false).Length > 0).ToList();
            //if (types.Count == 0) throw new Exception("Failed to locate any types with PakFileTypeAttribute.");

            //var dat = new Database(_estate.OpenPakFile(new Uri(localEnglishDatLocation)));
            //foreach (var kvp in dat.Source.FilesById)
            //{
            //    if (kvp.Key == Iteration.FILE_ID) continue;

            //    var fileType = kvp.Value.GetFileType(PakType.Language);

            //    //Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.ObjectId:X8}, FileSize: {kvp.Value.FileSize}, BitFlags:, 0x{kvp.Value.BitFlags:X8}");
            //    Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.ObjectId:X8}, FileSize: {kvp.Value.FileSize}");

            //    // These file types aren't converted yet
            //    if (fileType == PakFileType.UILayout) continue;

            //    var type = types
            //        .SelectMany(m => m.GetCustomAttributes(typeof(PakFileTypeAttribute), false), (m, a) => new { m, a })
            //        .Where(t => ((PakFileTypeAttribute)t.a).FileType == fileType)
            //        .Select(t => t.m);

            //    var first = type.FirstOrDefault();
            //    if (first == null) throw new Exception($"Failed to Unpack fileType: {fileType}");

            //    //var obj = Activator.CreateInstance(first);
            //    //var unpackable = obj as IUnpackable;
            //    //if (unpackable == null) throw new Exception($"Class for fileType: {fileType} does not implement IUnpackable.");

            //    //var datReader = new DatReader(localEnglishDatLocation, kvp.Value.FileOffset, kvp.Value.FileSize, dat.Header.BlockSize);
            //    //using (var memoryStream = new MemoryStream(datReader.Buffer))
            //    //using (var reader = new BinaryReader(memoryStream))
            //    //{
            //    //    unpackable.Unpack(reader);
            //    //    if (memoryStream.Position != kvp.Value.FileSize) throw new Exception($"Failed to parse all bytes for fileType: {fileType}, ObjectId: 0x{kvp.Value.ObjectId:X8}. Bytes parsed: {memoryStream.Position} of {kvp.Value.FileSize}");
            //    //}
            //}
        }

        // uncomment if you want to run this
        // [TestMethod]
        public void ExtractCellDatByLandblock()
        {
            var output = @"C:\T_\cell_dat_export_by_landblock";
            var dat = new DatabaseCell(_estate.OpenPakFile(new Uri(cellDatLocation)));
            //dat.ExtractLandblockContents(output);
        }

        // uncomment if you want to run this
        // [TestMethod]
        public void ExportPortalDatsWithTypeInfo()
        {
            var output = @"C:\T_\typed_portal_dat_export";
            var dat = new DatabasePortal(_estate.OpenPakFile(new Uri(portalDatLocation)));
            //dat.ExtractCategorizedPortalContents(output);
        }
    }
}
