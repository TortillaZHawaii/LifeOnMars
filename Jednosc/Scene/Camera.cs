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

    public Matrix4x4 ViewMatrix =>  Matrix4x4.CreateLookAt(Position, Target, UpWorld);
    
}
