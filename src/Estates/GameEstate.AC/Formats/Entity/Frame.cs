using System.IO;
using System.Numerics;

namespace GameEstate.AC.Formats.Entity
{
    /// <summary>
    /// Frame consists of a Vector3 Origin and a Quaternion Orientation
    /// </summary>
    public class Frame
    {
        public Vector3 Origin { get; private set; }
        public Quaternion Orientation { get; private set; }

        public Frame()
        {
            Origin = Vector3.Zero;
            Orientation = Quaternion.Identity;
        }
        //public Frame(ACE.Entity.Position position) => Init(position.Pos, position.Rotation);
        public Frame(Vector3 origin, Quaternion orientation) => Init(origin, orientation);
        public Frame(BinaryReader r)
        {
            Origin = r.ReadVector3();
            var qw = r.ReadSingle();
            var qx = r.ReadSingle();
            var qy = r.ReadSingle();
            var qz = r.ReadSingle();
            Orientation = new Quaternion(qx, qy, qz, qw);
        }

        public void Init(Vector3 origin, Quaternion orientation)
        {
            Origin = origin;
            Orientation = new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W);
        }

        public override string ToString() => $"{Origin} - {Orientation}";
    }
}
