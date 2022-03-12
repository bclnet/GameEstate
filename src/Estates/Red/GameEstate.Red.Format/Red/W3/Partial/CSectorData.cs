using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta(EREDMetaInfo.REDStruct)]
    public partial class CSectorData : ISerializable
    {
        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CSectorData(cr2w, parent, name);
    }
}