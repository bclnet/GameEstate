using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Red.Formats.Red
{
    public interface IEditableVariable
    {
        string REDName { get; }
        string REDType { get; }
        string REDValue { get; }
        ushort REDFlags { get; }

        Guid InternalGuid { get; set; }
        IEditableVariable ParentVar { get; }
        bool IsSerialized { get; set; }
        int VarChunkIndex { get; set; }
        int LookUpChunkIndex();

        CR2WFile cr2w { get; set; }

        void SetREDName(string val);

        //Control GetEditor();
        List<IEditableVariable> ChildrEditableVariables { get; }

        List<IEditableVariable> GetEditableVariables();
        List<IEditableVariable> GetExistingVariables(bool includebuffers);

        bool CanRemoveVariable(IEditableVariable child);
        bool CanAddVariable(IEditableVariable newvar);
        void AddVariable(CVariable var);
        bool RemoveVariable(IEditableVariable child);

        void Read(BinaryReader file, uint size);
        void Write(BinaryWriter file);
        CVariable Copy(CR2WCopyAction context);
    }
}