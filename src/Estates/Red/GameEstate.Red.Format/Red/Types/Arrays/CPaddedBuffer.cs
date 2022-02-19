using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta()]
    public class CPaddedBuffer<T> : CBufferBase<T> where T : CVariable
    {
        public CFloat padding;

        public CPaddedBuffer(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            padding = new CFloat(cr2w, this, "padding");
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CPaddedBuffer<T>(cr2w, parent, name);

        public override void Read(BinaryReader r, uint size)
        {
            var count = new CDynamicInt(cr2w, null, "");
            count.Read(r, size);
            base.Read(r, size, count.val);
            padding.Read(r, 4);
        }

        public override List<IEditableVariable> GetEditableVariables() => new List<IEditableVariable>(elements) { padding };

        public override void Write(BinaryWriter w)
        {
            var count = new CDynamicInt(cr2w, null, "")
            {
                val = elements.Count
            };
            count.Write(w);
            base.Write(w);
            padding.Write(w);
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = base.Copy(context) as CPaddedBuffer<T>;
            //foreach (var element in elements)
            //{
            //    var ccopy = element.Copy(new CR2WCopyAction() { DestinationFile = context.DestinationFile, Parent = copy });
            //    if (ccopy is T copye)
            //        copy.elements.Add(copye);
            //}
            copy.padding = (CFloat)padding.Copy(context);
            return copy;
        }
    }
}