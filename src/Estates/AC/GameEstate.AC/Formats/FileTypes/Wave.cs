using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameEstate.AC.Formats.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0A. All are stored in .WAV data format, though the header slightly different than a .WAV file header.
    /// I'm not sure of an instance where the server would ever need this data, but it's fun nonetheless and included for completion sake.
    /// </summary>
    [PakFileType(PakFileType.Wave)]
    public class Wave : FileType, IGetExplorerInfo
    {
        public byte[] Header { get; private set; }
        public byte[] Data { get; private set; }

        public Wave(BinaryReader r)
        {
            Id = r.ReadUInt32();
            var headerSize = r.ReadInt32();
            var dataSize = r.ReadInt32();
            Header = r.ReadBytes(headerSize);
            Data = r.ReadBytes(dataSize);
        }

        //: FileTypes.Sound
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var type = Header[0] == 0x55 ? "MP3" : "WAV";
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode(null, new ExplorerContentTab { Type = "AudioPlayer", Name = "Sound", Value = null, Tag = type }),
                new ExplorerInfoNode($"{nameof(Wave)}: {Id:X8}", items: new List<ExplorerInfoNode> {
                    new ExplorerInfoNode($"Type: {type}"),
                    new ExplorerInfoNode($"Header Size: {Header.Length}"),
                    new ExplorerInfoNode($"Data Size: {Data.Length}"),
                })
            };
            return nodes;
        }

        /// <summary>
        /// Exports Wave to a playable .wav file
        /// </summary>
        public void ExportWave(string directory)
        {
            var ext = Header[0] == 0x55 ? ".mp3" : ".wav";
            var filename = Path.Combine(directory, Id.ToString("X8") + ext);
            // Good summary of the header for a WAV file and what all this means
            // http://www.topherlee.com/software/pcm-tut-wavformat.html
            var f = new FileStream(filename, FileMode.Create);
            WriteData(f);
            f.Close();
        }

        public void WriteData(Stream stream)
        {
            var w = new BinaryWriter(stream);
            w.Write(Encoding.ASCII.GetBytes("RIFF"));
            var filesize = (uint)(Data.Length + 36); // 36 is added for all the extra we're adding for the WAV header format
            w.Write(filesize);
            w.Write(Encoding.ASCII.GetBytes("WAVE"));
            w.Write(Encoding.ASCII.GetBytes("fmt"));
            w.Write((byte)0x20); // Null ending to the fmt
            w.Write((int)0x10); // 16 ... length of all the above
            // AC audio headers start at Format Type, and are usually 18 bytes, with some exceptions notably objectID A000393 which is 30 bytes
            // WAV headers are always 16 bytes from Format Type to end of header, so this extra data is truncated here.
            w.Write(Header.Take(16).ToArray());
            w.Write(Encoding.ASCII.GetBytes("data"));
            w.Write((uint)Data.Length);
            w.Write(Data);
        }
    }
}
