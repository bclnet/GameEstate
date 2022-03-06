using System;
using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    class ChunkIvoSkin_900 : ChunkIvoSkin
    {
        /*
         * Node IDs for Ivo models
         * 1: NodeChunk
         * 2: MeshChunk
         * 3: MeshSubsets
         * 4: Indices
         * 5: VertsUVs (contains vertices, UVs and colors)
         * 6: Normals
         * 7: Tangents
         * 8: Bonemap  (assume all #ivo files have armatures)
         * 9: Colors
         */
        bool hasNormalsChunk = false; // If Flags2 of the meshchunk is 5, there is a separate normals chunk

        public override void Read(BinaryReader r)
        {
            var model = _model;
            base.Read(r);
            SkipBytes(r, 4);

            var meshChunk = new ChunkMesh_900();
            meshChunk._model = _model;
            meshChunk._header = _header;
            meshChunk._header.Offset = (uint)r.BaseStream.Position;
            meshChunk.ChunkType = ChunkType.Mesh;
            meshChunk.Read(r);
            meshChunk.ID = 2;
            meshChunk.MeshSubsetsData = 3;
            model.ChunkMap.Add(meshChunk.ID, meshChunk);
            if (meshChunk.Flags2 == 5) hasNormalsChunk = true;

            SkipBytes(r, 120);  // Unknown data.  All 0x00

            var subsetsChunk = new ChunkMeshSubsets_900(meshChunk.NumVertSubsets);
            // Create dummy header info here (ChunkType, version, size, offset)
            subsetsChunk._model = _model;
            subsetsChunk._header = _header;
            subsetsChunk._header.Offset = (uint)r.BaseStream.Position;
            subsetsChunk.Read(r);
            subsetsChunk.ChunkType = ChunkType.MeshSubsets;
            subsetsChunk.ID = 3;
            model.ChunkMap.Add(subsetsChunk.ID, subsetsChunk);

            while (r.BaseStream.Position != r.BaseStream.Length)
            {
                var chunkType = (DatastreamType)r.ReadUInt32();
                r.BaseStream.Position = r.BaseStream.Position - 4;
                switch (chunkType)
                {
                    case DatastreamType.IVOINDICES:
                        // Indices datastream
                        ChunkDataStream_900 indicesDatastreamChunk = new ChunkDataStream_900((uint)meshChunk.NumIndices);
                        indicesDatastreamChunk._model = _model;
                        indicesDatastreamChunk._header = _header;
                        indicesDatastreamChunk._header.Offset = (uint)r.BaseStream.Position;
                        indicesDatastreamChunk.Read(r);
                        indicesDatastreamChunk.DataStreamType = DatastreamType.INDICES;
                        indicesDatastreamChunk.ChunkType = ChunkType.DataStream;
                        indicesDatastreamChunk.ID = 4;
                        model.ChunkMap.Add(indicesDatastreamChunk.ID, indicesDatastreamChunk);
                        break;
                    case DatastreamType.IVOVERTSUVS:
                        ChunkDataStream_900 vertsUvsDatastreamChunk = new ChunkDataStream_900((uint)meshChunk.NumVertices);
                        vertsUvsDatastreamChunk._model = _model;
                        vertsUvsDatastreamChunk._header = _header;
                        vertsUvsDatastreamChunk._header.Offset = (uint)r.BaseStream.Position;
                        vertsUvsDatastreamChunk.Read(r);
                        vertsUvsDatastreamChunk.DataStreamType = DatastreamType.VERTSUVS;
                        vertsUvsDatastreamChunk.ChunkType = ChunkType.DataStream;
                        vertsUvsDatastreamChunk.ID = 5;
                        model.ChunkMap.Add(vertsUvsDatastreamChunk.ID, vertsUvsDatastreamChunk);

                        // Create colors chunk
                        ChunkDataStream_900 c = new ChunkDataStream_900((uint)meshChunk.NumVertices);
                        c._model = _model;
                        c._header = _header;
                        c.ChunkType = ChunkType.DataStream;
                        c.BytesPerElement = 4;
                        c.DataStreamType = DatastreamType.COLORS;
                        c.Colors = vertsUvsDatastreamChunk.Colors;
                        c.ID = 9;
                        model.ChunkMap.Add(c.ID, c);
                        break;
                    case DatastreamType.IVONORMALS:
                    case DatastreamType.IVONORMALS2:
                    case DatastreamType.IVONORMALS3:
                        ChunkDataStream_900 normals = new ChunkDataStream_900((uint)meshChunk.NumVertices);
                        normals._model = _model;
                        normals._header = _header;
                        normals._header.Offset = (uint)r.BaseStream.Position;
                        normals.Read(r);
                        normals.DataStreamType = DatastreamType.NORMALS;
                        normals.ChunkType = ChunkType.DataStream;
                        normals.ID = 6;
                        model.ChunkMap.Add(normals.ID, normals);
                        break;
                    case DatastreamType.IVOTANGENTS:
                        ChunkDataStream_900 tangents = new ChunkDataStream_900((uint)meshChunk.NumVertices);
                        tangents._model = _model;
                        tangents._header = _header;
                        tangents._header.Offset = (uint)r.BaseStream.Position;
                        tangents.Read(r);
                        tangents.DataStreamType = DatastreamType.TANGENTS;
                        tangents.ChunkType = ChunkType.DataStream;
                        tangents.ID = 7;
                        model.ChunkMap.Add(tangents.ID, tangents);
                        if (!hasNormalsChunk)
                        {
                            // Create a normals chunk from Tangents data
                            ChunkDataStream_900 norms = new ChunkDataStream_900((uint)meshChunk.NumVertices);
                            norms._model = _model;
                            norms._header = _header;
                            //norms._header.Offset = (uint)b.BaseStream.Position;
                            norms.ChunkType = ChunkType.DataStream;
                            norms.BytesPerElement = 4;
                            norms.DataStreamType = DatastreamType.NORMALS;
                            norms.Normals = tangents.Normals;
                            norms.ID = 6;
                            model.ChunkMap.Add(norms.ID, norms);
                        }
                        break;
                    case DatastreamType.IVOBONEMAP:
                        ChunkDataStream_900 bonemap = new ChunkDataStream_900((uint)meshChunk.NumVertices);
                        bonemap._model = _model;
                        bonemap._header = _header;
                        bonemap._header.Offset = (uint)r.BaseStream.Position;
                        bonemap.Read(r);
                        bonemap.DataStreamType = DatastreamType.BONEMAP;
                        bonemap.ChunkType = ChunkType.DataStream;
                        bonemap.ID = 8;
                        model.ChunkMap.Add(bonemap.ID, bonemap);
                        break;
                    default:
                        r.BaseStream.Position = r.BaseStream.Position + 4;
                        break;
                }
            }
        }
    }
}
