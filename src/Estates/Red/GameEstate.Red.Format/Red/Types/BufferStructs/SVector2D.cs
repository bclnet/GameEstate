using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Globalization;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SVector2D : CVariable
    {
        [Ordinal(0),RED] public CFloat x { get; set; }
        [Ordinal(1),RED] public CFloat y { get; set; }

        public SVector2D(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override CVariable SetValue(object val)
        {
            if (val is SVector2D v) { x = v.x; y = v.y; }
            return this;
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SVector2D(cr2w, parent, name);

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "V2[{0:0.00}, {1:0.00}]", x.val, y.val);
    }
}