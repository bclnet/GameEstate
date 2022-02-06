using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GameEstate.Red.Formats.Red.CR2W
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct CR2WEmbedded
    {
        [FieldOffset(0)] public uint importIndex;        // updated on cr2w write
        [FieldOffset(4)] public uint path;               // updated on cr2w write
        [FieldOffset(8)] public ulong pathHash;          // updated on cr2w write
        [FieldOffset(16)] public uint dataOffset;         // updated on data write
        [FieldOffset(20)] public uint dataSize;           // updated on data write
    }

    public class CR2WEmbeddedWrapper
    {
        public CR2WEmbedded Embedded;

        //CR2WFile parsedFile;

        public List<CR2WImportWrapper> ParentImports { get; set; }

        public string ClassName { get; set; } = "<failed to get class name>";
        public string ImportPath { get; set; } = "<failed to get import path>";
        public string ImportClass { get; set; } = "<failed to get import class>";

        public string Handle { get; set; }
        byte[] Data;
        public CR2WFile ParentFile { get; internal set; }

        public CR2WEmbeddedWrapper() => Embedded = new CR2WEmbedded();
        public CR2WEmbeddedWrapper(CR2WEmbedded embedded) => Embedded = embedded;

        public void SetOffset(uint offset) => Embedded.dataOffset = offset;
        public void SetPath(uint offset) => Embedded.path = offset;
        public void SetPathHash(ulong hash) => Embedded.pathHash = hash;
        public void SetImportIndex(uint idx) => Embedded.importIndex = idx;

        public void ReadData(BinaryReader file)
        {
            file.BaseStream.Seek(Embedded.dataOffset, SeekOrigin.Begin);
            Data = file.ReadBytes((int)Embedded.dataSize);

            if (ParentImports != null && ParentImports.Any() && ParentImports.Count > (int)Embedded.importIndex - 1)
            {
                var import = ParentImports[(int)Embedded.importIndex - 1];
                ImportClass = import.ClassNameStr;
                ImportPath = import.DepotPathStr;
            }
        }

        public void WriteData(BinaryWriter file)
        {
            Embedded.dataOffset = (uint)file.BaseStream.Position;
            if (Data != null)
                file.Write(Data);
            Embedded.dataSize = (uint)Data.Length;
        }

        public override string ToString() => Handle;

        public async Task<CR2WFile> GetParsedFile()
        {
            var parsedFile = new CR2WFile();
            switch (await parsedFile.Read(Data))
            {
                case EFileReadErrorCodes.NoError: break;
                case EFileReadErrorCodes.NoCr2w:
                case EFileReadErrorCodes.UnsupportedVersion: return null;
                default: throw new ArgumentOutOfRangeException();
            }

            if (parsedFile.Chunks != null && parsedFile.Chunks.Any())
                ClassName = parsedFile.Chunks.FirstOrDefault()?.REDType;

            return parsedFile;
        }

        public byte[] GetRawEmbeddedData() => Data;
        public void SetRawEmbeddedData(byte[] data) => Data = data;
    }
}