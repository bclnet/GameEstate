using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public class ChunkXmlFileSC_3 : ChunkXmlFileSC
    {
        public override void Read(BinaryReader r)
        {
            base.Read(r);
            var xml = CryXmlSerializer.ReadStream(r.BaseStream, true);
        }
    }
}
