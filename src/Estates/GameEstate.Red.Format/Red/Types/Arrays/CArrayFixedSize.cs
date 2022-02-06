using GameEstate.Red.Formats.Red.CR2W;
using System.Collections.Generic;
using System.Linq;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta()]
    public class CArrayFixedSize<T> : CArray<T> where T : CVariable
    {
        public CArrayFixedSize(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override string REDType => BuildTypeName(Elementtype, Flags.AsEnumerable().GetEnumerator());

        string BuildTypeName(string elementtype, IEnumerator<int> flags)
        {
            var v1 = flags.MoveNext() ? flags.Current : 0;
            return $"[{v1}]{Elementtype}";
        }
    }
}