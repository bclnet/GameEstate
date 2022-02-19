using GameEstate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public abstract class AbstractTest : IDisposable
    {
        protected readonly UnityTest Test;
        protected readonly Estate Estate;
        protected readonly List<EstatePakFile> PakFiles = new List<EstatePakFile>();
        protected readonly IUnityGraphic Graphic;

        public AbstractTest(UnityTest test)
        {
            Test = test;
            if (string.IsNullOrEmpty(test.Estate))
                return;
            Estate = EstateManager.GetEstate(test.Estate);
            if (!string.IsNullOrEmpty(test.PakUri)) PakFiles.Add(Estate.OpenPakFile(new Uri(test.PakUri)));
            if (!string.IsNullOrEmpty(test.Pak2Uri)) PakFiles.Add(Estate.OpenPakFile(new Uri(test.Pak2Uri)));
            if (!string.IsNullOrEmpty(test.Pak3Uri)) PakFiles.Add(Estate.OpenPakFile(new Uri(test.Pak3Uri)));
            var first = PakFiles.FirstOrDefault();
            Graphic = (IUnityGraphic)first?.Graphic;
        }

        public virtual void Dispose()
        {
            foreach (var pakFile in PakFiles) pakFile.Dispose();
            PakFiles.Clear();
        }

        public abstract void Start();

        public virtual void Update() { }
    }
}
