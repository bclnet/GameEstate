﻿using System;
using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public class ChunkHeader_746 : ChunkHeader_745
    {
        public override void Read(BinaryReader r)
        {
            var headerType = r.ReadUInt16() + 0xCCCBF000;
            ChunkType = (ChunkType)headerType;
            Version = r.ReadUInt16();
            ID = r.ReadInt32();
            Size = r.ReadUInt32();
            Offset = r.ReadUInt32();
            //if (ChunkType == ChunkTypeEnum.Timing) ID += 0xFFFF0000;
        }

        public override void Write(BinaryWriter w) => throw new NotImplementedException();
    }
}