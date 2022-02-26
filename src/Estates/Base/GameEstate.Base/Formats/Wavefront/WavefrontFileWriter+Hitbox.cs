using GameEstate.Formats.Unknown;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Formats.Wavefront
{
    partial class WavefrontFileWriter
    {
        void WriteHitbox(StreamWriter w, IEnumerable<IUnknownProxy> proxies)  // Pass a bone stream to write to the stream.  For .chr files (armatures)
        {
            var i = 0;
            // Write out all the bones
            foreach (var proxy in proxies)
            {
                // write out this bones vertex info.
                w.WriteLine("g"); // Need to find a way to get the material name associated with the bone, so we can link the hitbox to the body part.
                foreach (var vertex in proxy.Vertexs)
                {
                    // Transform the vertex
                    //var vertex = vertex.GetTransform(tmpVertsUVs.Vertices[j]);
                    w.WriteLine($"v {vertex.X:F7} {vertex.Y:F7} {vertex.Z:F7}");
                }
                w.WriteLine();
                w.WriteLine("g {0}", i++);

                // The material file doesn't have any elements with the Name of the material.  Use i
                w.WriteLine("usemtl {0}", i);
                for (var j = 0; j < proxy.Indexs.Length; j += 3)
                    w.WriteLine("f {0} {1} {2}",
                        proxy.Indexs[j] + 1 + CurrentVertexPosition,
                        proxy.Indexs[j + 1] + 1 + CurrentVertexPosition,
                        proxy.Indexs[j + 2] + 1 + CurrentVertexPosition);
                CurrentVertexPosition += proxy.Vertexs.Length;
                CurrentIndicesPosition += proxy.Indexs.Length;
                w.WriteLine();
            }
            w.WriteLine();
        }
    }
}