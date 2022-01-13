using Jednosc.Scene;
using System.Drawing;
using System.Numerics;

namespace Jednosc.Rendering;

public class TextureRenderer : IRenderer
{
    private DirectBitmap _bitmap;
    private RenderScene _scene;

    public TextureRenderer(DirectBitmap bitmap, RenderScene scene)
    {
        _bitmap = bitmap;
        _scene = scene;
    }

    public void RenderScene()
    {
        float fov = 80 * MathF.PI / 180;
        float aspectRatio = _bitmap.Width / _bitmap.Height;
        float nearPlaneDistance = 1f;
        float farPlaneDistance = 2f;
        
        var perspective = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
        var view = _scene.Camera.ViewMatrix;

        var viewPerspective = view * perspective;

        foreach (var prop in _scene.Objects)
        {
            Render(prop, viewPerspective);
        }

    }

    public void Render(RenderObject prop, Matrix4x4 viewPerspective)
    {
        using var graphics = Graphics.FromImage(_bitmap.Bitmap);
        _rng = new Random(0);
        float[,] zBuffer = new float[_bitmap.Width, _bitmap.Height];
        for (int i = 0; i < _bitmap.Width; i++)
        {
            for (int j = 0; j < _bitmap.Height; j++)
            {
                zBuffer[i, j] = float.PositiveInfinity;
            }
        }

        var model = prop.ModelMatrix;

        var mvp = model * viewPerspective;

        graphics.Clear(Color.Black);

        var newVertices = prop.Vertices.AsParallel()
            .Select(x => VsVn(Vector4.Transform(x, mvp))).ToArray();
        var newNormals = prop.VertexNormals.AsParallel()
            .Select(x => Vn(Vector4.Transform(x, mvp))).ToArray();

        for(int i = 0; i < prop.VertexIndexes.Length; ++i)
        {
            var triangle = prop.VertexIndexes[i];
            var triangle3 = new Triangle3(newVertices[triangle.a], newVertices[triangle.b], newVertices[triangle.c]);
            FloatScanLine(triangle3, zBuffer, prop, i, newNormals);
        }

        //for (int i = 0; i < prop.VertexIndexes.Length; ++i)
        //{
        //    var triangle = prop.VertexIndexes[i];
        //    var triangle3 = new Triangle3(newVertices[triangle.a], newVertices[triangle.b], newVertices[triangle.c]);
        //    WireScanLine(triangle3);
        //}
    }

    private Vector3 Vn(Vector4 vector)
    {
        return new Vector3()
        {
            X = vector.X,
            Y = vector.Y,
            Z = vector.Z,
        } / vector.W;
    }

    private Vector3 VsVn(Vector4 vector)
    {
        return new Vector3()
        {
            X = _bitmap.Width * (1 + vector.X / vector.W) * 0.5f,
            Y = _bitmap.Height * (1 - vector.Y / vector.W) * 0.5f,
            Z = (vector.Z / vector.W + 1) * 0.5f,
        };
    }

    private Random _rng = new Random(0);


    private void FloatScanLine(Triangle3 t, float[,] zBuffer, RenderObject prop, int triangleIndex, Vector3[] normals)
    {
        // discard using back-face culling
        var normal = GetNormal(t);
        float intensity = Vector3.Dot(normal, Vector3.UnitZ);

        bool isABackFace = intensity < 0;

        if (isABackFace)
            return;

        // from top to bottom
        var sorted = new[] { t.a, t.b, t.c }.OrderBy(v => v.Y).ToArray();
        var points = sorted.Select(v => new Point((int)v.X, (int)v.Y)).ToArray();

        var mTotal = GetM(points[0], points[2]);
        var mFirst = GetM(points[0], points[1]);
        var mSecond = GetM(points[1], points[2]);

        float x1 = points[0].X;
        // case when first is horizontal line
        float x2 = float.IsInfinity(mFirst) ? points[1].X : points[0].X;

        for(int y = points[0].Y; y <= points[2].Y; ++y)
        {
            bool isFirstHalf = y < points[1].Y;

            for(int x = (int)MathF.Min(x1, x2); x < MathF.Max(x1, x2); ++x)
            {
                FillPixel(x, y, t, zBuffer, intensity, prop, triangleIndex, normals);
            }

            x1 += mTotal;
            x2 += isFirstHalf ? mFirst : mSecond;
        }
    }

    // https://www.khronos.org/opengl/wiki/Calculating_a_Surface_Normal
    private static Vector3 GetNormal(Triangle3 t)
    {
        Vector3 u = t.c - t.a;
        Vector3 v = t.b - t.a;

        Vector3 normal = Vector3.Cross(u, v);

        return Vector3.Normalize(normal);
    }


    private void FillPixel(int x, int y, Triangle3 t, float[,] zBuffer, float intensity, RenderObject prop, 
        int index, Vector3[] normals)
    {
        Vector3 bary = GetBarycentric(t, x, y);

        float z = t.a.Z * bary.X + t.b.Z * bary.Y + t.c.Z * bary.Z;

        if (IsInBounds(x, y) && zBuffer[x, y] > z)
        {
            zBuffer[x, y] = z;

            if(prop.Texture != null)
            {
                Color textureColor = GetTextureColor(prop, index, bary);
                _bitmap.SetPixel(x, y, textureColor);
                //var normal = GetNormal(prop, index, bary, normals);
                //Color shadersColor = Shading(Vector3.UnitX, new Vector3(x, y, z), normal);
                //_bitmap.SetPixel(x, y, shadersColor);
            }
            else
            {
                _bitmap.SetPixel(x, y, Color.White);
            }
        }
    }

    private static Color GetTextureColor(RenderObject prop, int index, Vector3 bary)
    {
        var textureIndex = prop.TextureIndexes[index];
        var textureTriangle = RenderObject.GetTriangle3FromIndexes(textureIndex, prop.TextureCoordinates);

        var textureXs = new Vector3(textureTriangle.a.X, textureTriangle.b.X, textureTriangle.c.X);
        var textureYs = new Vector3(textureTriangle.a.Y, textureTriangle.b.Y, textureTriangle.c.Y);

        int textureX = (int)(Vector3.Dot(textureXs, bary) * (prop.Texture.Width - 1));
        int textureY = (int)((1 - Vector3.Dot(textureYs, bary)) * (prop.Texture.Height - 1));

        Color textureColor = prop.Texture.GetPixel(textureX, textureY);
        return textureColor;
    }

    private static Vector3 GetNormal(RenderObject prop, int index, Vector3 bary, Vector3[] normals)
    {
        var normalIndex = prop.NormalIndexes[index];
        var normalTriangle = RenderObject.GetTriangle3FromIndexes(normalIndex, normals);

        var xs = new Vector3(normalTriangle.a.X, normalTriangle.b.X, normalTriangle.c.X);
        var ys = new Vector3(normalTriangle.a.Y, normalTriangle.b.Y, normalTriangle.c.Y);
        var zs = new Vector3(normalTriangle.a.Z, normalTriangle.b.Z, normalTriangle.c.Z);

        var normal = new Vector3(
                Vector3.Dot(xs, bary),
                Vector3.Dot(ys, bary),
                Vector3.Dot(zs, bary)
            );

        return Vector3.Normalize(normal);
    }

    private static Vector3 GetVectorFromColor(Color color)
    {
        return new Vector3(color.R / 255, color.G / 255, color.B / 255);
    }

    private static Color GetColorFromVector(Vector3 vector)
    {
        var clamped = Vector3.Clamp(vector, Vector3.Zero, Vector3.One);
        var in255space = clamped * 255;

        return Color.FromArgb((int)in255space.X, (int)in255space.Y, (int)in255space.Z);
    }

    private float GetM(Point upper, Point lower)
    {
        float m = (float)(upper.X - lower.X) / (upper.Y - lower.Y);

        return m;
    }


    private void WireScanLine(Triangle3 t)
    {
        // from top to bottom
        var sorted = new[] { t.a, t.b, t.c }.OrderBy(v => v.Y).ToArray();
        var points = sorted.Select(v => new Point((int)v.X, (int)v.Y)).ToArray();

        var line01 = BresenhamLine(points[0], points[1]); // first half
        var line12 = BresenhamLine(points[1], points[2]); // second half
        var line02 = BresenhamLine(points[0], points[2]); // total height

        foreach (var p in line01)
        {
            _bitmap.SetPixel(p.X, p.Y, Color.Green);
        }

        foreach (var p in line02)
        {
            _bitmap.SetPixel(p.X, p.Y, Color.Blue);
        }

        foreach (var p in line12)
        {
            _bitmap.SetPixel(p.X, p.Y, Color.Red);
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _bitmap.Width && y < _bitmap.Height;
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

    private void FillNaive(Triangle3 triangle, float[,] zBuffer, RenderObject prop, int index)
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

               // FillPixel(x, y, triangle, zBuffer, 0f, prop, index);
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

        if (MathF.Abs(u.Z) < 1f)
        {
            return new Vector3(-1, 1, 1);
        }

        return new Vector3(1f - (u.X + u.Y) / u.Z, u.Y / u.Z, u.X / u.Z);
    }
}
