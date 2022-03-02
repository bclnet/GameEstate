using System;
using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public class ChunkController_826 : ChunkController
    {
        public override void Read(BinaryReader r)
        {
            base.Read(r);
            //Log($"ID is:  {id}");
            ControllerType = (CtrlType)Enum.ToObject(typeof(CtrlType), r.ReadUInt32());
            NumKeys = r.ReadUInt32();
            ControllerFlags = r.ReadUInt32();
            ControllerID = r.ReadUInt32();
            Keys = new Key[NumKeys];
            for (var i = 0; i < NumKeys; i++)
            {
                // Will implement fully later. Not sure I understand the structure, or if it's necessary.
                Keys[i].Time = r.ReadInt32();
                //Log($"Time {Keys[i].Time}", );
                Keys[i].AbsPos.X = r.ReadSingle();
                Keys[i].AbsPos.Y = r.ReadSingle();
                Keys[i].AbsPos.Z = r.ReadSingle();
                //Log($"Abs Pos: {Keys[i].AbsPos.X:F7}  {Keys[i].AbsPos.Y:F7}  {Keys[i].AbsPos.Z:F7}");
                Keys[i].RelPos.X = r.ReadSingle();
                Keys[i].RelPos.Y = r.ReadSingle();
                Keys[i].RelPos.Z = r.ReadSingle();
                //Log($"Rel Pos: {Keys[i].RelPos.X:F7}  {Keys[i].RelPos.Y:F7}  {Keys[i].RelPos.Z:F7}");
            }
        }
    }
}