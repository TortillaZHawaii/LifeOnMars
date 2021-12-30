using System.Numerics;

namespace Jednosc.Transformations;

public static class Translation
{
    public static Matrix4x4 GetTranslationMatrix(float tX, float tY, float tZ)
    {
        return new Matrix4x4
        {
            M11 = 1.0f,
            M22 = 1.0f,
            M33 = 1.0f,
            M44 = 1.0f,
            M14 = tX,
            M24 = tY,
            M34 = tZ,
        };
    }

    public static Matrix4x4 GetTranslationMatrix(Vector3 vector)
    {
        return GetTranslationMatrix(vector.X, vector.Y, vector.Z);
    }
}
