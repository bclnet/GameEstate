using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta]
    public class IdHandle : CVariable
    {
        public CName handlename;
        public CHandle<CBehaviorVariable> handle;

        public IdHandle(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            handlename = new CName(cr2w, this, "handlename" );
            handle = new CHandle<CBehaviorVariable>(cr2w, this, "handle" );
        }

        public override void Read(BinaryReader file, uint size)
        {
            handlename.Read(file, 2);
            handle.Read(file, 2);
        }

        public override void Write(BinaryWriter file)
        {
            handlename.Write(file);
            handle.Write(file);
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new IdHandle(cr2w, parent, name);

        public override string ToString() => $"[{handlename}]:{handle}";
    }
}