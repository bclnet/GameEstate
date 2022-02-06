using GameEstate.Red.Formats.Red.CR2W;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = "")]
    public class CMatrix3x3 : CVariable
    {
        public CVariable[] fields;
        public CFloat ax, ay, az, bx, by, bz, cx, cy, cz;

        public CMatrix3x3(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            fields = new CVariable[] {
                ax = new CFloat(cr2w, this, nameof(ax)),
                ay = new CFloat(cr2w, this, nameof(ay)),
                az = new CFloat(cr2w, this, nameof(az)),
                bx = new CFloat(cr2w, this, nameof(by)),
                by = new CFloat(cr2w, this, nameof(bz)),
                bz = new CFloat(cr2w, this, nameof(by)),
                cx = new CFloat(cr2w, this, nameof(cz)),
                cy = new CFloat(cr2w, this, nameof(cy)),
                cz = new CFloat(cr2w, this, nameof(cz)),
            };
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CMatrix3x3(cr2w, parent, name);

        public override List<IEditableVariable> GetEditableVariables() => new List<IEditableVariable>(fields);

        public override void Read(BinaryReader file, uint size)
        {
            foreach (var variable in fields)
                variable.Read(file, size);
        }

        public override void Write(BinaryWriter file)
        {
            foreach (var variable in fields)
                variable.Write(file);
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = base.Copy(context) as CMatrix3x3;
            for (var i = 0; i < fields.Length; i++)
                (copy.fields[i] as CFloat).val = (fields[i] as CFloat).val;
            return copy;
        }

        public override string ToString()
        {
            var b = new StringBuilder().Append(fields.Length);
            if (fields.Length > 0)
            {
                b.Append(":");
                foreach (var element in fields)
                {
                    b.Append(" <").Append(element.ToString()).Append(">");
                    if (b.Length > 100)
                    {
                        b.Remove(100, b.Length - 100);
                        break;
                    }
                }
            }
            return b.ToString();
        }
    }
}