using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene;

public record struct TriangleIndexes(int a, int b, int c);
public record struct Triangle2(Vector2 a, Vector2 b, Vector2 c);
public record struct TriangleInt2(Point a, Point b, Point c);
public record struct Triangle3(Vector3 a, Vector3 b, Vector3 c);
public record struct Triangle4(Vector4 a, Vector4 b, Vector4 c);

public class RenderObject
{
    /// <summary>
    /// List of geometric vertices, with (x, y, z [,w]) coordinates, w is optional and defaults to 1.0.
    /// In .obj file the representation starts with v.
    /// </summary>
    public Vector4[] Vertices { get; init; }

    /// <summary>
    /// Indexes of vertices that form triangles.
    /// Indexes start at 0 and end at <see cref="Vertices"/> length - 1.
    /// </summary>
    public TriangleIndexes[] TriangleIndexes { get; init; }

    public Vector3[] VertexNormals { get; init; }

    /// <summary>
    /// Position of the object in the space.
    /// </summary>
    public Vector3 Position { get; set; } = Vector3.Zero;

    /// <summary>
    /// Unit vector showing direction in which object is orientated.
    /// </summary>
    public Vector3 Forward { get; set; } = Vector3.UnitX;

    /// <summary>
    /// Unit vector showing where up is. It always evaluates to <see cref="Vector3.UnitY"/>.
    /// </summary>
    public static Vector3 Up => Vector3.UnitY;

    public Matrix4x4 ModelMatrix => Matrix4x4.CreateWorld(Position, Forward, Up);

    public RenderObject()
    {
        
    }

    public static async Task<RenderObject> FromFilenameAsync(string filename)
    {
        string[] lines = await File.ReadAllLinesAsync(filename);

        var vertices = new List<Vector4>();
        var triangleIndexes = new List<TriangleIndexes>();
        var vertexNormals = new List<Vector3>();

        foreach(var line in lines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            bool isLineWorthAnalysing = parts.Length > 1;
            if (!isLineWorthAnalysing)
                continue;

            switch(parts[0])
            {
                case "v": // Vertex
                    vertices.Add(GetVertexFromParts(parts));
                    break;
                case "f":
                    triangleIndexes.Add(GetVertexIndicesFromParts(parts));
                    break;
                case "vn": // vertex normal
                    vertexNormals.Add(GetVertexNormalFromParts(parts));
                    break;
                case "vt": 
                    break;
                case "vp": // space vertices
                    break;

            }
        }

        return new RenderObject()
        {
            Vertices = vertices.ToArray(),
            TriangleIndexes = triangleIndexes.ToArray(),
            VertexNormals = vertexNormals.ToArray(),
        };
    }

    private static TriangleIndexes GetVertexIndicesFromParts(string[] parts)
    {
        var separated = parts.Skip(1).Select(x =>
            x.Split('/', StringSplitOptions.RemoveEmptyEntries).Select(y =>
                int.Parse(y) - 1 // we move indexes so that they start at 0
                ).ToArray()
            ).ToArray();

        return new TriangleIndexes(separated[0][0], separated[1][0], separated[2][0]);
    }

    private static Vector4 GetVertexFromParts(string[] array)
    {
        // default to 1
        float w = array.Length == 5 ? float.Parse(array[4]) : 1.0f;

        return new Vector4()
        {
            X = float.Parse(array[1]),
            Y = float.Parse(array[2]),
            Z = float.Parse(array[3]),
            W = w,
        };
    }

    private static Vector3 GetVertexNormalFromParts(string[] array)
    {
        var vector = GetVector3FromParts(array);
        return Vector3.Normalize(vector);
    }

    private static Vector3 GetVector3FromParts(string[] array)
    {
        return new Vector3()
        {
            X = float.Parse(array[1]),
            Y = float.Parse(array[2]),
            Z = float.Parse(array[3]),
        };
    }
}
