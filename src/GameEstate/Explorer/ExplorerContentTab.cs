using System;
using System.Diagnostics;

namespace GameEstate.Explorer
{
    [DebuggerDisplay("{Type}: {Name}")]
    public class ExplorerContentTab
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public object Tag { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public IDisposable Dispose { get; set; }
        public Type EngineType { get; set; }
    }
}
