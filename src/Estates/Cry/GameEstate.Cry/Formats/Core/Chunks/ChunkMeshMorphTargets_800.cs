﻿using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkMeshMorphTargets_001 : ChunkMeshMorphTargets
    {
        public override void Read(BinaryReader r)
        {
            base.Read(r);

            // TODO: Implement ChunkMeshMorphTargets ver 0x801.
        }
    }
}