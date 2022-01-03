using Jednosc.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Rendering;

public class WiremeshRenderer : IRenderer
{
    private DirectBitmap _bitmap;
    private RenderScene _scene;

    public WiremeshRenderer(DirectBitmap bitmap, RenderScene scene)
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

        var newVertices = prop.Vertices.Select(x => Vs(Vn(Vector4.Transform(x, rotation * mvp)))).ToArray();

        foreach(var triangle in prop.VertexIndexes)
        {
            var points = new PointF[3];
            points[0] = newVertices[triangle.a];
            points[1] = newVertices[triangle.b]; 
            points[2] = newVertices[triangle.c];
            graphics.DrawPolygon(Pens.White, points);
        }

    }

    private static Vector4 Vn(Vector4 vector)
    {
        return vector / vector.W;
    }

    private PointF Vs(Vector4 vector)
    {
        return new PointF()
        {
            X = _bitmap.Width * (1 + vector.X) * 0.5f,
            Y = _bitmap.Height * (1 - vector.Y) * 0.5f,
        };
    }
}
