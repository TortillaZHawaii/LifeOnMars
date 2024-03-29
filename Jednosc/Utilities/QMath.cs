﻿using Jednosc.Rendering.Shaders;
using Jednosc.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Utilities
{
    internal static class QMath
    {
        public static Vector3 GetVectorFromColor(Color color)
        {
            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        public static Color GetColorFromVector(Vector3 vector)
        {
            var clamped = Vector3.Clamp(vector, Vector3.Zero, Vector3.One);
            var in255space = clamped * 255;

            return Color.FromArgb((int)in255space.X, (int)in255space.Y, (int)in255space.Z);
        }

        public static float GetM(Point upper, Point lower)
        {
            float m = (float)(upper.X - lower.X) / (upper.Y - lower.Y);

            return m;
        }

        public static Vector3 GetBarycentric(Triangle3 t, int x, int y)
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

        public static Vector3 InterpolateFromBary(Triangle3 triangle, Vector3 bary)
        {
            // probably would be faster if using matrix3x3 but it doesn't exist in System.Numerics
            var tX = new Vector3(triangle.a.X, triangle.b.X, triangle.c.X);
            var tY = new Vector3(triangle.a.Y, triangle.b.Y, triangle.c.Y);
            var tZ = new Vector3(triangle.a.Z, triangle.b.Z, triangle.c.Z);

            return new Vector3(Vector3.Dot(tX, bary), Vector3.Dot(tY, bary), Vector3.Dot(tZ, bary));
        }

        public static Vector3 GetNormalized(this Vector3 vector3)
        {
            return Vector3.Normalize(vector3);
        }
    }

    public record struct TriangleIndexes(int a, int b, int c);
    public record struct Triangle2(Vector2 a, Vector2 b, Vector2 c);
    public record struct TriangleInt2(Point a, Point b, Point c);
    public record struct Triangle3(Vector3 a, Vector3 b, Vector3 c)
    {
        public Triangle3 Apply(Func<Vector3, Vector3> func)
        {
            return new Triangle3(func(a), func(b), func(c));
        }

        public Triangle3 Transform(Matrix4x4 matrix)
        {
            var a4 = new Vector4(a, 1f);
            var b4 = new Vector4(b, 1f);
            var c4 = new Vector4(c, 1f);

            var t4 = new Triangle4(a4, b4, c4);

            return t4.Transform(matrix).Apply(ShadingUtils.VnTo3);
        }
    }

    public record struct Triangle4(Vector4 a, Vector4 b, Vector4 c)
    {
        public Triangle3 Apply(Func<Vector4, Vector3> func)
        {
            return new Triangle3(func(a), func(b), func(c));
        }

        public Triangle4 Transform(Matrix4x4 matrix)
        {
            return new Triangle4(Vector4.Transform(a, matrix), Vector4.Transform(b, matrix), Vector4.Transform(c, matrix));
        }
    }
}
