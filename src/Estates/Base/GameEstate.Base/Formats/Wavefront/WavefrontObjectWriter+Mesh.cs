using GameEstate.Formats.Generic;
using System;
using System.IO;
using System.Linq;
using static GameEstate.EstateDebug;

namespace GameEstate.Formats.Wavefront
{
    partial class WavefrontObjectWriter
    {
        void WriteMesh(StreamWriter w, IGenericMesh mesh) // Pass a node to this to have it write to the Stream
        {
            w.WriteLine("o {0}", mesh.Name);

            // We only use 3 things in obj files: vertices, normals and UVs. No need to process the Tangents.
            int tempVertexPosition = CurrentVertexPosition, tempIndicesPosition = CurrentIndicesPosition;
            foreach (var subset in mesh.Subsets)
            {
                // Write vertices data for each MeshSubSet (v)
                w.WriteLine("g {0}", GroupOverride ?? mesh.Name);

                // WRITE VERTICES OUT (V, VT)
                if (mesh.Vertices == null)
                {
                    // Probably using VertsUVs (3.7+).  Write those vertices out. Do UVs at same time.
                    for (var j = subset.FirstVertex; j < subset.NumVertices + subset.FirstVertex; j++)
                    {
                        // Let's try this using this node chunk's rotation matrix, and the transform is the sum of all the transforms.
                        // Get the transform.
                        // Scales the object by the bounding box.
                        var multiplerX = Math.Abs(mesh.MinBound.X - mesh.MaxBound.X) / 2f; if (multiplerX < 1) multiplerX = 1;
                        var multiplerY = Math.Abs(mesh.MinBound.Y - mesh.MaxBound.Y) / 2f; if (multiplerY < 1) multiplerY = 1;
                        var multiplerZ = Math.Abs(mesh.MinBound.Z - mesh.MaxBound.Z) / 2f; if (multiplerZ < 1) multiplerZ = 1;

                        mesh.Vertices[j].X = mesh.Vertices[j].X * multiplerX + (mesh.MaxBound.X + mesh.MinBound.X) / 2f;
                        mesh.Vertices[j].Y = mesh.Vertices[j].Y * multiplerY + (mesh.MaxBound.Y + mesh.MinBound.Y) / 2f;
                        mesh.Vertices[j].Z = mesh.Vertices[j].Z * multiplerZ + (mesh.MaxBound.Z + mesh.MinBound.Z) / 2f;
                        var vertex = mesh.GetTransform(mesh.Vertices[j]);
                        w.WriteLine($"v {MathX.Safe(vertex.X):F7} {MathX.Safe(vertex.Y):F7} {MathX.Safe(vertex.Z):F7}");
                    }
                    w.WriteLine();
                    for (var j = subset.FirstVertex; j < subset.NumVertices + subset.FirstVertex; j++)
                        w.WriteLine($"vt {MathX.Safe(mesh.UVs[j].X):F7} {MathX.Safe(1 - mesh.UVs[j].Y):F7} 0");
                }
                else
                {
                    for (var j = subset.FirstVertex; j < subset.NumVertices + subset.FirstVertex; j++)
                        if (mesh.Vertices != null)
                        {
                            // Rotate/translate the vertex
                            var vertex = mesh.GetTransform(mesh.Vertices[j]);
                            w.WriteLine($"v {MathX.Safe(vertex.X):F7} {MathX.Safe(vertex.Y):F7} {MathX.Safe(vertex.Z):F7}");
                        }
                        else Log($"Error rendering vertices for {mesh.Name:X}");
                    w.WriteLine();
                    for (var j = subset.FirstVertex; j < subset.NumVertices + subset.FirstVertex; j++)
                        w.WriteLine($"vt {MathX.Safe(mesh.UVs[j].X):F7} {MathX.Safe(1 - mesh.UVs[j].Y):F7} 0");
                }

                w.WriteLine();

                // WRITE NORMALS BLOCK (VN)
                if (mesh.Normals != null)
                    for (var j = subset.FirstVertex; j < subset.NumVertices + subset.FirstVertex; j++)
                        w.WriteLine($"vn {mesh.Normals[j].X:F7} {mesh.Normals[j].Y:F7} {mesh.Normals[j].Z:F7}");

                // WRITE GROUP (G)
                // w.WriteLine("g {0}", this.GroupOverride ?? chunkNode.Name);

                if (Smooth) w.WriteLine("s {0}", FaceIndex++);

                // WRITE MATERIAL BLOCK (USEMTL)
                var materials = File.Materials.ToArray();
                if (materials.Length > subset.MatId) w.WriteLine("usemtl {0}", materials[subset.MatId].Name);
                else
                {
                    if (materials.Length > 0) Log($"Missing Material {subset.MatId}");
                    // The material file doesn't have any elements with the Name of the material.  Use the object name.
                    w.WriteLine("usemtl {0}_{1}", File.Name, subset.MatId);
                }

                // Now write out the faces info based on the MtlName
                for (var j = subset.FirstIndex; j < subset.NumIndices + subset.FirstIndex; j += 3)
                    w.WriteLine("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", // Vertices, UVs, Normals
                        mesh.Indices[j] + 1 + CurrentVertexPosition,
                        mesh.Indices[j + 1] + 1 + CurrentVertexPosition,
                        mesh.Indices[j + 2] + 1 + CurrentVertexPosition);

                tempVertexPosition += subset.NumVertices;  // add the number of vertices so future objects can start at the right place
                tempIndicesPosition += subset.NumIndices;  // Not really used...
            }

            // Extend the current vertex, uv and normal positions by the length of those arrays.
            CurrentVertexPosition = tempVertexPosition;
            CurrentIndicesPosition = tempIndicesPosition;
        }
    }
}