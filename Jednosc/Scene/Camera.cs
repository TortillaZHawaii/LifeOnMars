using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene;

public class Camera
{
    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public Vector3 UpWorld => Vector3.UnitY;

    //public Vector3 Direction => Vector3.Normalize(FocusedPoint - Position);
    //public Vector3 Right => Vector3.Normalize(Vector3.Cross(UpWorld, Direction));
    //public Vector3 Up => Vector3.Normalize(Vector3.Cross(Direction, Right));

    public Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt(Position, Target, UpWorld);
    //{
    //    get
    //    {
    //        var m1 = new Matrix4x4()
    //        {
    //            M11 = Right.X,
    //            M12 = Right.Y,
    //            M13 = Right.Z,
    //            M21 = Up.X,
    //            M22 = Up.Y,
    //            M23 = Up.Z,
    //            M31 = Direction.X,
    //            M32 = Direction.Y,
    //            M33 = Direction.Z,
    //            M44 = 1.0f,
    //        };

    //        var m2 = new Matrix4x4()
    //        {
    //            M11 = 1.0f,
    //            M22 = 1.0f,
    //            M33 = 1.0f,
    //            M44 = 1.0f,
    //            M14 = -Position.X,
    //            M24 = -Position.Y,
    //            M34 = -Position.Z,
    //        };

    //        return m1 * m2;
    //    }
    //}

}
