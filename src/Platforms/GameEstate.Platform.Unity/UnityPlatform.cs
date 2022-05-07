using System;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace GameEstate
{
    public static class UnityPlatform
    {
        public static unsafe bool Startup()
        {
            var task = Task.Run(() => Application.platform.ToString());
            try
            {
                EstatePlatform.Platform = task.Result;
                EstatePlatform.GraphicFactory = source => new UnityGraphic(source);
                //Debug.Log(Platform);
                UnsafeX.Memcpy = (dest, src, count) => { UnsafeUtility.MemCpy((void*)dest, (void*)src, count); return IntPtr.Zero; };
                Debug.AssertFunc = x => UnityEngine.Debug.Assert(x);
                Debug.LogFunc = a => UnityEngine.Debug.Log(a);
                Debug.LogFormatFunc = (a, b) => UnityEngine.Debug.LogFormat(a, b);
                return true;
            }
            catch { return false; }
        }
    }
}