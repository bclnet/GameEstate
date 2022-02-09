using System;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Data
{
    public static class DIDTables
    {
        public class Table : IEquatable<Table>
        {
            public uint SetupId;
            public uint MotionTableId;
            public uint SoundTableId;
            public uint CombatTableId;

            public bool Equals(Table table) => SetupId.Equals(table.SetupId);
            public override int GetHashCode() => SetupId.GetHashCode();
        }

        public readonly static Dictionary<uint, Table> Tables = new Dictionary<uint, Table>();

        public static void Load()
        {
            var data = new StreamReader(typeof(DIDTables).Assembly.GetManifestResourceStream($"GameEstate.AC.Data.DIDTables.txt")).ReadToEnd();
            foreach (var line in File.ReadAllLines(data))
            {
                if (line.StartsWith("#")) continue; // comment
                var pieces = line.Split(',');
                if (pieces.Length != 4) throw new Exception("Error Parsing: GameEstate.AC.Data.DIDTables.txt");

                var table = new Table
                {
                    SetupId = pieces[0].Length > 0 ? Convert.ToUInt32(pieces[0], 16) : 0,
                    MotionTableId = pieces[1].Length > 0 ? Convert.ToUInt32(pieces[1], 16) : 0,
                    SoundTableId = pieces[2].Length > 0 ? Convert.ToUInt32(pieces[2], 16) : 0,
                    CombatTableId = pieces[3].Length > 0 ? Convert.ToUInt32(pieces[3], 16) : 0
                };
                Tables.Add(table.SetupId, table);
            }
        }
    }
}
