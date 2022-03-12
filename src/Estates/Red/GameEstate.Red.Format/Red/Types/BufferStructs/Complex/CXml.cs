using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class CXml : CVariable
    {
        public CXml(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        byte[] _data;
        public XDocument Data
        {
            get
            {
                if (_data == null || (_data != null && _data.Length <= 0)) return new XDocument();
                using (var ms = new MemoryStream(_data)) return new XDocument(XDocument.Load(ms));
            }
            set => _data = Encoding.ASCII.GetBytes(value.ToString());
        }

        public override void Read(BinaryReader r, uint size)
        {
            var len = r.ReadInt32();
            _data = r.ReadBytes(len);
        }

        public override void Write(BinaryWriter w)
        {
            w.Write(_data.Length);
            w.Write(_data);
        }

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case XDocument document: Data = document; break;
                case CXml cvar: Data = cvar.Data; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CXml)base.Copy(context);
            var._data = _data;
            return var;
        }

        public override string ToString() => $"{Data.ToString().Length} chars";
    }
}
