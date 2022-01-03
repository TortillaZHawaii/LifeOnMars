using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene;

public class Light
{
    public Vector3 Position = Vector3.Zero;

    public Vector3 DiffuseLight = Vector3.One;
    public Vector3 SpecularLight = Vector3.One;

    public float Ac = 1f;
    public float Al = 0.09f;
    public float Aq = 0.032f;

    public Light(Vector3 position)
    {
        Position = position;
    }

    public Light(Vector3 position, Vector3 color)
    {
        Position = position;
        DiffuseLight = SpecularLight = color;
    }

    public void SetLightColor(Vector3 vector)
    {
        DiffuseLight = vector;
        SpecularLight = vector;
    }

    public Vector3 GetVersorFrom(Vector3 pixelPosition)
    {
        return Vector3.Normalize(Position - pixelPosition);
    }

    public float GetAttenuation(Vector3 pixelPosition)
    {
        Vector3 r = Position - pixelPosition;
        float distance = r.Length();

        return 1f / (Ac + Al * distance + Aq * distance * distance);
    }
}
