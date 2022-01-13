using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene.Lights;

public class PointLight : ILight
{
    public Vector3 Position = Vector3.Zero;

    public Vector3 Color = Vector3.One;

    public float Ac = 1f;
    public float Al = 0.09f;
    public float Aq = 0.032f;

    public PointLight(Vector3 position)
    {
        Position = position;
    }

    public PointLight(Vector3 position, Vector3 color)
    {
        Position = position;
        Color = color;
    }

    public float GetAttenuation(Vector3 pixelPosition)
    {
        Vector3 r = Position - pixelPosition;
        float distance = r.Length();

        return 1f / (Ac + Al * distance + Aq * distance * distance);
    }

    public Vector3 GetLightColor(Vector3 position)
    {
        return Color;
    }

    public Vector3 GetToLightVersor(Vector3 pixelPosition)
    {
        return Vector3.Normalize(Position - pixelPosition);
    }
}
