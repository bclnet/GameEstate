﻿using System;
using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    // provides material name as used in the .mtl file. CCCC0014
    public class ChunkMtlName_744 : ChunkMtlName
    {
        public override void Read(BinaryReader r)
        {
            base.Read(r);

            Name = r.ReadFString(128);
            NumChildren = (int)r.ReadUInt32();
            MatType = NumChildren == 0 ? MtlNameType.Single : MtlNameType.Library;
            PhysicsType = new MtlNamePhysicsType[NumChildren];
            for (var i = 0; i < NumChildren; i++) PhysicsType[i] = (MtlNamePhysicsType)r.ReadUInt32();
        }
    }
}