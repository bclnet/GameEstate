using System;
using System.IO;

namespace GameEstate.Valve.Formats.Blocks
{
    /// <summary>
    /// "VXVS" block.
    /// </summary>
    public class VXVS : Block
    {
        public override void Read(BinaryPak parent, BinaryReader r)
        {
            r.Position(Offset);
            throw new NotImplementedException();
        }
    }
}
