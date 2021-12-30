using System.Numerics;

namespace Jednosc.Transformations;

public static class Projection
{
    public static Matrix4x4 GetProjectionMatrix(float fov, float aspect, float f, float n)
    {
        return new Matrix4x4()
        {
            M11 = aspect / MathF.Tan(fov / 2),
            M22 = 1 / MathF.Tan(fov / 2),
            M33 = (f + n) / (f - n),
            M34 = -2 * f * n / (f - n),
            M43 = 1,
        };
    }
}
