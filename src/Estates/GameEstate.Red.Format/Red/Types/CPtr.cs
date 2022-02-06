using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;
using System.Linq;

namespace GameEstate.Red.Formats.Red.Types
{
    /// <summary>
    /// A pointer to a chunk within the same cr2w file.
    /// </summary>
    [REDMeta]
    public class CPtr<T> : CVariable, IPtrAccessor where T : CVariable
    {
        public CPtr(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public CR2WExportWrapper Reference { get; set; }

        public string ReferenceType => REDType.Split(':').Last();

        public string GetPtrTargetType()
        {
            return ReferenceType;
            //try
            //{
            //    if (Reference == null)
            //        return "NULL";
            //    return Reference.REDType;
            //}
            //catch (Exception ex) { throw new InvalidPtrException(ex.Message); }
        }

        /// <summary>
        /// Reads an int from the stream and stores a reference to a chunk.
        /// A value of 0 means a null reference, all other chunk indeces are shifted by 1.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="size"></param>
        public override void Read(BinaryReader r, uint size) => SetValueInternal(r.ReadInt32());

        void SetValueInternal(int val)
        {
            try
            {
                Reference = val == 0 ? null : cr2w.Chunks[val - 1];
            }
            catch (Exception ex)
            {
                throw new InvalidPtrException(ex.Message);
            }

            // Try reparenting on virtual mountpoint
            if (Reference != null)
            {
                //Populate the reverse-lookups
                Reference.AdReferences.Add(this);
                cr2w.Chunks[LookUpChunkIndex()].AbReferences.Add(this);
                //Soft mount the chunk except root chunk
                if (Reference.ChunkIndex != 0)
                {
                    Reference.MountChunkVirtually(LookUpChunkIndex());
                }
                //Hard mounts
                switch (REDName)
                {
                    case "parent":
                    case "transformParent": cr2w.Chunks[LookUpChunkIndex()].MountChunkVirtually(Reference, true); break;
                        //   case "child" when Reference.IsVirtuallyMounted:
                        //       //tried for w2ent IAttachments, not the proper way to do it, this is graph viz territory
                        //       Reference.MountChunkVirtually(GetVarChunkIndex(), true); break;
                }
            }
        }

        public override void Write(BinaryWriter w)
        {
            var val = 0;
            if (Reference != null)
                val = Reference.ChunkIndex + 1;
            w.Write(val);
        }

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case CR2WExportWrapper wrapper: Reference = wrapper; break;
                case IPtrAccessor cval: Reference = cval.Reference; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = (CPtr<T>)base.Copy(context);
            if (Reference != null)
            {
                var newref = context.TryLookupReference(Reference, copy);
                if (newref != null)
                    copy.SetValue(newref);
            }
            return copy;
        }

        public override string ToString() => Reference == null ? "NULL" : $"{Reference.REDType} #{Reference.ChunkIndex}";

        public override string REDLeanValue() => Reference == null ? "" : $"{Reference.ChunkIndex}";
    }
}
