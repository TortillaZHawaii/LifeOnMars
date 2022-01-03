using Jednosc.Scene;
using System.Drawing;
using System.Numerics;

namespace Jednosc.Rendering;

public class FlatShaderRenderer : IRenderer
{
    private DirectBitmap _bitmap;
    private RenderScene _scene;

    public FlatShaderRenderer(DirectBitmap bitmap, RenderScene scene)
    {
        _bitmap = bitmap;
        _scene = scene;
    }

    public void Render()
    {

    }

    private float _angle = 0;

    public void Render(RenderObject prop)
    {
        using var graphics = Graphics.FromImage(_bitmap.Bitmap);
        _rng = new Random(0);
        float[,] zBuffer = new float[_bitmap.Width, _bitmap.Height];
        for(int i = 0; i < _bitmap.Width; i++)
        {
            for(int j = 0; j < _bitmap.Height; j++)
            {
                zBuffer[i, j] = float.PositiveInfinity;
            }
        }

        float fov = 80 * MathF.PI / 180;
        float aspectRatio = _bitmap.Width / _bitmap.Height;
        float nearPlaneDistance = 0.1f;
        float farPlaneDistance = 100f;

        var perspective = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
        var view = _scene.Camera.ViewMatrix;
        var model = prop.ModelMatrix;

        var mvp = model * view * perspective;

        var scale = Matrix4x4.CreateScale(0.1f);

        var rotation = Matrix4x4.CreateRotationY(_angle);

        _angle += 0.1f;

        graphics.Clear(Color.Black);

        var newVertices = prop.Vertices.AsParallel()
            .Select(x => Vs(Vn(Vector4.Transform(x, scale * rotation * mvp)))).ToArray();

        foreach (var triangle in prop.VertexIndexes)
        {
            //ScanLineTriangle(newVertices[triangle.a], newVertices[triangle.b], newVertices[triangle.c], Vector3.Zero, zBuffer);
            var triangle3 = new Triangle3(newVertices[triangle.a], newVertices[triangle.b], newVertices[triangle.c]);
            FillNaive(triangle3, zBuffer);
        }
    }

    private static Vector3 Vn(Vector4 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z) / vector.W;
    }

    private Point Vs(Vector4 vector)
    {
        return new Point()
        {
            X = (int)(_bitmap.Width * (1 + vector.X) * 0.5f),
            Y = (int)(_bitmap.Height * (1 - vector.Y) * 0.5f),
        };
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

    private Random _rng = new Random(0);


    // adapted from
    // https://github.com/ssloy/tinyrenderer/wiki/Lesson-2:-Triangle-rasterization-and-back-face-culling
    private void ScanLineTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, float[,] zBuffer)
    {
        bool isHorizontalLine = a.Y == b.Y && a.Y == c.Y;
        if (isHorizontalLine)
            return;

        var color = Color.FromArgb(_rng.Next(256), _rng.Next(256), _rng.Next(256));

        // sort
        var sorted = new[] { a, b, c }.OrderBy(p => p.Y).ToArray();

        int s0y = (int)sorted[0].Y;
        int s1y = (int)sorted[1].Y;
        int s2y = (int)sorted[2].Y;

        int totalHeight = s2y - s0y;

        // mono thread is quicker than parallel
        for (int y = 0; y < totalHeight; ++y)
        {
            bool isSecondHalf = y > (s1y - s0y) || s1y == s0y;

            int segmentHeight = isSecondHalf ?
                (s2y - s1y) :
                (s1y - s0y);

            float alpha = (float)y / totalHeight;
            float beta = (float)(y - (isSecondHalf ? s1y - s0y : 0)) / segmentHeight;

            int x1 = (int)(sorted[0].X + ((sorted[2].X - sorted[0].X) * alpha));
            int x2 = (int)(isSecondHalf ?
                sorted[1].X + (int)((sorted[2].X - sorted[1].X) * beta) :
                sorted[0].X + (int)((sorted[1].X - sorted[0].X) * beta));

            float z1 = (sorted[0].Z + (sorted[2].Z + sorted[0].Z) * alpha);
            float z2 = isSecondHalf ?
                sorted[1].Z + (sorted[2].Z - sorted[1].Z) * beta :
                sorted[0].Z + (sorted[1].Z - sorted[0].Z) * beta;

            float zLeft, zRight;
            int xLeft, xRight;

            if(x1 < x2)
            {
                zLeft = z1;
                zRight = z2;
                xLeft = x1;
                xRight = x2;
            }
            else
            {
                zLeft = z2;
                zRight = z1;
                xLeft = x2;
                xRight = x1;
            }

            int totalWidth = xRight - xLeft + 1;

            for (int x = xLeft; x <= xRight; ++x)
            {
                float gamma = (float)(x - xLeft) / totalWidth;

                float z = (zRight - zLeft) * gamma;

                if(zBuffer[x, y] > z)
                {
                    zBuffer[x, y] = z;

                    _bitmap.SetPixel(x, y + s0y, color);
                }
            }
        }
    }

   

    private static Vector3 Barycentric(Triangle3 t, Point point)
    {
        Vector3 vX = new Vector3()
        {
            X = t.c.X - t.a.X,
            Y = t.b.X - t.a.X,
            Z = t.a.X - point.X,
        };

        Vector3 vY = new Vector3()
        {
            X = t.c.Y - t.a.Y,
            Y = t.b.Y - t.a.Y,
            Z = t.a.Y - point.Y,
        };


        Vector3 u = Vector3.Cross(vX, vY);

        if(MathF.Abs(u.Z) < 1f)
        {
            return new Vector3(-1, 1, 1);
        }

        return new Vector3(1f - (u.X+u.Y) / u.Z, u.Y / u.Z, u.X / u.Z);
    }

    // adapted from
    // https://github.com/ssloy/tinyrenderer/wiki/Lesson-2:-Triangle-rasterization-and-back-face-culling
    private void ScanLineTriangle(Triangle3 t, float[,] zBuffer)
    {
        bool isHorizontalLine = t.a.Y == t.b.Y && t.a.Y == t.c.Y;
        if (isHorizontalLine)
            return;

        var color = Color.FromArgb(_rng.Next(256), _rng.Next(256), _rng.Next(256));

        // sort
        var sorted = new[] { t.a, t.b, t.c }.OrderBy(p => p.Y).ToArray();

        int s0y = (int)sorted[0].Y;
        int s1y = (int)sorted[1].Y;
        int s2y = (int)sorted[2].Y;

        int totalHeight = s2y - s0y;

        if (s0y >= _bitmap.Height || s2y < 0)
            return;

        // mono thread is quicker than parallel
        for (int y0 = 0; y0 < totalHeight; ++y0)
        {
            int y = y0 + s0y;

            if (y < 0)
                continue;
            if (y >= _bitmap.Height)
                return;

            bool isSecondHalf = y0 > (s1y - s0y) || s1y == s0y;

            int segmentHeight = isSecondHalf ?
                (s2y - s1y) :
                (s1y - s0y);

            float alpha = (float)y0 / totalHeight;
            float beta = (float)(y0 - (isSecondHalf ? s1y - s0y : 0)) / segmentHeight;

            int x1 = (int)(sorted[0].X + ((sorted[2].X - sorted[0].X) * alpha));
            int x2 = (int)(isSecondHalf ?
                sorted[1].X + (int)((sorted[2].X - sorted[1].X) * beta) :
                sorted[0].X + (int)((sorted[1].X - sorted[0].X) * beta));

            int xLeft = Math.Min(x1, x2);
            int xRight = Math.Max(x1, x2);

            if (xLeft >= _bitmap.Width || xRight < 0)
                continue;

            xLeft = Math.Max(xLeft, 0);
            xRight = Math.Min(xRight, _bitmap.Width - 1);

            for (int x = xLeft; x <= xRight; ++x)
            {
                Vector3 bary = Barycentric(t, new Point(x, y0));

                float z = t.a.Z * bary.X + t.b.Z * bary.Y + t.c.Z * bary.Z;

                if (zBuffer[x, y] > z)
                {
                    zBuffer[x, y] = z;

                    _bitmap.SetPixel(x, y, color);
                }
            }
        }
    }
    private IEnumerable<Point> BresenhamLine(Point start, Point end)
    {
        int x = start.X;
        int y = start.Y;
        int x2 = end.X;
        int y2 = end.Y;

        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            yield return new Point(x, y);

            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }
    private void ScanLine(Triangle3 t, float[,] zBuffer)
    {
        var color = Color.FromArgb(_rng.Next(256), _rng.Next(256), _rng.Next(256));

        // from top to bottom
        var sorted = new[] { t.a, t.b, t.c }.OrderBy(v => v.Y).ToArray();
        var points = sorted.Select(v => new Point((int)v.X, (int)v.Y)).ToArray();

        var line01enum = BresenhamLine(points[0], points[1]).GetEnumerator(); // first half
        var line12enum = BresenhamLine(points[1], points[2]).GetEnumerator(); // second half
        var line02 = BresenhamLine(points[0], points[2]); // total height

        foreach (var p0 in line02)
        {
            // we need to start line12enum at p1y
            bool isFirstHalf = p0.Y < points[1].Y;

            Point p1;
            if (isFirstHalf)
            {
                while (line01enum.MoveNext() && line01enum.Current.Y < p0.Y) ;
                p1 = line01enum.Current;
            }
            else
            {
                while (line12enum.MoveNext() && line12enum.Current.Y < p0.Y) ;
                p1 = line12enum.Current;
            }

            int y = p0.Y;

            if (p0.Y != p1.Y)
                throw new Exception("ajajaj");

            int x1 = Math.Min(p0.X, p1.X);
            int x2 = Math.Max(p0.X, p1.X);

            for (int x = x1; x <= x2; ++x)
            {
                var bary = GetBarycentric(t, x, y);

                bool isOutOfTriangle = bary.X < 0 || bary.Y < 0 || bary.Z < 0;
                //if (isOutOfTriangle)
                //    continue;

                float z = t.a.Z * bary.X + t.b.Z * bary.Y + t.c.Z * bary.Z;

                if (IsInBounds(x, y) && zBuffer[x, y] > z)
                {
                    //zBuffer[x, y] = z;
                    _bitmap.SetPixel(x, y, color);
                }
            }
        }
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
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _bitmap.Width && y < _bitmap.Height;
    }

    private void FillNaive(Triangle3 triangle, float[,] zBuffer)
    {
        var (min, max) = GetBoundingBox(triangle);

        var color = Color.FromArgb(_rng.Next(256), _rng.Next(256), _rng.Next(256));

        if (min.X == max.X || min.Y == max.Y)
            return;

        Parallel.For(min.Y, max.Y, y =>
        {
            for (int x = min.X; x <= max.X; ++x)
            {
                Vector3 bary = Barycentric(triangle, new Point(x, y));

                bool isOutOfTriangle = bary.X < 0 || bary.Y < 0 || bary.Z < 0;
                if (isOutOfTriangle)
                    continue;

                float z = triangle.a.Z * bary.X + triangle.b.Z * bary.Y + triangle.c.Z * bary.Z;

                if (zBuffer[x, y] > z)
                {
                    zBuffer[x, y] = z;

                    _bitmap.SetPixel(x, y, color);
                }
            }
        });
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
}
