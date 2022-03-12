using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GameEstate.Red.Formats.Red
{
    /// <summary>
    /// The reflection magic happens mostly here, with System.Activator.
    /// A class is instantiated from its type and the properties are deserialized from 
    /// cr2w later, in CVariable.Read and CR2WFile.ReadVariable
    /// </summary>
    public static class CR2WTypeManager
    {
        public static IEnumerable<string> AvailableTypes => AvailableVanillaTypes;
        public static IEnumerable<string> AvailableVanillaTypes
        {
            get
            {
                const string nspace = "GameEstate.Red.Formats.Red.CR2W.Types";
                var cr2wassembly = Assembly.GetExecutingAssembly();
                var vanillaclassNames = cr2wassembly.GetTypes()
                    .Where(_ => _.IsClass && _.Namespace == nspace)
                    .Select(_ => _.Name);
                return vanillaclassNames;
            }
        }

        /// <summary>
        /// The instantiation step of the RedEngine-3 reflection.
        /// </summary>
        /// <param name="typename">Can be either a generic type such as CUint32 - then converted with GetWKitTypeFromREDType, or a complex type like a
        /// pointer to a class, a handle to an import file, an array, a soft reference, a static reference, various buffers, an enum.</param>
        /// <param name="varname">The variable name</param>
        /// <param name="cr2w">The cr2w base file</param>
        /// <param name="parentVariable">The class owning this attribute</param>
        /// <param name="readUnknownAsBytes"></param>
        /// <returns></returns>
        public static CVariable Create(string typename, string varname, CR2WFile cr2w, CVariable parentVariable, bool readUnknownAsBytes = true)
        {
            typename = REDReflection.GetWKitBaseTypeFromREDBaseType(typename);
            var fullname = typename;
            // check for normal type
            if (AssemblyDictionary.TypeExists(typename))
            {
                var type = AssemblyDictionary.GetTypeByName(typename);
                if (type != null) return Activator.CreateInstance(type, cr2w, parentVariable, varname) as CVariable;
            }
            // check for enum types
            if (AssemblyDictionary.EnumExists(typename))
            {
                var e = (Enum)Activator.CreateInstance(AssemblyDictionary.GetEnumByName(typename));
                var cenum = MakeGenericEnumType(typeof(CEnum<>), e);
                return cenum;
            }
            //else if (CR2WManager.EnumExists(typename))
            //{
            //    var e = (Enum)Activator.CreateInstance(CR2WManager.GetEnumByName(typename));
            //    var cenum = MakeGenericEnumType(typeof(CEnum<>), e);
            //    return cenum;
            //}
            // check for generic type
            else if (typename.Contains(':'))
            {
                var splits = typename.Split(':');
                var generictype = splits.First();
                var innertype = string.Join(":", splits.Skip(1));
                // e.g. handle:CEntityTemplate
                switch (generictype)
                {
                    case "CHandle":
                    case "handle":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CHandle<>), innerobject);
                        }
                    case "CPtr":
                    case "ptr":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CPtr<>), innerobject);
                        }
                    case "CSoft":
                    case "soft":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CSoft<>), innerobject);
                        }
                    case "array":
                        {
                            // match pattern e.g. 
                            // array:            Array: (2),(0),(handle:CBitmapTexture)
                            // array:            Array: (2),(0),(Int32)
                            // array of array:   Array: (2),(0),(Array:133,0,EngineQsTransform)

                            var arraysplits = innertype.Split(',');
                            var flag1 = arraysplits[0];
                            var flag2 = arraysplits[1];
                            var body = string.Join(",", arraysplits.Skip(2));
                            if (arraysplits.Length >= 3)
                            {
                                // byte arrays, these can be huge, using ordinary arrays is just too slow.
                                if (body == "Uint8" || body == "Int8")
                                {
                                    var bytearray = new CByteArray(cr2w, parentVariable, varname);
                                    // save the actual redengine type for serialization: e.g. array:2,0,Uint8
                                    bytearray.InternalType = typename;
                                    return bytearray;
                                }

                                // all other arrays
                                var innerobject = Create(body, "", cr2w, null);
                                var arrayacc = MakeArray(typeof(CArray<>), innerobject.GetType());
                                arrayacc.Flags = new List<int> { int.Parse(flag1), int.Parse(flag2) };
                                if (innerobject is IArrayAccessor accessor && accessor.Flags != null) arrayacc.Flags.AddRange(accessor.Flags);
                                arrayacc.Elementtype = body;
                                return arrayacc as CVariable;
                            }
                            else
                            {
                                var innerobject = Create(innertype, "", cr2w, null);
                                var arrayacc = MakeArray(typeof(CArray<>), innerobject.GetType());
                                arrayacc.Elementtype = body;
                                return arrayacc as CVariable;
                            }
                        }
                    case "static":
                        {
                            typename = generictype;

                            // match pattern e.g. 
                            // static:  (4),(Uint32)
                            var regArrayType = new Regex(@"(\d+),(.+)");
                            var matchArrayType = regArrayType.Match(fullname);
                            if (matchArrayType.Success)
                            {
                                var innerobject = Create(matchArrayType.Groups[2].Value, "", cr2w, null);
                                var arrayacc = MakeArray(typeof(CStatic<>), innerobject.GetType());
                                arrayacc.Flags = new List<int>() { int.Parse(matchArrayType.Groups[1].Value) };
                                arrayacc.Elementtype = matchArrayType.Groups[2].Value;
                                return arrayacc as CVariable;
                            }
                            else throw new FormatException($"Invalid static type format: typename: {typename}.");
                        }
                    case "CBufferUInt16":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CBufferUInt16<>), innerobject);
                        }
                    case "CBufferUInt32":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CBufferUInt32<>), innerobject);
                        }
                    case "CBufferVLQInt32":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CBufferVLQInt32<>), innerobject);
                        }
                    case "CCompressedBuffer":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CCompressedBuffer<>), innerobject);
                        }
                    case "CPaddedBuffer":
                        {
                            var innerobject = Create(innertype, "", cr2w, null);
                            return MakeGenericType(typeof(CPaddedBuffer<>), innerobject);
                        }
                    case "CEnum":
                        {
                            var innerobject = CreateEnum(innertype);
                            return MakeGenericEnumType(typeof(CEnum<>), innerobject);
                        }
                    default: throw new NotImplementedException();
                }
            }
            else
            {
                // match pattern e.g. 
                // [(1)](Bezier2dHandle)
                var regFixedSizeArray = new Regex(@"^\[(\d+)\](.+)$");
                var matchFixedSizeArray = regFixedSizeArray.Match(typename);
                if (matchFixedSizeArray.Success)
                {
                    var innerobject = Create(matchFixedSizeArray.Groups[2].Value, "", cr2w, null);
                    var arrayacc = MakeArray(typeof(CArrayFixedSize<>), innerobject.GetType());
                    arrayacc.Flags = new List<int>() { int.Parse(matchFixedSizeArray.Groups[1].Value) };
                    arrayacc.Elementtype = matchFixedSizeArray.Groups[2].Value;
                    return arrayacc as CVariable;
                }

                if (fullname.Contains("@SItem"))
                {
                    cr2w.UnknownTypes.Add($"Congratulations! You have found one of the hidden e3 files! These files are special." +
                        $" If you edited this file and are experiencing errors, please contact a member of the Wkit Team. ErrorCode: {fullname}");
                    return new SItem(cr2w, parentVariable, varname);
                }
                else if (fullname.Contains("#CEnvironmentDefinition"))
                {
                    cr2w.UnknownTypes.Add($"Congratulations! You have found one of the hidden e3 files! These files are special." +
                        $" If you edited this file and are experiencing errors, please contact a member of the Wkit Team. ErrorCode: {fullname}");
                    return new CHandle<CEnvironmentDefinition>(cr2w, parentVariable, varname);
                }
                else
                {
                    // check if custom type
                    //if (CR2WManager.TypeExists(typename))
                    //{
                    //    var type = CR2WManager.GetTypeByName(typename);
                    //    return Activator.CreateInstance(type, cr2w, parentVariable, varname) as CVariable;
                    //}

                    // this should never happen
                    if (!cr2w.UnknownTypes.Contains(fullname)) cr2w.UnknownTypes.Add(fullname);

                    return readUnknownAsBytes ? new CBytes(cr2w, parentVariable, $"UNKNOWN:{typename}:{varname}") : null;
                }
            }

            IArrayAccessor MakeArray(Type arraytype, Type generictype)
            {
                Type elementType;
                if (arraytype == typeof(CStatic<>)) elementType = typeof(CStatic<>).MakeGenericType(generictype);
                else if (arraytype == typeof(CArrayFixedSize<>)) elementType = typeof(CArrayFixedSize<>).MakeGenericType(generictype);
                else if (arraytype == typeof(CArray<>)) elementType = typeof(CArray<>).MakeGenericType(generictype);
                else throw new NotImplementedException();
                var array = Activator.CreateInstance(elementType, cr2w, parentVariable, varname) as CVariable;
                return array as IArrayAccessor;
            }


            CVariable MakeGenericType(Type gentype, CVariable innerobject)
            {
                if (innerobject != null)
                {
                    var elementType = gentype.MakeGenericType(innerobject.GetType());
                    return Activator.CreateInstance(elementType, cr2w, parentVariable, varname) as CVariable;
                }
                throw new Exception();
            }

            CVariable MakeGenericEnumType(Type gentype, Enum innerobject)
            {
                if (innerobject != null)
                {
                    var elementType = gentype.MakeGenericType(innerobject.GetType());
                    return Activator.CreateInstance(elementType, cr2w, parentVariable, varname) as CVariable;
                }
                throw new Exception();
            }
        }

        static Enum CreateEnum(string value)
        {
            if (AssemblyDictionary.EnumExists(value))
            {
                var type = AssemblyDictionary.GetEnumByName(value);
                var e = (Enum)Activator.CreateInstance(type);
                return e;
            }
            //else if (CR2WManager.EnumExists(value))
            //{
            //    var type = CR2WManager.GetEnumByName(value);
            //    var e = (Enum)Activator.CreateInstance(type);
            //    return e;
            //}
            return null;
        }
    }
}