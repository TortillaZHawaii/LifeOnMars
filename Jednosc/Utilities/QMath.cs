﻿using Jednosc.Scene;
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
            return new Vector3(color.R / 255, color.G / 255, color.B / 255);
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

        public static Vector3 InterpolateFromBary(Triangle3 triangle, Vector3 bary)
        {
            // probably would be faster if using matrix3x3 but it doesn't exist in System.Numerics
            var tX = new Vector3(triangle.a.X, triangle.b.X, triangle.c.X);
            var tY = new Vector3(triangle.a.Y, triangle.b.Y, triangle.c.Y);
            var tZ = new Vector3(triangle.a.Z, triangle.b.Z, triangle.c.Z);

            return new Vector3(Vector3.Dot(tX, bary), Vector3.Dot(tY, bary), Vector3.Dot(tZ, bary));
        }

        public static Vector3 InterpolateFromBary(Triangle2 triangle, Vector3 bary)
        {
            throw new NotImplementedException();
        }
    }
}