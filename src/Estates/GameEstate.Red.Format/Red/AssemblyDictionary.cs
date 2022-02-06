using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameEstate.Red.Formats.Red
{
    /// <summary>
    /// Provides methods to get runtime types withtin the current assembly by name.
    /// </summary>
    public static class AssemblyDictionary
    {
        static Dictionary<string, Type> _types;
        static Dictionary<string, Type> _enums;

        static AssemblyDictionary()
        {
            LoadTypes();
            LoadEnums();
        }

        public static Type GetTypeByName(string typeName)
        {
            _types.TryGetValue(typeName, out Type type);
            return type;
        }

        public static IEnumerable<Type> GetSubClassesOf(Type type) => _types?.Values.Where(_ => _.IsSubclassOf(type)).ToList();

        public static bool TypeExists(string typeName) => _types.ContainsKey(typeName);

        static void LoadTypes()
        {
            _types = new Dictionary<string, Type>
            {
                { "Uint8", typeof(byte) },
                { "Uint16", typeof(ushort) },
                { "Uint32", typeof(uint) },
                { "Uint64", typeof(ulong) },
                { "Int8", typeof(sbyte) },
                { "Int16", typeof(short) },
                { "Int32", typeof(int) },
                { "Int64", typeof(long) },
                { "Bool", typeof(bool) },
                { "Float", typeof(float) },
                { "Double", typeof(double) },
                { "String", typeof(string) }
            };
            var lib = Assembly.GetExecutingAssembly();

            foreach (Type type in lib.GetTypes())
            {
                if (!type.IsPublic)
                    continue;

                if (_types.ContainsKey(type.Name))
                    continue;

                _types.Add(type.Name, type);
            }
        }

        static void LoadEnums()
        {
            _enums = new Dictionary<string, Type>();

            foreach (Type type in typeof(Enums).GetNestedTypes())
            {
                if (!type.IsEnum)
                    continue;

                if (_enums.ContainsKey(type.Name))
                    continue;

                _enums.Add(type.Name, type);
            }
        }

        public static Type GetEnumByName(string typeName)
        {
            _enums.TryGetValue(typeName, out var type);
            return type;
        }

        public static bool EnumExists(string typeName) => _enums.ContainsKey(typeName);

        public static void Reload()
        {
            _types.Clear();
            _enums.Clear();
            LoadTypes();
            LoadEnums();
        }
    }
}