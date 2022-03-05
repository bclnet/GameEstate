using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public class ChunkDataStream_800 : ChunkDataStream
    {
        // This includes changes for 2.6 created by Dymek (byte4/1/2hex, and 20 byte per element vertices). Thank you!
        public static float Byte4HexToFloat(string hexString) => BitConverter.ToSingle(BitConverter.GetBytes(uint.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier)), 0);
        public static int Byte1HexToIntType2(string hexString) => Convert.ToSByte(hexString, 16);

        public static float Byte2HexIntFracToFloat2(string hexString)
        {
            var sintPart = hexString.Substring(0, 2);
            var sfracPart = hexString.Substring(2, 2);
            var intPart = Byte1HexToIntType2(sintPart);
            var intnum = short.Parse(sfracPart, System.Globalization.NumberStyles.AllowHexSpecifier);
            var intbytes = BitConverter.GetBytes(intnum);
            var intbinary = Convert.ToString(intbytes[0], 2).PadLeft(8, '0');
            var binaryIntPart = intbinary;

            var num = short.Parse(sfracPart, System.Globalization.NumberStyles.AllowHexSpecifier);
            var bytes = BitConverter.GetBytes(num);
            var binary = Convert.ToString(bytes[0], 2).PadLeft(8, '0');
            var binaryFracPart = binary;

            //convert Fractional Part
            var dec = 0F;
            for (var i = 0; i < binaryFracPart.Length; i++)
            {
                if (binaryFracPart[i] == '0') continue;
                dec += (float)Math.Pow(2, (i + 1) * (-1));
            }
            var r = 0F;
            r = intPart + dec;
            //if (intPart > 0) r = intPart + dec;
            //if (intPart < 0) r = intPart - dec;
            //if (intPart == 0) r = dec;
            return r;
        }

        public override void Read(BinaryReader r)
        {
            base.Read(r);
            Flags2 = r.ReadUInt32(); // another filler
            var tmpdataStreamType = r.ReadUInt32();
            DataStreamType = (DataStreamTypeEnum)Enum.ToObject(typeof(DataStreamTypeEnum), tmpdataStreamType);
            NumElements = r.ReadUInt32(); // number of elements in this chunk
            if (_model.FileVersion == FileVersion.CryTek_3_5 || _model.FileVersion == FileVersion.CryTek_3_4) BytesPerElement = r.ReadUInt32(); // bytes per element
            if (_model.FileVersion == FileVersion.CryTek_3_6)
            {
                BytesPerElement = (uint)r.ReadInt16(); // Star Citizen 2.0 is using an int16 here now.
                r.ReadInt16(); // unknown value.  Doesn't look like padding though.
            }
            SkipBytes(r, 8);

            // Now do loops to read for each of the different Data Stream Types. If vertices, need to populate Vector3s for example.
            switch (DataStreamType)
            {
                case DataStreamTypeEnum.VERTICES: // Ref is 0x00000000
                    Vertices = new Vector4[NumElements];
                    switch (BytesPerElement)
                    {
                        case 12:
                            for (var i = 0; i < NumElements; i++)
                            {
                                Vertices[i].X = r.ReadSingle();
                                Vertices[i].Y = r.ReadSingle();
                                Vertices[i].Z = r.ReadSingle();
                            }
                            break;
                        case 8:  // Prey files, and old Star Citizen files
                            for (var i = 0; i < NumElements; i++)
                            {
                                // 2 byte floats.  Use the Half structure from TK.Math
                                //Vertices[i].X = Byte4HexToFloat(r.ReadUInt16().ToString("X8"));
                                //Vertices[i].Y = Byte4HexToFloat(r.ReadUInt16().ToString("X8")); r.ReadUInt16();
                                //Vertices[i].Z = Byte4HexToFloat(r.ReadUInt16().ToString("X8"));
                                //Vertices[i].W = Byte4HexToFloat(r.ReadUInt16().ToString("X8"));
                                Vertices[i].X = new Half { bits = r.ReadUInt16() }.ToSingle();
                                Vertices[i].Y = new Half { bits = r.ReadUInt16() }.ToSingle();
                                Vertices[i].Z = new Half { bits = r.ReadUInt16() }.ToSingle();
                                r.ReadUInt16();
                            }
                            break;
                        case 16:
                            //Console.WriteLine("method: (3)");
                            for (var i = 0; i < NumElements; i++)
                            {
                                Vertices[i].X = r.ReadSingle();
                                Vertices[i].Y = r.ReadSingle();
                                Vertices[i].Z = r.ReadSingle();
                                Vertices[i].W = r.ReadSingle(); // TODO:  Sometimes there's a W to these structures.  Will investigate.
                            }
                            break;
                    }
                    break;
                case DataStreamTypeEnum.INDICES:  // Ref is 
                    Indices = new uint[NumElements];
                    if (BytesPerElement == 2) for (var i = 0; i < NumElements; i++) Indices[i] = (uint)r.ReadUInt16(); //Console.WriteLine(R"Indices {i}: {Indices[i]}");
                    if (BytesPerElement == 4) for (var i = 0; i < NumElements; i++) Indices[i] = r.ReadUInt32();
                    //Log($"Offset is {r.BaseStream.Position:X}");
                    break;
                case DataStreamTypeEnum.NORMALS:
                    Normals = new Vector3[NumElements];
                    for (var i = 0; i < NumElements; i++)
                    {
                        Normals[i].X = r.ReadSingle();
                        Normals[i].Y = r.ReadSingle();
                        Normals[i].Z = r.ReadSingle();
                    }
                    //Log($"Offset is {r.BaseStream.Position:X}");
                    break;
                case DataStreamTypeEnum.UVS:
                    UVs = new UV[NumElements];
                    for (var i = 0; i < NumElements; i++)
                    {
                        UVs[i].U = r.ReadSingle();
                        UVs[i].V = r.ReadSingle();
                    }
                    //Log($"Offset is {r..BaseStream.Position:X}");
                    break;
                case DataStreamTypeEnum.TANGENTS:
                    Tangents = new Tangent[NumElements, 2];
                    Normals = new Vector3[NumElements];
                    for (var i = 0; i < NumElements; i++)
                        switch (BytesPerElement)
                        {
                            case 0x10:
                                // These have to be divided by 32767 to be used properly (value between 0 and 1)
                                Tangents[i, 0].X = r.ReadInt16();
                                Tangents[i, 0].Y = r.ReadInt16();
                                Tangents[i, 0].Z = r.ReadInt16();
                                Tangents[i, 0].W = r.ReadInt16();
                                //
                                Tangents[i, 1].X = r.ReadInt16();
                                Tangents[i, 1].Y = r.ReadInt16();
                                Tangents[i, 1].Z = r.ReadInt16();
                                Tangents[i, 1].W = r.ReadInt16();
                                break;
                            case 0x08:
                                // These have to be divided by 127 to be used properly (value between 0 and 1)
                                // Tangent
                                Tangents[i, 0].W = r.ReadSByte() / 127.0f;
                                Tangents[i, 0].X = r.ReadSByte() / 127.0f;
                                Tangents[i, 0].Y = r.ReadSByte() / 127.0f;
                                Tangents[i, 0].Z = r.ReadSByte() / 127.0f;
                                // Binormal
                                Tangents[i, 1].W = r.ReadSByte() / 127.0f;
                                Tangents[i, 1].X = r.ReadSByte() / 127.0f;
                                Tangents[i, 1].Y = r.ReadSByte() / 127.0f;
                                Tangents[i, 1].Z = r.ReadSByte() / 127.0f;
                                // Calculate the normal based on the cross product of the tangents.
                                //Normals[i].X = (Tangents[i, 0].Y * Tangents[i, 1].Z - Tangents[i, 0].Z * Tangents[i, 1].Y);
                                //Normals[i].Y = 0 - (Tangents[i, 0].X * Tangents[i, 1].Z - Tangents[i, 0].Z * Tangents[i, 1].X);
                                //Normals[i].Z = (Tangents[i, 0].X * Tangents[i, 1].Y - Tangents[i, 0].Y * Tangents[i, 1].X);
                                //Console.WriteLine($"Tangent: {Tangents[i, 0].X:F6} {Tangents[i, 0].Y:F6} {Tangents[i, 0].Z:F6}");
                                //Console.WriteLine($"Binormal: {Tangents[i, 1].X:F6} {Tangents[i, 1].Y:F6} {Tangents[i, 1].Z:F6}");
                                //Console.WriteLine($"Normal: {Normals[i].X:F6} {Normals[i].Y:F6} {Normals[i].Z:F6}");
                                break;
                            default: throw new Exception("Need to add new Tangent Size");
                        }
                    //Log($"Offset is {r.BaseStream.Position:X}");
                    break;
                case DataStreamTypeEnum.COLORS:
                    switch (BytesPerElement)
                    {
                        case 3:
                            RGBColors = new IRGB[NumElements];
                            for (var i = 0; i < NumElements; i++)
                            {
                                RGBColors[i].r = r.ReadByte();
                                RGBColors[i].g = r.ReadByte();
                                RGBColors[i].b = r.ReadByte();
                            }
                            break;
                        case 4:
                            RGBAColors = new IRGBA[NumElements];
                            for (var i = 0; i < NumElements; i++)
                            {
                                RGBAColors[i].r = r.ReadByte();
                                RGBAColors[i].g = r.ReadByte();
                                RGBAColors[i].b = r.ReadByte();
                                RGBAColors[i].a = r.ReadByte();
                            }
                            break;
                        default:
                            Log("Unknown Color Depth");
                            for (var i = 0; i < NumElements; i++) SkipBytes(r, BytesPerElement);
                            break;
                    }
                    break;
                case DataStreamTypeEnum.VERTSUVS:  // 3 half floats for verts, 3 half floats for normals, 2 half floats for UVs
                    Vertices = new Vector4[NumElements];
                    Normals = new Vector3[NumElements];
                    RGBColors = new IRGB[NumElements];
                    UVs = new UV[NumElements];
                    switch (BytesPerElement)  // new Star Citizen files
                    {
                        case 20:  // Dymek wrote this.  Used in 2.6 skin files.  3 floats for vertex position, 4 bytes for normals, 2 halfs for UVs.  Normals are calculated from Tangents
                            for (var i = 0; i < NumElements; i++)
                            {
                                Vertices[i].X = r.ReadSingle();
                                Vertices[i].Y = r.ReadSingle();
                                Vertices[i].Z = r.ReadSingle(); // For some reason, skins are an extra 1 meter in the z direction.
                                // Normals are stored in a signed byte, prob div by 127.
                                Normals[i].X = (float)r.ReadSByte() / 127;
                                Normals[i].Y = (float)r.ReadSByte() / 127;
                                Normals[i].Z = (float)r.ReadSByte() / 127;
                                r.ReadSByte(); // Should be FF.
                                UVs[i].U = new Half { bits = r.ReadUInt16() }.ToSingle();
                                UVs[i].V = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //UVs[i].U = Byte4HexToFloat(r.ReadUInt16().ToString("X8"));
                                //UVs[i].V = Byte4HexToFloat(r.ReadUInt16().ToString("X8"));
                            }
                            break;
                        case 16:   // Dymek updated this.
                                   //Console.WriteLine("method: (5), 3 half floats for verts, 3 colors, 2 half floats for UVs");
                            for (var i = 0; i < NumElements; i++)
                            {
                                //ushort bver = 0;
                                //var ver = 0F;
                                Vertices[i].X = Byte2HexIntFracToFloat2(r.ReadUInt16().ToString("X4")) / 127f;
                                Vertices[i].Y = Byte2HexIntFracToFloat2(r.ReadUInt16().ToString("X4")) / 127f;
                                Vertices[i].Z = Byte2HexIntFracToFloat2(r.ReadUInt16().ToString("X4")) / 127f;
                                Vertices[i].W = Byte2HexIntFracToFloat2(r.ReadUInt16().ToString("X4")) / 127f; // Almost always 1
                                // Next structure is Colors, not normals.  For 16 byte elements, normals are calculated from Tangent data.
                                //RGBColors[i].r = r.ReadByte();
                                //RGBColors[i].g = r.ReadByte();
                                //RGBColors[i].b = r.ReadByte();
                                //r.ReadByte();           // additional byte.
                                //
                                //Normals[i].X = (r.ReadByte() - 128.0f) / 127.5f;
                                //Normals[i].Y = (r.ReadByte() - 128.0f) / 127.5f;
                                //Normals[i].Z = (r.ReadByte() - 128.0f) / 127.5f;
                                //r.ReadByte();           // additional byte.
                                // Read a Quat, convert it to vector3
                                var quat = new Vector4
                                {
                                    X = (r.ReadByte() - 128.0f) / 127.5f,
                                    Y = (r.ReadByte() - 128.0f) / 127.5f,
                                    Z = (r.ReadByte() - 128.0f) / 127.5f,
                                    W = (r.ReadByte() - 128.0f) / 127.5f
                                };
                                Normals[i].X = (2 * (quat.X * quat.Z + quat.Y * quat.W));
                                Normals[i].Y = (2 * (quat.Y * quat.Z - quat.X * quat.W));
                                Normals[i].Z = (2 * (quat.Z * quat.Z + quat.W * quat.W)) - 1;

                                // UVs ABSOLUTELY should use the Half structures.
                                UVs[i].U = new Half { bits = r.ReadUInt16() }.ToSingle();
                                UVs[i].V = new Half { bits = r.ReadUInt16() }.ToSingle();

                                //Vertices[i].X = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //Vertices[i].Y = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //Vertices[i].Z = new Half { bits = r.ReadUInt16() }.ToSingle(); 
                                //Normals[i].X = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //Normals[i].Y = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //Normals[i].Z = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //UVs[i].U = new Half { bits = r.ReadUInt16() }.ToSingle();
                                //UVs[i].V = new Half { bits = r.ReadUInt16() }.ToSingle();
                            }
                            break;
                        default:
                            Log("Unknown VertUV structure");
                            SkipBytes(r, NumElements * BytesPerElement);
                            break;
                    }
                    break;
                case DataStreamTypeEnum.BONEMAP:
                    var skin = GetSkinningInfo();
                    skin.HasBoneMapDatastream = true;
                    skin.BoneMapping = new List<MeshBoneMapping>();
                    // Bones should have 4 bone IDs (index) and 4 weights.
                    for (var i = 0; i < NumElements; i++)
                    {
                        var tmpMap = new MeshBoneMapping();
                        switch (BytesPerElement)
                        {
                            case 8:
                                tmpMap.BoneIndex = new int[4];
                                tmpMap.Weight = new int[4];
                                for (var j = 0; j < 4; j++) tmpMap.BoneIndex[j] = r.ReadByte(); // read the 4 bone indexes first
                                for (var j = 0; j < 4; j++) tmpMap.Weight[j] = r.ReadByte(); // read the weights. 
                                skin.BoneMapping.Add(tmpMap);
                                break;
                            case 12:
                                tmpMap.BoneIndex = new int[4];
                                tmpMap.Weight = new int[4];
                                for (var j = 0; j < 4; j++) tmpMap.BoneIndex[j] = r.ReadUInt16(); // read the 4 bone indexes first
                                for (var j = 0; j < 4; j++) tmpMap.Weight[j] = r.ReadByte(); // read the weights.
                                skin.BoneMapping.Add(tmpMap);
                                break;
                            default: Log("Unknown BoneMapping structure"); break;
                        }
                    }
                    break;
                case DataStreamTypeEnum.UNKNOWN1:
                    Tangents = new Tangent[NumElements, 2];
                    Normals = new Vector3[NumElements];
                    for (var i = 0; i < NumElements; i++)
                    {
                        Tangents[i, 0].W = r.ReadSByte() / 127.0f;
                        Tangents[i, 0].X = r.ReadSByte() / 127.0f;
                        Tangents[i, 0].Y = r.ReadSByte() / 127.0f;
                        Tangents[i, 0].Z = r.ReadSByte() / 127.0f;
                        // Binormal
                        Tangents[i, 1].W = r.ReadSByte() / 127.0f;
                        Tangents[i, 1].X = r.ReadSByte() / 127.0f;
                        Tangents[i, 1].Y = r.ReadSByte() / 127.0f;
                        Tangents[i, 1].Z = r.ReadSByte() / 127.0f;
                        // Calculate the normal based on the cross product of the tangents.
                        Normals[i].X = (Tangents[i, 0].Y * Tangents[i, 1].Z - Tangents[i, 0].Z * Tangents[i, 1].Y);
                        Normals[i].Y = 0 - (Tangents[i, 0].X * Tangents[i, 1].Z - Tangents[i, 0].Z * Tangents[i, 1].X);
                        Normals[i].Z = (Tangents[i, 0].X * Tangents[i, 1].Y - Tangents[i, 0].Y * Tangents[i, 1].X);
                    }
                    break;
                default: Log("***** Unknown DataStream Type *****"); break;
            }
        }
    }
}