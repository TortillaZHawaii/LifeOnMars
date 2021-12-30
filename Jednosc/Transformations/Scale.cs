using System.Numerics;

namespace Jednosc.Transformations;

public static class Scale
{
    public static Matrix4x4 GetScaleMatrix(float scaleX, float scaleY, float scaleZ)
    {
        return new Matrix4x4()
        { 
            M11 = scaleX, M22 = scaleY, M33 = scaleZ, M44 = 1.0f 
        };
    }

    public static Matrix4x4 GetScaleMatrix(float scale)
    {
        return GetScaleMatrix(scale, scale, scale);
    }
}
