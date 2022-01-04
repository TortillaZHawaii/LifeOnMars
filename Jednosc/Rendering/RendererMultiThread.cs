﻿using Jednosc.Rendering.Shaders;
using Jednosc.Scene;
using System.Drawing;
using System.Numerics;

namespace Jednosc.Rendering;

public class RendererMultiThread : IRenderer
{
    private DirectBitmap _bitmap;
    private RenderScene _scene;

    public RendererMultiThread(DirectBitmap bitmap, RenderScene scene)
    {
        _bitmap = bitmap;
        _scene = scene;
    }

    public void RenderScene()
    {
        ClearBitmap();
        float[,] zBuffer = GetNewZBuffer();
        Matrix4x4 viewPerspective = GetViewPerspective();

        foreach(var prop in _scene.Objects)
        {
            DrawProp(prop, zBuffer, viewPerspective);
        }
    }

    private Matrix4x4 GetViewPerspective()
    {
        float fov = 80 * MathF.PI / 180;
        float aspectRatio = _bitmap.Width / _bitmap.Height;
        float nearPlaneDistance = 1f;
        float farPlaneDistance = 20f;

        var perspective = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
        var view = _scene.Camera.ViewMatrix;

        var viewPerspective = view * perspective;

        return viewPerspective;
    }

    private float[,] GetNewZBuffer()
    {
        float[,] zBuffer = new float[_bitmap.Width, _bitmap.Height];

        for (int y = 0; y < _bitmap.Height; ++y)
        {
            for (int x = 0; x < _bitmap.Width; ++x)
            {
                zBuffer[x, y] = float.PositiveInfinity;
            }
        }

        return zBuffer;
    }

    private void DrawProp(RenderObject prop, float[,] zBuffer, Matrix4x4 viewPerspective)
    {
        var shader = new TextureShader(prop, viewPerspective);
        //var shader = new LightNormalMapShader(prop, viewPerspective, _scene);
        for(int iFace = 0; iFace < prop.VertexIndexes.Length; ++iFace)
        {
            FillTriangle(zBuffer, iFace, shader);
        }
    }

    private void FillTriangle(float[,] zBuffer, int iFace, IShader shader)
    {
        // when using scan line we need to preprocess triangles and keep only the in screen ones
        var rawTriangle = shader.Triangle(iFace);
        var triangle = rawTriangle.Apply(Vs);

        // back-face culling
        if (IsTriangleFacingBack(triangle))
            return;

        if (IsVertexOutOfCube(rawTriangle))
            return;

        var (min, max) = GetBoundingBox(triangle);

        if (min.X == max.X || min.Y == max.Y)
            return;

        Parallel.For(min.Y, max.Y + 1, y =>
        {
            for (int x = min.X; x <= max.X; ++x)
            {
                Vector3 bary = GetBarycentric(triangle, x, y);

                bool isOutOfTriangle = bary.X < 0 || bary.Y < 0 || bary.Z < 0;
                if (isOutOfTriangle)
                    continue;

                float z = GetZ(bary, triangle);

                if(zBuffer[x, y] > z)
                {
                    var color = shader.Fragment(bary);
                    if(color != null)
                    {
                        zBuffer[x, y] = z;
                        _bitmap.SetPixel(x, y, color.Value);
                    }
                }
            }
        });
    }

    private bool IsVertexOutOfCube(Triangle3 triangle)
    {
        return triangle.a.X < -1.2f || triangle.a.Y < -1.2f || triangle.a.Z < -1f ||
            triangle.b.X < -1.2f || triangle.b.Y < -1.2f || triangle.b.Z < -1 ||
            triangle.c.X < -1.2f || triangle.c.Y < -1.2f || triangle.c.Z < -1 ||
            triangle.a.X > 1.2f || triangle.a.Y > 1.2f || triangle.a.Z > 1 ||
            triangle.b.X > 1.2f || triangle.b.Y > 1.2f || triangle.b.Z > 1 ||
            triangle.c.X > 1.2f || triangle.c.Y > 1.2f || triangle.c.Z > 1;
    }

    private Triangle3 Vs(Triangle3 triangle)
    {
        return triangle.Apply(Vs);
    }

    private Vector3 Vs(Vector3 vector)
    {
        return new Vector3()
        {
            X = _bitmap.Width * (1 + vector.X) * 0.5f,
            Y = _bitmap.Height * (1 - vector.Y) * 0.5f,
            Z = vector.Z,
        };
    }

    // back-face culling
    private static bool IsTriangleFacingBack(Triangle3 triangle)
    {
        var normal = GetNormal(triangle);
        Vector3 fromScreen = Vector3.UnitZ;
        float intensity = Vector3.Dot(normal, fromScreen);
        
        bool isBackFace = intensity < 0;

        return isBackFace;
    }

    private static Vector3 GetNormal(Triangle3 triangle)
    {
        Vector3 u = triangle.c - triangle.a;
        Vector3 v = triangle.b - triangle.a;

        Vector3 normal = Vector3.Cross(u, v);

        return Vector3.Normalize(normal);
    }

    private float GetZ(Vector3 bary, Triangle3 triangle)
    {
        return triangle.a.Z * bary.X + triangle.b.Z * bary.Y + triangle.c.Z * bary.Z;
    }

    private (Point min, Point max) GetBoundingBox(Triangle3 t)
    {
        int minX = (int)MathF.Min(t.a.X, MathF.Min(t.b.X, t.c.X));
        int minY = (int)MathF.Min(t.a.Y, MathF.Min(t.b.Y, t.c.Y));

        int maxX = (int)MathF.Max(t.a.X, MathF.Max(t.b.X, t.c.X));
        int maxY = (int)MathF.Max(t.a.Y, MathF.Max(t.b.Y, t.c.Y));

        // to the screen
        minX = Math.Max(minX, 0);
        minY = Math.Max(minY, 0);

        maxX = Math.Min(maxX, _bitmap.Width - 1);
        maxY = Math.Min(maxY, _bitmap.Height - 1);

        return (new Point(minX, minY), new Point(maxX, maxY));
    }

    private static Vector3 GetBarycentric(Triangle3 t, int x, int y)
    {
        Vector3 vX = new Vector3()
        {
            X = t.c.X - t.a.X,
            Y = t.b.X - t.a.X,
            Z = t.a.X - x,
        };

        Vector3 vY = new Vector3()
        {
            X = t.c.Y - t.a.Y,
            Y = t.b.Y - t.a.Y,
            Z = t.a.Y - y,
        };


        Vector3 u = Vector3.Cross(vX, vY);

        if (MathF.Abs(u.Z) < 1f)
        {
            return new Vector3(-1, 1, 1);
        }

        return new Vector3(1f - (u.X + u.Y) / u.Z, u.Y / u.Z, u.X / u.Z);
    }

    private void ClearBitmap()
    {
        Parallel.For(0, _bitmap.Height, (y) =>
        {
            for(int x = 0; x < _bitmap.Width; x++)
            {
                _bitmap.SetPixel(x, y, _scene.BackgroundColor);
            }
        });
    }
}