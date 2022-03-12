using GameEstate.Red.Formats.Red.Types;
using System;

namespace GameEstate.Red.Formats.Red.CR2W
{
    public static class CR2WFileHelper
    {
        public static CR2WFile FromCResource(this CR2WFile file, CResource res, bool cooked = false)
        {
            // checks to see if the variable from which the chunk is built is properly constructed
            if (res == null || res.REDName != res.REDType || res.ParentVar != null) throw new NotImplementedException();
            file.CreateChunk(res);
            return file;
        }
    }
}